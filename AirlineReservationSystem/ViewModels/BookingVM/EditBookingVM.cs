namespace AirlineReservationSystem.ViewModels.BookingVM
{
    public class EditBookingVM
    {
        public int Id { get; set; }

        public string BookingReference { get; set; } = null!;

        public string CustomerName { get; set; } = null!;

        public string FlightNumber { get; set; } = null!;

        [Display(Name = "Booking Status")]
        [Required]
        public BookingStatus Status { get; set; }
    }
}
