
using Microsoft.EntityFrameworkCore.Storage;
using Stripe.Checkout;
using System.Linq.Expressions;

namespace AirlineReservationSystem.Services.Implementations
{
    public class BookingCustomerService : IBookingCustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFlightRepository _flightRepository;
        private readonly ITicketService _ticketService;
        private static readonly TimeSpan ReservationHold = TimeSpan.FromMinutes(10);

        private const decimal PointRedemptionRate = 10m;
        private const int PointsPerEgpEconomy = 10;
        private const int PointsPerEgpBusiness = 15;
        // سعر ثابت لكل كيلوجرام إضافي
        private const decimal PricePerKg = 15m;

       

        public BookingCustomerService( IUnitOfWork unitOfWork, IFlightRepository flightRepository,
            ITicketService ticketService)  
        {
            _unitOfWork = unitOfWork;
            _flightRepository = flightRepository;
            _ticketService = ticketService;  
        }
        public async Task<(PassengerInfoVM? Vm, string? Error)> BuildPassengerInfoFormAsync(
            int flightId, List<int> seatIds, CancellationToken cancellationToken = default)
        {
            if (seatIds is null || seatIds.Count == 0)
                return (null, "No seats were received.");

            var flight = await _flightRepository.GetOneWithIncludesAsync(
                expression: f => f.Id == flightId,
                include: q => q.Include(f => f.FlightSeats).ThenInclude(fs => fs.Seat),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (flight is null)
                return (null, $"Flight {flightId} was not found.");

            if (flight.Status != FlightStatus.Scheduled)
                return (null, $"Flight {flightId} is not Scheduled (status: {flight.Status}).");

            var flightSeatsDict = flight.FlightSeats.ToDictionary(fs => fs.Id);
            var vm = new PassengerInfoVM { FlightId = flightId };

            foreach (var seatId in seatIds)
            {
                if (!flightSeatsDict.TryGetValue(seatId, out var flightSeat))
                    return (null, $"Seat id {seatId} does not belong to flight {flightId}.");

                if (flightSeat.Status != FlightSeatStatus.Available)
                    return (null, $"Seat id {seatId} is not Available (status: {flightSeat.Status}).");

                vm.Passengers.Add(new PassengerFormItemVM
                {
                    FlightSeatId = flightSeat.Id,
                    SeatNumber = flightSeat.Seat.SeatNumber
                });
            }

            return (vm, null);
        }

        public async Task<BookingCreationResult> CreateBookingAsync(
            int flightId, List<PassengerFormItemVM> passengers,
            string userId,CancellationToken cancellationToken = default)
        {
            if (passengers is null || passengers.Count == 0)
                return new BookingCreationResult { IsSuccess = false, ErrorMessage = "No passengers provided." };

            var seatIds = passengers.Select(p => p.FlightSeatId).ToList();

            if (seatIds.Distinct().Count() != seatIds.Count)
                return new BookingCreationResult { IsSuccess = false, ErrorMessage = "Duplicate seat selection detected." };

            // فتح الـ Transaction من الـ UnitOfWork لضمان حماية العملية كلها
            await using IDbContextTransaction transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var flight = await _flightRepository.GetOneWithIncludesAsync(
                    expression: f => f.Id == flightId,
                    include: q => q.Include(f => f.FlightSeats),
                    tracked: true,
                    cancellationToken: cancellationToken
                );

                if (flight is null || flight.Status != FlightStatus.Scheduled)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new BookingCreationResult { IsSuccess = false, ErrorMessage = "This flight is no longer available." };
                }

                var flightSeatsDict = flight.FlightSeats.ToDictionary(fs => fs.Id);

                foreach (var seatId in seatIds)
                {
                    if (!flightSeatsDict.TryGetValue(seatId, out var seat) || seat.Status != FlightSeatStatus.Available)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new BookingCreationResult
                        {
                            IsSuccess = false,
                            ErrorMessage = "One or more selected seats are no longer available. Please go back and choose again."
                        };
                    }
                }

                var totalAmount = seatIds.Sum(id => flightSeatsDict[id].Price);

                var booking = new Booking
                {
                    UserId = userId,
                    FlightId = flightId,
                    Status = BookingStatus.Pending,
                    TotalAmount = totalAmount,
                    CreatedAt = DateTime.UtcNow
                };

