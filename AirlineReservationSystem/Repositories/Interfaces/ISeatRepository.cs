using AirlineReservationSystem.Models.Entities;

namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        Task<IEnumerable<Seat>> GetAvailableSeatsAsync(
         int flightId,
         CancellationToken cancellationToken = default);
    }
}