using AirlineReservationSystem.ViewModels.Loyalty;
using AirlineReservationSystem.ViewModels.LoyaltyAccountVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface ILoyaltyService
    {
        Task<LoyaltyIndexVM> GetAllAsync(
            LoyaltyIndexVM vm,
            CancellationToken cancellationToken = default);

        Task<LoyaltyDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<LoyaltyCreateVM> GetForCreateAsync(
            CancellationToken cancellationToken = default);

        Task<LoyaltyUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task CreateAsync(
            LoyaltyCreateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            LoyaltyUpdateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}