

namespace AirlineReservationSystem.Models.Entities;

public class Baggage : AuditableEntity
{
    public int BookingId { get; set; }

    public BaggageType Type { get; set; }

    public decimal Weight { get; set; }

    public decimal Price { get; set; }

    public Booking Booking { get; set; } = null!;
}