                // إضافة الحجز في الذاكرة (بدون SaveChanges فوري)
                await _unitOfWork.Bookings.AddAsync(booking, cancellationToken);

                var reservedUntil = DateTime.UtcNow.Add(ReservationHold);

                // 1. تجميع الباسبورات لجلبها دفعة واحدة (حل مشكلة الـ N+1)
                var passportNumbers = passengers.Select(p => p.PassportNumber).ToList();

                var existingPassengersDict = (await _unitOfWork.Passengers.GetAllAsync(
                    expression: p => passportNumbers.Contains(p.PassportNumber),
                    includes: null,
                    tracked: true,
                    cancellationToken: cancellationToken
                )).ToDictionary(p => p.PassportNumber);

                foreach (var p in passengers)
                {
                    if (!existingPassengersDict.TryGetValue(p.PassportNumber, out var passenger))
                    {
                        passenger = new Passenger
                        {
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            DateOfBirth = p.DateOfBirth,
                            Gender = p.Gender,
                            Nationality = p.Nationality,
                            PassportNumber = p.PassportNumber,
                            PassportExpiryDate = p.PassportExpiryDate
                        };
                        // إضافة المسافر في الذاكرة
                        await _unitOfWork.Passengers.AddAsync(passenger, cancellationToken);
                        existingPassengersDict[p.PassportNumber] = passenger;
                    }

                    // ربط العلاقات بالكائنات مباشرة لتفادي مشكلة الـ IDs غير المولدة
                    var bookingPassenger = new BookingPassenger
                    {
                        Booking = booking,
                        Passenger = passenger,
                        FlightSeatId = p.FlightSeatId,
                        ReservedUntil = reservedUntil
                    };
                    await _unitOfWork.BookingPassengers.AddAsync(bookingPassenger, cancellationToken);

                    var flightSeat = flightSeatsDict[p.FlightSeatId];
                    flightSeat.Status = FlightSeatStatus.Reserved;
                    _unitOfWork.FlightSeats.Update(flightSeat);
                }

