namespace AirlineReservationSystem.ViewModels.TicketVM
{
    public class TicketListItemVm
    {
        public int Id { get; set; }
        public string TicketNumber { get; set; } = null!;
        public string? Barcode { get; set; }
        public decimal Fare { get; set; }
        public TicketStatus Status { get; set; }
        public DateTime IssuedAt { get; set; }

        public string PassengerFullName { get; set; } = null!;
        public string BookingReference { get; set; } = null!;
        public string? FlightNumber { get; set; }
        public string? SeatNumber { get; set; }
    }
}
