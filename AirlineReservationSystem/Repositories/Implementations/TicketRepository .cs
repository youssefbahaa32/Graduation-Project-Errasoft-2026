using AirlineReservationSystem.Common;

namespace AirlineReservationSystem.Repositories.Implementations
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(ApplicationDbContext context) : base(context)
        {
        }

        private IQueryable<Ticket> WithDetails() =>
            _dbSet
                .Include(t => t.BookingPassenger)
                    .ThenInclude(bp => bp.Passenger)
                .Include(t => t.BookingPassenger)
                    .ThenInclude(bp => bp.Booking)
                .Include(t => t.BookingPassenger)
                    .ThenInclude(bp => bp.FlightSeat)
                        .ThenInclude(fs => fs.Flight)
                .Include(t => t.BookingPassenger)
                    .ThenInclude(bp => bp.FlightSeat)
                        .ThenInclude(fs => fs.Seat);

        public async Task<Ticket?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            return await WithDetails().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<(IEnumerable<Ticket> Items, int TotalCount)> SearchAsync(
            string? ticketNumber,
            string? passengerName,
            TicketStatus? status,
            int page,
            int pageSize,
            SortOrder sortOrder,
            CancellationToken cancellationToken = default)
        {
            var query = WithDetails();

            if (!string.IsNullOrWhiteSpace(ticketNumber))
                query = query.Where(t => t.TicketNumber.Contains(ticketNumber));

            if (!string.IsNullOrWhiteSpace(passengerName))
                query = query.Where(t => t.BookingPassenger.Passenger.FirstName.Contains(passengerName));

            if (status.HasValue)
                query = query.Where(t => t.Status == status.Value);

            query = sortOrder == SortOrder.Ascending
                ? query.OrderBy(t => t.IssuedAt)
                : query.OrderByDescending(t => t.IssuedAt);

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Ticket?> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default)
        {
            return await WithDetails().FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber, cancellationToken);
        }

        public async Task<bool> ExistsByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(t => t.TicketNumber == ticketNumber, cancellationToken);
        }

        public async Task<bool> ExistsForBookingPassengerAsync(int bookingPassengerId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(t => t.BookingPassengerId == bookingPassengerId, cancellationToken);
        }
    }


}


