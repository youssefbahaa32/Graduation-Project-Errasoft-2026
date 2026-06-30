namespace AirlineReservationSystem.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        // Navigation Property
        public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
    }
}