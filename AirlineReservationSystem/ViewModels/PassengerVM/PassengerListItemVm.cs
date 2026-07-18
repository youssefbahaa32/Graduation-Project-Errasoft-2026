namespace AirlineReservationSystem.ViewModels.PassengerVM
{
    public class PassengerListItemVm
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string PassportNumber { get; set; } = null!;
        public string Nationality { get; set; } = null!;
        public DateTime PassportExpiryDate { get; set; }
    }
}
