using AirlineReservationSystem.ViewModels.Flight;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IFlightService
    {
        Task<FlightIndexVM> GetAllAsync(
            FlightIndexVM vm,
            CancellationToken cancellationToken = default);

        Task<FlightDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<FlightCreateVM> GetForCreateAsync(
            CancellationToken cancellationToken = default);

        Task<FlightUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task CreateAsync(
            FlightCreateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            FlightUpdateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}