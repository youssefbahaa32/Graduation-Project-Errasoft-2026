namespace AirlineReservationSystem.Repositories.Implementations
{
    public class FlightRepository : IFlightRepository
    {
        protected readonly ApplicationDbContext _context;
        public FlightRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
