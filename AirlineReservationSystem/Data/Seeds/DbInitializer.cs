public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // 1. ملى المطارات والطيارات الأساسية أولاً
        await SeedAirportsAsync(context);
        await SeedAircraftsAsync(context);

        // 2. ملى دورة الحجز الكاملة (تذكرة، مسافر، دفع، إلخ) لو مش موجودة
        await SeedBookingCycleAsync(context);
    }

    private static async Task SeedAirportsAsync(ApplicationDbContext context)
    {
        // مطار القاهرة (CAI) - تم تعديل الشرط ليتأكد من الكودين منعاً للتكرار
        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'CAI' OR ICAOCode = 'HECA')
            BEGIN
                INSERT INTO Airports (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
                VALUES ('Cairo International Airport', 'CAI', 'HECA', 'Cairo', 'Egypt', 0, GETUTCDATE(), 0);
            END
            ");

        // مطار دبي (DXB) - عشان يكون عندك مطار الوصول جاهز للرحلات
        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'DXB' OR ICAOCode = 'OMDB')
            BEGIN
                INSERT INTO Airports (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
                VALUES ('Dubai International Airport', 'DXB', 'OMDB', 'Dubai', 'United Arab Emirates', 0, GETUTCDATE(), 0);
            END
            ");
    }

    private static async Task SeedBookingCycleAsync(ApplicationDbContext context)
    {
        // لو جدول الحجوزات فاضي، هننشئ الدورة كاملة متربطة ببعضها
        if (!await context.Bookings.AnyAsync())
        {
            // جلب المطارين والطيارة اللي لسه ضايفينهم فوق عشان نربط بيهم
            var departureAirport = await context.Airports.FirstOrDefaultAsync(a => a.IATACode == "CAI");
            var arrivalAirport = await context.Airports.FirstOrDefaultAsync(a => a.IATACode == "DXB");
            var aircraft = await context.Set<Aircraft>().FirstOrDefaultAsync(a => a.RegistrationNumber == "SU-GDX");

            // لو ملقيناش الطيارة المعينة دي، ننشئها عشان نضمن الربط
            if (aircraft == null)
            {
                aircraft = new Aircraft
                {
                    RegistrationNumber = "SU-GDX",
                    Manufacturer = "Boeing",
                    Model = "777-300ER",
                    Capacity = 346,
                    Status = AircraftStatus.Active
                };
                context.Set<Aircraft>().Add(aircraft);
            }

            // إنشاء مستخدم للتجربة
            var user = new ApplicationUser
            {
                UserName = "passenger.test@example.com",
                Email = "passenger.test@example.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Doe"
            };
            context.Users.Add(user);

            // إنشاء الرحلة وربطها بالمطارين والطيارة
            var flight = new Flight
            {
                FlightNumber = "MS-777",
                DepartureAirport = departureAirport,
                ArrivalAirport = arrivalAirport,
                Aircraft = aircraft,
                DepartureTime = DateTime.UtcNow.AddDays(2),
                BasePrice = 400.00m
            };
            context.Flights.Add(flight);

            // إنشاء المقعد
            var seat = new Seat
            {
                Aircraft = aircraft,
                SeatNumber = "12A",
                SeatClass = SeatClass.Economy
            };
            context.Seats.Add(seat);

            // إنشاء FlightSeat
            var flightSeat = new FlightSeat
            {
                Flight = flight,
                Seat = seat,
                Price = 450.00m,
                Status = FlightSeatStatus.Reserved
            };
            context.Set<FlightSeat>().Add(flightSeat);

            // إنشاء الحجز
            var booking = new Booking
            {
                User = user,
                Flight = flight,
                TotalAmount = 450.00m,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };
            context.Bookings.Add(booking);

            // إنشاء بيانات الدفع
            var payment = new Payment
            {
                Booking = booking,
                Amount = 450.00m,
                PaymentMethod = PaymentMethod.CreditCard,
                Status = PaymentStatus.Paid,
                TransactionReference = "TXN-11223344"
            };
            context.Payments.Add(payment);

            // إنشاء المسافر
            var passenger = new Passenger
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                DateOfBirth = new DateTime(1990, 1, 1),
                PassportNumber = "A12345678",
                Gender = "Male",
                Nationality = "Egyptian",
            };
            context.Passengers.Add(passenger);

            // إنشاء تفاصيل ربط المسافر بالحجز
            var bookingPassenger = new BookingPassenger
            {
                Booking = booking,
                Passenger = passenger,
                FlightSeat = flightSeat
            };
            context.BookingPassengers.Add(bookingPassenger);

            // إنشاء التذكرة
            var ticket = new Ticket
            {
                TicketNumber = "TK-12345",
                Fare = 150.00m,
                Status = TicketStatus.Issued,
                BookingPassenger = bookingPassenger
            };
            context.Tickets.Add(ticket);

            // حفظ كل البيانات المترابطة دي في الـ Database مع بعض بـ Transaction واحدة أوتوماتيك
            await context.SaveChangesAsync();
        }
    }

    private static async Task SeedAircraftsAsync(ApplicationDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GDX')
            BEGIN
                INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
                VALUES ('777-300ER', 'Boeing', 'SU-GDX', 346, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GCM')
            BEGIN
                INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
                VALUES ('737-800', 'Boeing', 'SU-GCM', 189, 0);
            END

            IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GCN')
            BEGIN
                INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
                VALUES ('737-800', 'Boeing', 'SU-GCN', 189, 0);
            END
            -- باقي الطائرات بتاعتك مفيش مشكلة تفضل هنا...
            ");
    }
}
