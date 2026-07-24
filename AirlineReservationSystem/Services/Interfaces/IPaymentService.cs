namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IPaymentService
    {

        Task<(IEnumerable<Payment> Items, int TotalCount)> GetAllPaymentsAsync(
            string? searchTransactionRef = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default);

        Task<Payment?> GetDetailsAsync(int id, CancellationToken ct = default);

        Task<Payment?> GetByIdAsync(int id, CancellationToken ct = default);

        Task AddPaymentAsync(Payment payment, CancellationToken ct = default);

        Task UpdatePaymentAsync(Payment payment, CancellationToken ct = default);

        Task DeletePaymentAsync(int id, CancellationToken ct = default);

        Task<List<Booking>> GetBookingsForLookupAsync(int? excludeCurrentBookingId = null, CancellationToken ct = default);
    }
}
