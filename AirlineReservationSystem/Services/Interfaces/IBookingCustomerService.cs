using AirlineReservationSystem.ViewModels.LuggageCustomerVM;
using AirlineReservationSystem.ViewModels.PassengerCustomerVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IBookingCustomerService
    {
        Task<PassengerInfoVM?> BuildPassengerInfoFormAsync(
    int flightId, List<int> seatIds, CancellationToken cancellationToken = default);

        Task<BookingCreationResult> CreateBookingAsync(
            int flightId, List<PassengerFormItemVM> passengers, string userId,
            CancellationToken cancellationToken = default);

        Task<SelectLuggageVM?> BuildSelectLuggageFormAsync(
      int bookingId, CancellationToken cancellationToken = default);

        Task<BookingCreationResult> AddBaggageAsync(
            int bookingId, List<PassengerBaggageSelectionVM> selections,
            CancellationToken cancellationToken = default);
    }
}
