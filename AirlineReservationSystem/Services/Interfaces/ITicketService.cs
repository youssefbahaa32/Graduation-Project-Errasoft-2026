using AirlineReservationSystem.Common;
using AirlineReservationSystem.ViewModels.TicketVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface ITicketService
    {

        Task<TicketIndexVm> SearchAsync(
                   string? ticketNumber,
                   string? passengerName,
                   TicketStatus? status,
                   int page,
                   int pageSize,
                   SortOrder sortOrder,
                   CancellationToken cancellationToken = default);

        Task<TicketDetailsVm?> GetDetailsVmAsync(int id, CancellationToken cancellationToken = default);
        Task<Ticket> IssueTicketAsync(int bookingPassengerId, CancellationToken cancellationToken = default);
        Task<bool> CancelTicketAsync(int id, CancellationToken cancellationToken = default);
        Task<bool> ReinstateAsync(int id, CancellationToken cancellationToken = default);
    }
}
    


