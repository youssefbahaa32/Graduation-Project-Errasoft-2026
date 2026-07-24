using AirlineReservationSystem.ViewModels.TicketCustomerVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface ITicketCustomerService
    {
        Task<List<MyTicketItemVM>> GetMyTicketsAsync(string userId, CancellationToken cancellationToken = default);

        Task<TicketDetailsCustomerVM?> GetTicketDetailsAsync(int ticketId, string userId, CancellationToken cancellationToken = default);
    }
}
    

