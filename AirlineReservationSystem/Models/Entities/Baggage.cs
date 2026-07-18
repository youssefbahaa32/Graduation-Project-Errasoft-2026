

namespace AirlineReservationSystem.Models.Entities;

public class Baggage : AuditableEntity
{
    public string BagTagNumber { get; set; } = null!; // رقم الباركود الفريد للملصق

    public BaggageType Type { get; set; }

    public decimal Weight { get; set; }

    public decimal Price { get; set; }

    // علاقة الحقيبة بالمسافر (كل حقيبة تخص مسافر واحد)
    public int BookingPassengerId { get; set; }
    public BookingPassenger BookingPassenger { get; set; } = null!;

    // علاقة الحقيبة بالرحلة (الحقيبة تشحن على رحلة معينة)
    public int FlightId { get; set; }
    public Flight Flight { get; set; } = null!;
}