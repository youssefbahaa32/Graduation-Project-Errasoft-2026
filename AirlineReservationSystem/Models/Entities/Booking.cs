

namespace AirlineReservationSystem.Models.Entities;

public class Booking : AuditableEntity
{
    public string BookingReference { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public int FlightId { get; set; }

    public decimal TotalAmount { get; set; }

    public BookingStatus Status { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public Flight Flight { get; set; } = null!;

    public ICollection<BookingPassenger> BookingPassengers { get; set; }
        = new HashSet<BookingPassenger>();

    public ICollection<Payment> Payments { get; set; }
        = new HashSet<Payment>();

    public ICollection<Ticket> Tickets { get; set; }
        = new HashSet<Ticket>();

    public ICollection<Baggage> Baggages { get; set; }
        = new HashSet<Baggage>();
}