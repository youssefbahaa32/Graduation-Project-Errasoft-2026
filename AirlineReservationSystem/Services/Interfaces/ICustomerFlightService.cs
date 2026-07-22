using AirlineReservationSystem.ViewModels.FlightCustomerVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface ICustomerFlightService
    {
        Task<List<FlightSearchResultVm>> SearchAsync(FlightSearchVM vm, CancellationToken cancellationToken = default);
        Task<FlightDetailsVm?> GetDetailsAsync(int id, CancellationToken cancellationToken = default);
    }
}

