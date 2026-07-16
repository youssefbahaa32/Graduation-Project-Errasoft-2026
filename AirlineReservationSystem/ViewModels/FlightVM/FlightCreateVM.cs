

namespace AirlineReservationSystem.ViewModels.Flight
{
    public class FlightCreateVM
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "Flight Number")]
        public string FlightNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Aircraft")]
        public int AircraftId { get; set; }

        [Required]
        [Display(Name = "Departure Airport")]
        public int DepartureAirportId { get; set; }

        [Required]
        [Display(Name = "Arrival Airport")]
        public int ArrivalAirportId { get; set; }

        [Required]
        [Display(Name = "Departure Time")]
        public DateTime DepartureTime { get; set; }

        [Required]
        [Display(Name = "Arrival Time")]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        [Display(Name = "Base Price")]
        public decimal BasePrice { get; set; }

        [Required]
        public FlightStatus Status { get; set; }
    }
}