namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IAircraftService
    {
        Task<AircraftIndexVM> GetAllAsync(AircraftIndexVM vm,
            CancellationToken cancellationToken = default);

        Task<AircraftDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<AircraftUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task CreateAsync(
            AircraftCreateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> UpdateAsync(
            AircraftUpdateVM vm,
            CancellationToken cancellationToken = default);

        Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
