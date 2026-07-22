public  class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManger;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManger,
        ApplicationDbContext context, ILogger<DbInitializer> logger)
    {
        _roleManager = roleManager;
        _userManger = userManger;
        _context = context;
        _logger = logger;
    }

    
    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAirportsAsync();
        await SeedAircraftsAsync();
        await SeedBookingCycleAsync();
    }

    private async Task SeedRolesAsync()
    {
        try
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.SUPER_ADMIN_ROLE));
                await _roleManager.CreateAsync(new IdentityRole(SD.ADMIN_ROLE));
                await _roleManager.CreateAsync(new IdentityRole(SD.CUSTOMER_ROLE));
                await _roleManager.CreateAsync(new IdentityRole(SD.EMPLOYEE_ROLE));

                var superAdmin = new ApplicationUser()
                {
                    Email = "SuperAdmin@AirLine.com",
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin"
                };

                var result = await _userManger.CreateAsync(superAdmin, password: "Admin123$");

                if (result.Succeeded)
                {
                    await _userManger.AddToRoleAsync(superAdmin, SD.SUPER_ADMIN_ROLE);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}");
        }
    }

  
    private async Task SeedAirportsAsync()
    {
        await _context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'CAI' OR ICAOCode = 'HECA')
            BEGIN
                INSERT INTO Airports (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
                VALUES ('Cairo International Airport', 'CAI', 'HECA', 'Cairo', 'Egypt', 0, GETUTCDATE(), 0);
            END
            ");

        await _context.Database.ExecuteSqlRawAsync(@"
            IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'DXB' OR ICAOCode = 'OMDB')
            BEGIN
                INSERT INTO Airports (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
                VALUES ('Dubai International Airport', 'DXB', 'OMDB', 'Dubai', 'United Arab Emirates', 0, GETUTCDATE(), 0);
            END
            ");
    }

 
    private async Task SeedBookingCycleAsync()
    {
        if (!await _context.Bookings.AnyAsync())
        {
            var departureAirport = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == "CAI");
            var arrivalAirport = await _context.Airports.FirstOrDefaultAsync(a => a.IATACode == "DXB");
            var aircraft = await _context.Set<Aircraft>().FirstOrDefaultAsync(a => a.RegistrationNumber == "SU-GDX");

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
                _context.Set<Aircraft>().Add(aircraft);
            }

            var user = new ApplicationUser
            {
                UserName = "passenger.test@example.com",
                Email = "passenger.test@example.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Doe"
            };
            _context.Users.Add(user);

            var flight = new Flight
            {
                FlightNumber = "MS-777",
                DepartureAirport = departureAirport,
                ArrivalAirport = arrivalAirport,
                Aircraft = aircraft,
                DepartureTime = DateTime.UtcNow.AddDays(2),
                BasePrice = 400.00m
            };
            _context.Flights.Add(flight);

            var seat = new Seat
            {
                Aircraft = aircraft,
                SeatNumber = "12A",
                SeatClass = SeatClass.Economy
            };
            _context.Seats.Add(seat);

            var flightSeat = new FlightSeat
            {
                Flight = flight,
                Seat = seat,
                Price = 450.00m,
                Status = FlightSeatStatus.Reserved
            };
            _context.Set<FlightSeat>().Add(flightSeat);

            var booking = new Booking
            {
                User = user,
                Flight = flight,
                TotalAmount = 450.00m,
                Status = BookingStatus.Confirmed,
                CreatedAt = DateTime.UtcNow
            };
            _context.Bookings.Add(booking);

            var payment = new Payment
            {
                Booking = booking,
                Amount = 450.00m,
                PaymentMethod = PaymentMethod.CreditCard,
                Status = PaymentStatus.Paid,
                TransactionReference = "TXN-11223344"
            };
            _context.Payments.Add(payment);

            var passenger = new Passenger
            {
                FirstName = "Ahmed",
                LastName = "Ali",
                DateOfBirth = new DateTime(1990, 1, 1),
                PassportNumber = "A12345678",
                Gender = "Male",
                Nationality = "Egyptian",
            };
            _context.Passengers.Add(passenger);

            var bookingPassenger = new BookingPassenger
            {
                Booking = booking,
                Passenger = passenger,
                FlightSeat = flightSeat
            };
            _context.BookingPassengers.Add(bookingPassenger);

            var ticket = new Ticket
            {
                TicketNumber = "TK-12345",
                Fare = 150.00m,
                Status = TicketStatus.Issued,
                BookingPassenger = bookingPassenger
            };
            _context.Tickets.Add(ticket);

            await _context.SaveChangesAsync();
        }
    }

   
    private async Task SeedAircraftsAsync()
    {
        await _context.Database.ExecuteSqlRawAsync(@"
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
            ");
    }
}
