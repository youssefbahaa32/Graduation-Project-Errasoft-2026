namespace AirlineReservationSystem.ViewModels.TicketCustomerVM
{
    public class TicketDetailsCustomerVM
    {
        public int TicketId { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string? Barcode { get; set; }
        public decimal Fare { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime IssuedAt { get; set; }

        public string PassengerFullName { get; set; } = null!;
        public string BookingReference { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string SeatNumber { get; set; } = null!;
    }
}
