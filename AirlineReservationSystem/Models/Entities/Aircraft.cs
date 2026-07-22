namespace AirlineReservationSystem.Models.Entities;

public class Aircraft : AuditableEntity // طائرة
{
    public string RegistrationNumber { get; set; } = null!; // رقم التسجيل (لوحة الطائرة الفريدة)

    public string Manufacturer { get; set; } = null!; // الشركة المصنعة (Boeing, Airbus)

    public string Model { get; set; } = null!; // طراز الطائرة (737-800, A320)

    public int Capacity { get; set; } // السعة الإجمالية للركاب

    public AircraftStatus Status { get; set; } // حالة الطائرة الحالية (متاحة، صيانة، إلخ)
    public ICollection<AircraftImage> Images { get; set; } = new HashSet<AircraftImage>();

    // العلاقات (Navigation Properties)
    public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>(); // مقاعد هذه الطائرة

    public ICollection<Flight> Flights { get; set; } = new HashSet<Flight>(); // الرحلات التي قامت/ستقوم بها الطائرة
}