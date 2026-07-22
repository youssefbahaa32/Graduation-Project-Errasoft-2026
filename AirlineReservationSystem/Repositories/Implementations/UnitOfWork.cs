public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IAirportRepository Airports { get; }

    public IAircraftRepository Aircrafts { get; }

    public ISeatRepository Seats { get; }

    public IFlightRepository Flights { get; }

    public IBookingRepository Bookings { get; }
    
    public IImageRepository Images { get; }

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
    }

    public async Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}