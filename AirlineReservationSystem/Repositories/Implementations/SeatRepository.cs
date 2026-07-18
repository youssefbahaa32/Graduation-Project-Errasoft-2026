

namespace AirlineReservationSystem.Repositories.Implementations
{
    public class SeatRepository
        : GenericRepository<Seat>, ISeatRepository
    {
        public SeatRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}