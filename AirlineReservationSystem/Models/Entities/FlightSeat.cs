

namespace AirlineReservationSystem.Models.Entities;

public class FlightSeat : AuditableEntity
{
    public int FlightId { get; set; }

    public int SeatId { get; set; }

    public decimal Price { get; set; }

    public SeatStatus Status { get; set; }

    public Flight Flight { get; set; } = null!;

    public Seat Seat { get; set; } = null!;
}