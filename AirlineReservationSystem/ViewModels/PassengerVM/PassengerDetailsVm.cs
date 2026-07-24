namespace AirlineReservationSystem.ViewModels.PassengerVM
{
    public class PassengerDetailsVM
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public DateTime PassportExpiryDate { get; set; }
        public List<string> BookingReferences { get; set; } = new();
        public List<PassengerImageVM> Images { get; set; } = [];
    }
}
