
namespace AirlineReservationSystem.Models.Entities;

public class BookingPassenger : AuditableEntity //راكبًا داخل حجز معين مع المقعد المخصص له والتذكره الخاصه به
{
    public int BookingId { get; set; }

    public int PassengerId { get; set; }

    public int FlightSeatId { get; set; }

    public Booking Booking { get; set; } = null!;

    public Passenger Passenger { get; set; } = null!;

    public FlightSeat FlightSeat { get; set; } = null!; //كل راكب يجلس في مقعد مختلف

    public Ticket? Ticket { get; set; }//لأن التذكرة تخص راكبًا معينًا داخل حجز معين وعلى مقعد معين.
}