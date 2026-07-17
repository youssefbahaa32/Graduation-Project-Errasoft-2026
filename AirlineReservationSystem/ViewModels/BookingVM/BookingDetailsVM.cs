namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class BookingDetailsVM  //يمثل صفحة التفاصيل بالكامل
    {
        public int Id { get; set; }

        public string BookingReference { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string CustomerEmail { get; set; } = null!;

        public string FlightNumber { get; set; } = null!;

        public decimal TotalAmount { get; set; }

        public BookingStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public PaymentVM? Payment { get; set; }

        public IEnumerable<BookingPassengerVM> Passengers { get; set; }
            = Enumerable.Empty<BookingPassengerVM>();
    }
}
