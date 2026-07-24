namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class BookingPassengerVM  //يمثل راكبًا واحدًا داخل الحجز.
    {
        public string FullName { get; set; } = null!;

        public string PassportNumber { get; set; } = null!;

        public string? SeatNumber { get; set; } = null!;

        public string? SeatClass { get; set; } = null!;

        public string? TicketNumber { get; set; }

        public int BaggagesCount { get; set; }
    }
}
