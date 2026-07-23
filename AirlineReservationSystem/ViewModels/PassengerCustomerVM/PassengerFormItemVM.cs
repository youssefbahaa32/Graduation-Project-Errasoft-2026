namespace AirlineReservationSystem.ViewModels.PassengerCustomerVM
{
    public class PassengerFormItemVM
    {
        public int FlightSeatId { get; set; }
        public string SeatNumber { get; set; } = null!;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Nationality is required.")]
        public string Nationality { get; set; } = null!;

        [Required(ErrorMessage = "Passport number is required.")]
        public string PassportNumber { get; set; } = null!;

        [Required(ErrorMessage = "Passport expiry date is required.")]
        [DataType(DataType.Date)]
        public DateTime PassportExpiryDate { get; set; }
    }
}
