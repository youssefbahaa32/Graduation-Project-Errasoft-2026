

namespace AirlineReservationSystem.Models.Entities;

public class Aircraft : AuditableEntity
{
    public string RegistrationNumber { get; set; } = null!;

    public string Manufacturer { get; set; } = null!;

    public string Model { get; set; } = null!;

    public int Capacity { get; set; }

    public AircraftStatus Status { get; set; }

    public ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();

    public ICollection<Flight> Flights { get; set; } = new HashSet<Flight>();
}