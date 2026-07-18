

namespace AirlineReservationSystem.Models.Entities;

public class Seat : AuditableEntity //المقعد داخل الطائرة، له رقم، نوع، وحالة."static seat"
{
    public int AircraftId { get; set; }

    public string SeatNumber { get; set; } = null!;

    public SeatClass SeatClass { get; set; }
    // Navigation property
    public Aircraft Aircraft { get; set; } = null!;

    public ICollection<FlightSeat> FlightSeats { get; set; } = new HashSet<FlightSeat>();
}