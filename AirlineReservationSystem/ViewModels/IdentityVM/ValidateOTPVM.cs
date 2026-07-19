namespace AirlineReservationSystem.ViewModels
{
    public class ValidateOTPVM
    {
        [Required]
        public string OTP { get; set; } = string.Empty;
    }
}
