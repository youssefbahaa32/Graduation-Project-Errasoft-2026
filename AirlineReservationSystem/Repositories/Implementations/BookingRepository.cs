namespace AirlineReservationSystem.Repositories.Implementations
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Booking>> GetAllWithBasicInfoAsync(  CancellationToken cancellationToken = default)
        {
            return await _context.Bookings

                .Include(b => b.User)

                .Include(b => b.Flight)

                .Include(b => b.Payment)

                .OrderByDescending(b => b.CreatedAt)

                .AsNoTracking()

                .ToListAsync(cancellationToken);
        }

  
        public async Task<IEnumerable<Booking>> GetAllWithDetailsAsync( CancellationToken cancellationToken = default)
        {
            return await _context.Bookings

                .Include(b => b.User)

                .Include(b => b.Flight)

                .Include(b => b.Payment)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Passenger)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.FlightSeat)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Ticket)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Baggages)

                .OrderByDescending(b => b.CreatedAt)

                .AsNoTracking()

                .ToListAsync(cancellationToken);
        }


        public async Task<Booking?> GetBookingWithDetailsAsync( int bookingId, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings

                .Include(b => b.User)

                .Include(b => b.Flight)

                .Include(b => b.Payment)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Passenger)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.FlightSeat)
                    .ThenInclude(fs => fs.Seat)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Ticket)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Baggages)

                .FirstOrDefaultAsync(
                    b => b.Id == bookingId,
                    cancellationToken);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByStatusAsync( BookingStatus status, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings

                .Include(b => b.User)

                .Include(b => b.Flight)

                .Include(b => b.Payment)

                .Where(b => b.Status == status)

                .OrderByDescending(b => b.CreatedAt)

                .AsNoTracking()

                .ToListAsync(cancellationToken);
        }

        public async Task<Booking?> GetByReferenceAsync(string bookingReference, CancellationToken cancellationToken = default)
        {
            return await _context.Bookings

                .Include(b => b.User)

                .Include(b => b.Flight)

                .Include(b => b.Payment)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Passenger)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.FlightSeat)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Ticket)

                .Include(b => b.BookingPassengers)
                    .ThenInclude(bp => bp.Baggages)

                .FirstOrDefaultAsync(
                    b => b.BookingReference == bookingReference,
                    cancellationToken);
        }

        public async Task<bool> HasCompletedPaymentAsync( int bookingId, CancellationToken cancellationToken = default)
        {
            return await _context.Payments
                .AnyAsync(
                    p => p.BookingId == bookingId &&
                         p.Status == PaymentStatus.Paid,
                    cancellationToken);
        }
    }
}
