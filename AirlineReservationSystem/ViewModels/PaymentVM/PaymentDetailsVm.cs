namespace AirlineReservationSystem.ViewModels.PaymentVM
{
    public class PaymentDetailsVm
    {

        public int Id { get; set; }
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = null!;
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        public string TransactionReference { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