                //  حفظ نهائي  
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new BookingCreationResult { IsSuccess = true, BookingId = booking.Id };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
        public async Task<SelectLuggageVM?> BuildSelectLuggageFormAsync(
                   int bookingId, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetOneWithIncludesAsync(
                expression: b => b.Id == bookingId,
                include: q => q
                    .Include(b => b.BookingPassengers).ThenInclude(bp => bp.Passenger)
                    .Include(b => b.BookingPassengers).ThenInclude(bp => bp.FlightSeat).ThenInclude(fs => fs.Seat),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (booking is null || booking.Status != BookingStatus.Pending)
                return null;

            var now = DateTime.UtcNow;
            if (booking.BookingPassengers.Any(bp => bp.ReservedUntil != null && bp.ReservedUntil <= now))
                return null;

            return new SelectLuggageVM
            {
                BookingId = booking.Id,
                PricePerKg = PricePerKg,
                CurrentTotal = booking.TotalAmount,
                Passengers = booking.BookingPassengers.Select(bp => new PassengerLuggageCardVM
                {
                    BookingPassengerId = bp.Id,
                    FullName = $"{bp.Passenger.FirstName} {bp.Passenger.LastName}",
                    SeatNumber = bp.FlightSeat.Seat.SeatNumber
                }).ToList()
            };
        }

        public async Task<BookingCreationResult> AddBaggageAsync(
            int bookingId, List<PassengerBaggageSelectionVM> selections, CancellationToken cancellationToken = default)
        {
            var validSelections = selections?.Where(s => s.ExtraWeightKg > 0).ToList() ?? new();

            var booking = await _unitOfWork.Bookings.GetOneWithIncludesAsync(
                expression: b => b.Id == bookingId,
                include: q => q.Include(b => b.BookingPassengers),
                tracked: true,
                cancellationToken: cancellationToken
            );

            if (booking is null || booking.Status != BookingStatus.Pending)
                return new BookingCreationResult { IsSuccess = false, ErrorMessage = "This booking is no longer valid." };

            var now = DateTime.UtcNow;
            if (booking.BookingPassengers.Any(bp => bp.ReservedUntil != null && bp.ReservedUntil <= now))
                return new BookingCreationResult { IsSuccess = false, ErrorMessage = "Your seat reservation has expired." };

            var validBookingPassengerIds = booking.BookingPassengers.Select(bp => bp.Id).ToHashSet();

            decimal addedAmount = 0;
            var newBaggages = new List<Baggage>();

            foreach (var selection in validSelections)
            {
                if (!validBookingPassengerIds.Contains(selection.BookingPassengerId))
                    return new BookingCreationResult { IsSuccess = false, ErrorMessage = "Invalid passenger selection." };

                var price = selection.ExtraWeightKg * PricePerKg;

                newBaggages.Add(new Baggage
                {
                    BookingPassengerId = selection.BookingPassengerId,
                    ExtraWeightKg = selection.ExtraWeightKg,
                    Price = price
                });

                addedAmount += price;
            }

            if (newBaggages.Count > 0)
            {
                await _unitOfWork.Baggages.AddRangeAsync(newBaggages, cancellationToken);
            }

            booking.TotalAmount += addedAmount;
            _unitOfWork.Bookings.Update(booking);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookingCreationResult { IsSuccess = true, BookingId = booking.Id };
        }

        public async Task<CheckoutVM?> BuildCheckoutFormAsync(
          int bookingId, string userId, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetOneWithIncludesAsync(
                expression: b => b.Id == bookingId && b.UserId == userId,
                include: q => q
                    .Include(b => b.Flight).ThenInclude(f => f.DepartureAirport)
                    .Include(b => b.Flight).ThenInclude(f => f.ArrivalAirport)
                    .Include(b => b.BookingPassengers).ThenInclude(bp => bp.Passenger)
                    .Include(b => b.BookingPassengers).ThenInclude(bp => bp.FlightSeat).ThenInclude(fs => fs.Seat)
                    .Include(b => b.BookingPassengers).ThenInclude(bp => bp.Baggages),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (booking is null || booking.Status != BookingStatus.Pending)
                return null;

            var now = DateTime.UtcNow;
            if (booking.BookingPassengers.Any(bp => bp.ReservedUntil != null && bp.ReservedUntil <= now))
                return null;

            var seatsTotal = booking.BookingPassengers.Sum(bp => bp.FlightSeat.Price);
            var luggageTotal = booking.BookingPassengers.Sum(bp => bp.Baggages.Sum(bg => bg.Price));

            var loyaltyAccount = await _unitOfWork.LoyaltyAccounts.GetOneAsync(
                expression: la => la.UserId == userId,
                tracked: false,
                cancellationToken: cancellationToken
            );

            var availablePoints = loyaltyAccount?.TotalPoints ?? 0;
            var pointsValueInEgp = availablePoints / PointRedemptionRate;

            return new CheckoutVM
            {
                BookingId = booking.Id,
                BookingReference = booking.BookingReference,
                FlightNumber = booking.Flight.FlightNumber,

                DepartureAirport = $"{booking.Flight.DepartureAirport.City} ({booking.Flight.DepartureAirport.IATACode})",
                ArrivalAirport = $"{booking.Flight.ArrivalAirport.City} ({booking.Flight.ArrivalAirport.IATACode})",

                DepartureTime = booking.Flight.DepartureTime,

                Passengers = booking.BookingPassengers.Select(bp => new PassengerCheckoutItemVM
                {
                    FullName = $"{bp.Passenger.FirstName} {bp.Passenger.LastName}",
                    SeatNumber = bp.FlightSeat.Seat.SeatNumber,
                    SeatPrice = bp.FlightSeat.Price,
                    BaggageFee = bp.Baggages.Sum(bg => bg.Price)
                }).ToList(),

                SeatsAndFlightTotal = seatsTotal,
                LuggageTotal = luggageTotal,
                GrandTotal = booking.TotalAmount,

                AvailablePoints = availablePoints,
                PointsValueInEGP = pointsValueInEgp,
                CanPayWithPoints = pointsValueInEgp >= booking.TotalAmount
            };
        }

        public async Task<(string? ResultUrl, string? ErrorMessage)> ProcessPaymentAsync(
            ProcessPaymentVM vm, string userId, CancellationToken cancellationToken = default)
        {
            var booking = await _unitOfWork.Bookings.GetOneWithIncludesAsync(
                expression: b => b.Id == vm.BookingId && b.UserId == userId,
                include: q => q.Include(b => b.BookingPassengers).ThenInclude(bp => bp.FlightSeat).ThenInclude(fs => fs.Seat),
                tracked: true,
                cancellationToken: cancellationToken
            );

            if (booking is null || booking.Status != BookingStatus.Pending)
                return (null, "This booking is no longer valid.");

            var now = DateTime.UtcNow;
            if (booking.BookingPassengers.Any(bp => bp.ReservedUntil != null && bp.ReservedUntil <= now))
                return (null, "Your seat reservation has expired. Please book again.");


            // الحالة الأولى: الدفع بالنقاط (Loyalty Points) 

            if (vm.PaymentMethod == PaymentMethod.Points)
            {
                await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
                try
                {
                    var loyaltyAccount = await _unitOfWork.LoyaltyAccounts.GetOneAsync(
                        expression: la => la.UserId == userId,
                        tracked: true,
                        cancellationToken: cancellationToken
                    );

                    var availableValue = (loyaltyAccount?.TotalPoints ?? 0) / PointRedemptionRate;

                    if (loyaltyAccount is null || availableValue < booking.TotalAmount)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return (null, "Insufficient points balance.");
                    }

                    int pointsToDeduct = (int)Math.Ceiling(booking.TotalAmount * PointRedemptionRate);
                    loyaltyAccount.TotalPoints -= pointsToDeduct;
                    _unitOfWork.LoyaltyAccounts.Update(loyaltyAccount);

                    await _unitOfWork.PointTransactions.AddAsync(new PointTransaction
                    {
                        LoyaltyAccountId = loyaltyAccount.Id,
                        Points = -pointsToDeduct,
                        Description = $"Payment for booking {booking.BookingReference}"
                    }, cancellationToken);

                    // إنشاء دفعة ناجحة فوراً
                    var payment = new Payment
                    {
                        BookingId = booking.Id,
                        Amount = booking.TotalAmount,
                        PaymentMethod = PaymentMethod.Points,
                        Status = PaymentStatus.Paid,
                        TransactionReference = $"PTS-{Guid.NewGuid().ToString("N")[..12].ToUpper()}"
                    };
                    await _unitOfWork.Payments.AddAsync(payment, cancellationToken);

                    booking.Status = BookingStatus.Confirmed;
                    _unitOfWork.Bookings.Update(booking);

                    foreach (var bp in booking.BookingPassengers)
                    {
                        bp.ReservedUntil = null;
                        bp.FlightSeat.Status = FlightSeatStatus.Booked;
                    }

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    //  إصدار التذاكر تلقائيًا لكل راكب
                    foreach (var bp in booking.BookingPassengers)
                    {
                        await _ticketService.IssueTicketAsync(bp.Id, cancellationToken);
                    }

                    await transaction.CommitAsync(cancellationToken);

                    return ("PointsSuccess", null);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }


            else
            {
                // إعدادات جلسة Stripe Checkout
                var options = new SessionCreateOptions
                {
                    SuccessUrl = $"https://localhost:7117/Customer/Payment/PaymentSuccess?bookingId={booking.Id}&sessionId={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7117/Customer/Payment/Checkout?bookingId={booking.Id}",
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(booking.TotalAmount * 100), // Stripe يحسب بالقرش
                                Currency = "egp",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Flight Ticket - Ref: {booking.BookingReference}"
                                },
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options, cancellationToken: cancellationToken);


                var payment = new Payment
                {
                    BookingId = booking.Id,
                    Amount = booking.TotalAmount,
                    PaymentMethod = PaymentMethod.CreditCard,
                    Status = PaymentStatus.Pending,
                    TransactionReference = session.Id
                };

                await _unitOfWork.Payments.AddAsync(payment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // نرجع رابط صفحة الدفع الخاصة بـ Stripe عشان الـ Controller يحول العميل عليها
                return (session.Url, null);
            }
        }

        // ميثود تأكيد الحجز بعد نجاح الدفع على Stripe
        public async Task<bool> ConfirmStripePaymentAsync(int bookingId, string sessionId, CancellationToken cancellationToken = default)
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var booking = await _unitOfWork.Bookings.GetOneWithIncludesAsync(
                    expression: b => b.Id == bookingId,
                    include: q => q.Include(b => b.BookingPassengers).ThenInclude(bp => bp.FlightSeat).ThenInclude(fs => fs.Seat),
                    tracked: true,
                    cancellationToken: cancellationToken
                );

                var payment = await _unitOfWork.Payments.GetOneAsync(
                    expression: p => p.BookingId == bookingId && p.TransactionReference == sessionId,
                    tracked: true,
                    cancellationToken: cancellationToken
                );

                if (booking is null || payment is null || payment.Status == PaymentStatus.Paid)
                    return false;

                // تحديث حالة الدفع والـ Booking لـ مؤكد
                payment.Status = PaymentStatus.Paid;
                booking.Status = BookingStatus.Confirmed;

                // حساب وإضافة نقاط الولاء لأن العميل دفع بالفيزا
                var loyaltyAccount = await _unitOfWork.LoyaltyAccounts.GetOneAsync(
                    expression: la => la.UserId == booking.UserId, tracked: true, cancellationToken: cancellationToken);

                if (loyaltyAccount is not null)
                {
                    int earnedPoints = 0;
                    foreach (var bp in booking.BookingPassengers)
                    {
                        var rate = bp.FlightSeat.Seat.SeatClass == SeatClass.Business ? PointsPerEgpBusiness : PointsPerEgpEconomy;
                        earnedPoints += (int)(bp.FlightSeat.Price * rate);
                    }
                    loyaltyAccount.TotalPoints += earnedPoints;
                    _unitOfWork.LoyaltyAccounts.Update(loyaltyAccount);

                    await _unitOfWork.PointTransactions.AddAsync(new PointTransaction
                    {
                        LoyaltyAccountId = loyaltyAccount.Id,
                        Points = earnedPoints,
                        Description = $"Points earned from booking {booking.BookingReference}"
                    }, cancellationToken);
                }

                foreach (var bp in booking.BookingPassengers)
                {
                    bp.ReservedUntil = null;
                    bp.FlightSeat.Status = FlightSeatStatus.Booked;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                //إصدار التذاكر تلقائيًا لكل راكب
                foreach (var bp in booking.BookingPassengers)
                {
                    await _ticketService.IssueTicketAsync(bp.Id, cancellationToken);
                }

                await transaction.CommitAsync(cancellationToken);
                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
        }


        public async Task<List<MyBookingItemVM>> GetMyBookingsAsync(
            string userId, CancellationToken cancellationToken = default)
        {
            var bookings = await _unitOfWork.Bookings.GetAllAsync(
                expression: b => b.UserId == userId,
                includes: new Expression<Func<Booking, object>>[]
                {
            b => b.Flight,
            b => b.BookingPassengers
                },
                tracked: false,
                cancellationToken: cancellationToken
            );

            return bookings
                .OrderByDescending(b => b.CreatedAt)
                .Select(b => new MyBookingItemVM
                {
                    BookingId = b.Id,
                    BookingReference = b.BookingReference,
                    FlightNumber = b.Flight.FlightNumber,
                    DepartureTime = b.Flight.DepartureTime,
                    Status = b.Status,
                    TotalAmount = b.TotalAmount,
                    PassengerCount = b.BookingPassengers.Count
                })
                .ToList();
        }

        public async Task<PaymentSuccessDetailsVM?> GetPaymentSuccessDetailsAsync(
        int bookingId, string userId, CancellationToken cancellationToken = default)
         {
            var booking = await _unitOfWork.Bookings.GetOneWithIncludesAsync(
                expression: b => b.Id == bookingId && b.UserId == userId,
                include: q => q.Include(b => b.Payment),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (booking is null || booking.Payment is null)
                return null;

            int? remainingPoints = null;

            if (booking.Payment.PaymentMethod == PaymentMethod.Points)
            {
                var loyaltyAccount = await _unitOfWork.LoyaltyAccounts.GetOneAsync(
                    expression: la => la.UserId == userId,
                    tracked: false,
                    cancellationToken: cancellationToken
                );

                remainingPoints = loyaltyAccount?.TotalPoints ?? 0;
            }

            return new PaymentSuccessDetailsVM
            {
                BookingId = booking.Id,
                BookingReference = booking.BookingReference,
                AmountPaid = booking.Payment.Amount,
                PaymentMethod = booking.Payment.PaymentMethod,
                RemainingPoints = remainingPoints
            };
        }
    }
}