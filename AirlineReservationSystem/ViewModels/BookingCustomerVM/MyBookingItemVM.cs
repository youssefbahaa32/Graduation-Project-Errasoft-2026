namespace AirlineReservationSystem.ViewModels.BookingCustomerVM
{
    public class MyBookingItemVM
    {
        public int BookingId { get; set; }
        public string BookingReference { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public BookingStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int PassengerCount { get; set; }
    }
}
