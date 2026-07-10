
namespace AirlineReservationSystem.ViewModels.Seat
{
    public class SeatCreateVM
    {
        [Required]
        public int AircraftId { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Seat Number")]
        public string SeatNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Seat Class")]
        public SeatClass SeatClass { get; set; }
    }
}