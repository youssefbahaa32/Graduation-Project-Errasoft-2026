

namespace AirlineReservationSystem.Models.Entities;

public class Flight : AuditableEntity
{
    public string FlightNumber { get; set; } = null!;

    public int AircraftId { get; set; }

    public int DepartureAirportId { get; set; }

    public int ArrivalAirportId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public decimal BasePrice { get; set; }

    public FlightStatus Status { get; set; }
    public double DistanceKm { get; set; }

    // Navigation properties
    public ICollection<Booking> Bookings { get; set; }
    = new HashSet<Booking>();
    public Airport DepartureAirport { get; set; } = null!;
    public Airport ArrivalAirport { get; set; } = null!;
    public Aircraft Aircraft { get; set; } = null!;
    public ICollection<FlightSeat> FlightSeats { get; set; } = new HashSet<FlightSeat>();
}