

namespace AirlineReservationSystem.Models.Entities;

public class Baggage : AuditableEntity
{
    public decimal ExtraWeightKg { get; set; } // كام كيلو زيادة طلبها المسافر

    public decimal Price { get; set; }   // السعر المحسوب لهذا الوزن (Snapshot وقت الشراء)

   
    public int BookingPassengerId { get; set; }
    public BookingPassenger BookingPassenger { get; set; } = null!;


}