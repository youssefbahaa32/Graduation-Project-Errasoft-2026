using AirlineReservationSystem.ViewModels.BookingVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IBookingService
    {
        Task<IEnumerable<Booking>> GetAllAsync( CancellationToken cancellationToken = default);

        Task<BookingListVM> GetPagedListAsync(BookingFilterVM filter, CancellationToken cancellationToken = default);

        Task<Booking?> GetDetailsAsync( int bookingId, CancellationToken cancellationToken = default);

   
        Task<IEnumerable<Booking>> GetByStatusAsync( BookingStatus status, CancellationToken cancellationToken = default);


        Task ChangeStatusAsync( int bookingId, BookingStatus newStatus, CancellationToken cancellationToken = default);

        Task CancelAsync( int bookingId, CancellationToken cancellationToken = default);

    }
}
