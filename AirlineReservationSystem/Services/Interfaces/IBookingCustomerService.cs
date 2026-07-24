using AirlineReservationSystem.ViewModels.LuggageCustomerVM;
using AirlineReservationSystem.ViewModels.PassengerCustomerVM;
using AirlineReservationSystem.ViewModels.PaymentCustomerVM;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface IBookingCustomerService
    {
        Task<(PassengerInfoVM? Vm, string? Error)> BuildPassengerInfoFormAsync(
            int flightId, List<int> seatIds, CancellationToken cancellationToken = default);

        Task<BookingCreationResult> CreateBookingAsync(
            int flightId, List<PassengerFormItemVM> passengers, string userId,
            CancellationToken cancellationToken = default);

        Task<SelectLuggageVM?> BuildSelectLuggageFormAsync(
      int bookingId, CancellationToken cancellationToken = default);

        Task<BookingCreationResult> AddBaggageAsync(
            int bookingId, List<PassengerBaggageSelectionVM> selections,
            CancellationToken cancellationToken = default);

       
        Task<CheckoutVM?> BuildCheckoutFormAsync(
            int bookingId, string userId, CancellationToken cancellationToken = default);

       
        Task<(string? ResultUrl, string? ErrorMessage)> ProcessPaymentAsync(
            ProcessPaymentVM vm, string userId, CancellationToken cancellationToken = default);

        // ميثود جديدة لتأكيد الدفع بعد عودة العميل من Stripe
        Task<bool> ConfirmStripePaymentAsync(int bookingId, string sessionId, CancellationToken cancellationToken = default);

        Task<List<MyBookingItemVM>> GetMyBookingsAsync(string userId, CancellationToken cancellationToken = default);

        Task<PaymentSuccessDetailsVM?> GetPaymentSuccessDetailsAsync(
    int bookingId, string userId, CancellationToken cancellationToken = default);
    }
}

