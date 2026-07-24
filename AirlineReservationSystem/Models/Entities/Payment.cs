

namespace AirlineReservationSystem.Models.Entities;

public class Payment : AuditableEntity
{
    public int BookingId { get; set; }

    public decimal Amount { get; set; }

    public PaymentMethod PaymentMethod { get; set; }

    public PaymentStatus Status { get; set; }
    //stripاللى هيجى من الSessionId دا هيكون
    public string TransactionReference { get; set; } = null!;

    public Booking Booking { get; set; } = null!;
}