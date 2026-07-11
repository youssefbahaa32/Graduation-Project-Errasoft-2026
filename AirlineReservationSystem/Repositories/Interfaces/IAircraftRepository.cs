namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {
        
        Task<bool> HasFlightsAsync(int aircraftId, CancellationToken cancellationToken = default);
    }
}
