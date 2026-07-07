

namespace AirlineReservationSystem.Models.Entities;

public class Ticket : AuditableEntity
{
    [Required]
    [MaxLength(20)]
    public string TicketNumber { get; set; } = null!;

    public int BookingPassengerId { get; set; }

    public TicketStatus Status { get; set; }

    public BookingPassenger BookingPassenger { get; set; } = null!;

}