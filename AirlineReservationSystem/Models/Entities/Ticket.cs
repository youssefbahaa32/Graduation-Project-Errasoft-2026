

namespace AirlineReservationSystem.Models.Entities;

public class Ticket : AuditableEntity
{
    public string TicketNumber { get; set; } = null!;

    public int BookingId { get; set; }

    public int PassengerId { get; set; }

    public TicketStatus Status { get; set; }

    public Booking Booking { get; set; } = null!;

    public Passenger Passenger { get; set; } = null!;
}