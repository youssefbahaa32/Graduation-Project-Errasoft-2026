namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class BookingIndexVM
    {
        public int Id { get; set; }

        public string BookingReference { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string FlightNumber { get; set; } = null!;

        public int PassengersCount { get; set; }

        public decimal TotalAmount { get; set; }

        public BookingStatus Status { get; set; }

        public PaymentStatus? PaymentStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
