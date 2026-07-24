namespace AirlineReservationSystem.ViewModels.PaymentCustomerVM
{
    public class CheckoutVM
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string DepartureAirport { get; set; } = null!;
        public string ArrivalAirport { get; set; } = null!;
        public DateTime DepartureTime { get; set; }

        public List<PassengerCheckoutItemVM> Passengers { get; set; } = new();

        public decimal SeatsAndFlightTotal { get; set; }
        public decimal LuggageTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public int AvailablePoints { get; set; }
        public decimal PointsValueInEGP { get; set; }
        public bool CanPayWithPoints { get; set; }
    }
}
