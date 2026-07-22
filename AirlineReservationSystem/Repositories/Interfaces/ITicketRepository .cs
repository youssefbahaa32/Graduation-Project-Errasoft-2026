using AirlineReservationSystem.Common;

namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
           Task<Ticket?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);

        Task<(IEnumerable<Ticket> Items, int TotalCount)> SearchAsync(
            string? ticketNumber,
            string? passengerName,
            TicketStatus? status,
            int page,
            int pageSize,
            SortOrder sortOrder,
            CancellationToken cancellationToken = default);

        Task<Ticket?> GetByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default);
        Task<bool> ExistsByTicketNumberAsync(string ticketNumber, CancellationToken cancellationToken = default);
        Task<bool> ExistsForBookingPassengerAsync(int bookingPassengerId, CancellationToken cancellationToken = default);
    }

}

