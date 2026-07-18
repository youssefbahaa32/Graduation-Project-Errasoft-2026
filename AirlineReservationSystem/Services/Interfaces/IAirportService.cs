
using AirlineReservationSystem.ViewModels.AirportVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IAirportService
    {
        Task<AirportIndexVM> GetAllAsync(AirportIndexVM vm,
            CancellationToken cancellationToken = default);

        Task<AirportDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<AirportUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task CreateAsync(
            AirportCreateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            AirportUpdateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}