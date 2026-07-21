
namespace AirlineReservationSystem.Models.Entities;

public class Airport : AuditableEntity//مطار
{
    public string Name { get; set; } = null!;

    public string IATACode { get; set; } = null!;// IATA code is a 3-letter code designating many airports around the world, defined by the International Air Transport Association (IATA).

    public string ICAOCode { get; set; } = null!;// ICAO code is a four-letter alphanumeric code designating each airport around the world, defined by the International Civil Aviation Organization (ICAO).

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;
    public AirportStatus Status { get; set; } = AirportStatus.Active;
    public ICollection<AirportImage> Images { get; set; } = new HashSet<AirportImage>();

    // Navigation properties
    public ICollection<Flight> DepartureFlights { get; set; } = new HashSet<Flight>();

    public ICollection<Flight> ArrivalFlights { get; set; } = new HashSet<Flight>();
}