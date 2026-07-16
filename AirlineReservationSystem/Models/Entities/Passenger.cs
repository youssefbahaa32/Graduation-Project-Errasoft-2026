
namespace AirlineReservationSystem.Models.Entities;

public class Passenger : AuditableEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string Nationality { get; set; } = null!;

    public string PassportNumber { get; set; } = null!;

    public DateTime PassportExpiryDate { get; set; }

    // Navigation
    public ICollection<BookingPassenger> BookingPassengers { get; set; }
        = new HashSet<BookingPassenger>();

   
}