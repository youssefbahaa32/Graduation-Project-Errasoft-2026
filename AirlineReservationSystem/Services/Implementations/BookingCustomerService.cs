using AirlineReservationSystem.ViewModels.LuggageCustomerVM;
using AirlineReservationSystem.ViewModels.PassengerCustomerVM;
using Microsoft.EntityFrameworkCore.Storage;

namespace AirlineReservationSystem.Services.Implementations
{
    public class BookingCustomerService: IBookingCustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFlightRepository _flightRepository;
        private static readonly TimeSpan ReservationHold = TimeSpan.FromMinutes(10);

        // سعر ثابت لكل كيلوجرام إضافي
        private const decimal PricePerKg = 15m;

        public BookingCustomerService(IUnitOfWork unitOfWork, IFlightRepository flightRepository)
        {
            _unitOfWork = unitOfWork;
            _flightRepository = flightRepository;
        }

        public async Task<PassengerInfoVM?> BuildPassengerInfoFormAsync(
            int flightId, List<int> seatIds, CancellationToken cancellationToken = default)
        {
            if (seatIds is null || seatIds.Count == 0) return null;

            var flight = await _flightRepository.GetOneWithIncludesAsync(
                expression: f => f.Id == flightId,
                include: q => q.Include(f => f.FlightSeats).ThenInclude(fs => fs.Seat),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (flight is null || flight.Status != FlightStatus.Scheduled)
                return null;

            var flightSeatsDict = flight.FlightSeats.ToDictionary(fs => fs.Id);
            var vm = new PassengerInfoVM { FlightId = flightId };

            foreach (var seatId in seatIds)
            {
                if (!flightSeatsDict.TryGetValue(seatId, out var flightSeat) ||
                    flightSeat.Status != FlightSeatStatus.Available)
                {
                    return null;
                }

                vm.Passengers.Add(new PassengerFormItemVM
                {
                    FlightSeatId = flightSeat.Id,
                    SeatNumber = flightSeat.Seat.SeatNumber
                });
            }

            return vm;
        }

        public async Task<BookingCreationResult> CreateBookingAsync(
            int flightId,
            List<PassengerFormItemVM> passengers,
            string userId,
            CancellationToken cancellationToken = default)
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

    }
}

