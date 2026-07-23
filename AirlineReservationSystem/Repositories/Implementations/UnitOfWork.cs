using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IAirportRepository Airports { get; }
    public IAircraftRepository Aircrafts { get; }
    public ISeatRepository Seats { get; }
    public IFlightRepository Flights { get; }
    public IBookingRepository Bookings { get; }
    public IImageRepository Images { get; }

    // 1. تعريف الـ Generic Repositories للجداول الجديدة
    public IGenericRepository<Passenger> Passengers { get; }
    public IGenericRepository<BookingPassenger> BookingPassengers { get; }
    public IGenericRepository<FlightSeat> FlightSeats { get; }

    public IGenericRepository<Baggage> Baggages { get; }

    public UnitOfWork(
        ApplicationDbContext context,
        IAirportRepository airports,
        IAircraftRepository aircrafts,
        ISeatRepository seats,
        IFlightRepository flights,
        IBookingRepository bookings,
        IImageRepository images)
    {
        _context = context;

        Airports = airports;
        Aircrafts = aircrafts;
        Seats = seats;
        Flights = flights;
        Bookings = bookings;
        Images = images;

        Passengers = new GenericRepository<Passenger>(_context);
        BookingPassengers = new GenericRepository<BookingPassenger>(_context);
        FlightSeats = new GenericRepository<FlightSeat>(_context);
        Baggages = new GenericRepository<Baggage>(_context); 
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await _context.Database.BeginTransactionAsync(cancellationToken);

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}