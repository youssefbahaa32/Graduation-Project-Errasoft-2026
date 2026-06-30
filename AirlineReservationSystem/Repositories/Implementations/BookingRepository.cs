namespace AirlineReservationSystem.Repositories.Implementations
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ApplicationDbContext _context;
        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
