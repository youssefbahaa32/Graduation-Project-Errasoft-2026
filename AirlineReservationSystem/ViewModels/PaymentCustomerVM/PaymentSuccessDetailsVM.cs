namespace AirlineReservationSystem.ViewModels.PaymentCustomerVM
{
    public class PaymentSuccessDetailsVM
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = null!;
        public decimal AmountPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        // بتتملى بس لو الدفع كان بالنقاط
        public int? RemainingPoints { get; set; }
    }
}
