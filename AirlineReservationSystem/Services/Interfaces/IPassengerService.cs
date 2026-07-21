namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IPassengerService
    {
        Task<(IEnumerable<Passenger> Items, int TotalCount)> GetAllActivePassengersAsync(
             string? searchPassport = null, int pageNumber = 1, int pageSize = 10,CancellationToken ct = default);
        Task<Passenger?> GetPassengerDetailsWithHistoryAsync(int id, CancellationToken ct = default);
        Task<Passenger?> GetByIdAsync(int id, CancellationToken ct = default);
        Task AddPassengerAsync(PassengerCreateVM vm, CancellationToken ct = default);
        Task<bool> UpdatePassengerAsync(PassengerUpdateVM vm, CancellationToken ct = default);
        Task SoftDeletePassengerAsync(int id, CancellationToken ct = default);
    }
}
