public class FlightRepository
    : GenericRepository<Flight>, IFlightRepository
{
    public FlightRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}