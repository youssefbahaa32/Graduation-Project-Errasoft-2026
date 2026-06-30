
namespace AirlineReservationSystem.Models.Entities;

public class Airport : AuditableEntity
{
    public string Name { get; set; } = null!;

    public string IATACode { get; set; } = null!;

    public string ICAOCode { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public ICollection<Flight> DepartureFlights { get; set; } = new HashSet<Flight>();

    public ICollection<Flight> ArrivalFlights { get; set; } = new HashSet<Flight>();
}