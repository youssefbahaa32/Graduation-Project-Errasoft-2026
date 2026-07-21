namespace AirlineReservationSystem.ViewModels.IdentityVM
{
    public class ForgetPasswordVM
    {
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}
