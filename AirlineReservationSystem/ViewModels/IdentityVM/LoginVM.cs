namespace AirlineReservationSystem.ViewModels.IdentityVM
{
    public class LoginVM
    {
        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }  // by default : 10 day
    }
}
