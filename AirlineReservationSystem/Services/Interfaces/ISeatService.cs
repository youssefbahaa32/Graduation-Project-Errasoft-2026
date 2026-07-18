using AirlineReservationSystem.ViewModels.Seat;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface ISeatService
    {
        Task<SeatIndexVM> GetAllAsync(
            SeatIndexVM vm,
            CancellationToken cancellationToken = default);

        Task<SeatCreateVM> GetForCreateAsync(
            CancellationToken cancellationToken = default);

        Task<SeatUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task CreateAsync(
            SeatCreateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            SeatUpdateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}