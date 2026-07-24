namespace AirlineReservationSystem.ViewModels.PaymentCustomerVM
{
    public class PaymentResultVM
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int? BookingId { get; set; }
    }
}
