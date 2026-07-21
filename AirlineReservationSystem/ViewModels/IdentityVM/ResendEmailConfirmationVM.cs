namespace AirlineReservationSystem.ViewModels.IdentityVM
{
    public class ResendEmailConfirmationVM
    {
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}
