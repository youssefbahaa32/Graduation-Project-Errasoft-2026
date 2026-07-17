namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class PaymentVM
    {
        public decimal Amount { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public PaymentStatus Status { get; set; }

        public string TransactionReference { get; set; } = null!;
    }
}
