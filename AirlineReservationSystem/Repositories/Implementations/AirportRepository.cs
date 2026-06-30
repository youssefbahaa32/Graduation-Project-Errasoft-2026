namespace AirlineReservationSystem.Repositories.Implementations
{
    public class AirportRepository : IAirportRepository
    {
        protected readonly ApplicationDbContext _context;
        public AirportRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
