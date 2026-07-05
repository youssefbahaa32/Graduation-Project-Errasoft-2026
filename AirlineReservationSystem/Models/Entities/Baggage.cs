

namespace AirlineReservationSystem.Models.Entities;

public class Baggage : AuditableEntity
{
    public int PassengerId { get; set; }

    public BaggageType Type { get; set; }

    public decimal Weight { get; set; }

    public decimal Price { get; set; }

    public Passenger Passenger { get; set; } = null!;
}