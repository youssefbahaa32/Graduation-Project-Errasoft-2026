

namespace AirlineReservationSystem.ViewModels.Flight
{
    public class FlightUpdateVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FlightNumber { get; set; } = null!;

        [Required]
        public int AircraftId { get; set; }

        [Required]
        public int DepartureAirportId { get; set; }

        [Required]
        public int ArrivalAirportId { get; set; }

        [Required]
        public DateTime DepartureTime { get; set; }

        [Required]
        public DateTime ArrivalTime { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal BasePrice { get; set; }

        [Required]
        public FlightStatus Status { get; set; }
    }
}