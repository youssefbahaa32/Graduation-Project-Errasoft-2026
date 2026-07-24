
namespace AirlineReservationSystem.ViewModels.Flight
{
    public class FlightDetailsVM
    {
        public int Id { get; set; }

        public string FlightNumber { get; set; } = null!;

        public string Aircraft { get; set; } = null!;

        public string DepartureAirport { get; set; } = null!;

        public string ArrivalAirport { get; set; } = null!;

        public DateTime DepartureTime { get; set; }

        public DateTime ArrivalTime { get; set; }

        public decimal BasePrice { get; set; }

        public FlightStatus Status { get; set; }
        public List<FlightImageVM> Images { get; set; } = [];
    }
}