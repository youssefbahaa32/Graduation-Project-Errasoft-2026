
namespace AirlineReservationSystem.Models.Entities;

public class BookingPassenger : AuditableEntity //راكبًا داخل حجز معين مع المقعد المخصص له والتذكره الخاصه به بالاضافه للحقائب معلومات
{
    public int BookingId { get; set; }

    public int PassengerId { get; set; }

    public int FlightSeatId { get; set; }

    public Booking Booking { get; set; } = null!;

    public Passenger Passenger { get; set; } = null!;

    public FlightSeat FlightSeat { get; set; } = null!;

    public Ticket? Ticket { get; set; }

    public ICollection<Baggage> Baggages { get; set; }
        = new HashSet<Baggage>();
}