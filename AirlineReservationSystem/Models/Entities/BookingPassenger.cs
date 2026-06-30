
namespace AirlineReservationSystem.Models.Entities;

public class BookingPassenger : AuditableEntity
{
    public int BookingId { get; set; }

    public int PassengerId { get; set; }

    public int FlightSeatId { get; set; }

    public Booking Booking { get; set; } = null!;

    public Passenger Passenger { get; set; } = null!;

    public FlightSeat FlightSeat { get; set; } = null!;
}