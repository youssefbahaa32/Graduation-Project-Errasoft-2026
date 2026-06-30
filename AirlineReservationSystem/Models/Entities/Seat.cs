

namespace AirlineReservationSystem.Models.Entities;

public class Seat : AuditableEntity
{
    public int AircraftId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public SeatClass SeatClass { get; set; }

    public Aircraft Aircraft { get; set; } = null!;

    public ICollection<FlightSeat> FlightSeats { get; set; } = new HashSet<FlightSeat>();
}