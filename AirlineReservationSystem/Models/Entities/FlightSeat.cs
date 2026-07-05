

namespace AirlineReservationSystem.Models.Entities;

public class FlightSeat : AuditableEntity// حالة المقعد في الرحله معينه، هل هو محجوز، متاح، أو غير متاح.
{
    public int FlightId { get; set; }

    public int SeatId { get; set; }

    public decimal Price { get; set; } // Price of the seat for this specific flight سعر في رحله معينه مش في كل الرحلات

    public SeatStatus Status { get; set; }

    public Flight Flight { get; set; } = null!;

    public Seat Seat { get; set; } = null!;
}