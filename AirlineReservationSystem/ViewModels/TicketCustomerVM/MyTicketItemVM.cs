namespace AirlineReservationSystem.ViewModels.TicketCustomerVM
{
    public class MyTicketItemVM
    {
        public int TicketId { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string SeatNumber { get; set; } = null!;
        public TicketStatus Status { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
