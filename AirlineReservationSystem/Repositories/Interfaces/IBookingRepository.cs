namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {     
        Task<IEnumerable<Booking>> GetAllWithBasicInfoAsync( CancellationToken cancellationToken = default);

        Task<IEnumerable<Booking>> GetAllWithDetailsAsync(
            CancellationToken cancellationToken = default);


        Task<Booking?> GetBookingWithDetailsAsync(
            int bookingId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<Booking>> GetBookingsByStatusAsync(
            BookingStatus status,
            CancellationToken cancellationToken = default);

        Task<Booking?> GetByReferenceAsync(
            string bookingReference,
            CancellationToken cancellationToken = default);


        Task<bool> HasCompletedPaymentAsync(
            int bookingId,
            CancellationToken cancellationToken = default);
    }
}
