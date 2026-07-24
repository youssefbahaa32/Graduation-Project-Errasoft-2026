namespace AirlineReservationSystem.ViewModels.PaymentCustomerVM
{
    public class PassengerCheckoutItemVM
    {
        public string FullName { get; set; } = null!;
        public string SeatNumber { get; set; } = null!;
        public decimal SeatPrice { get; set; }
        public decimal BaggageFee { get; set; }
    }
}
