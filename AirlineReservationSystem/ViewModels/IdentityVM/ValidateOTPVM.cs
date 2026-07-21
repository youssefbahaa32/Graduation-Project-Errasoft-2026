namespace AirlineReservationSystem.ViewModels.IdentityVM
{
    public class ValidateOTPVM
    {
        [Required]
        public string OTP { get; set; } = string.Empty;
    }
}
