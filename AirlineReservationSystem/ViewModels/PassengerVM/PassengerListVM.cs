namespace AirlineReservationSystem.ViewModels.PassengerVM
{
    public class PassengerListVM
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public DateTime PassportExpiryDate { get; set; }
        public string? ImageUrl { get; set; }
    }
}
