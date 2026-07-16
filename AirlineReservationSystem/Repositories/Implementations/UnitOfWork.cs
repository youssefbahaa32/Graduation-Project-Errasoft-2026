

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IAirportRepository Airports { get; }

    public IFlightRepository Flights { get; }

    public IBookingRepository Bookings { get; }

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;

        Airports = new AirportRepository(context);
        Flights = new FlightRepository(context);
        Bookings = new BookingRepository(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()//هذا الميثود يقوم بتحرير الموارد المستخدمة من قبل الكائن UnitOfWork عند الانتهاء من استخدامه.
    {
        _context.Dispose();
    }
}