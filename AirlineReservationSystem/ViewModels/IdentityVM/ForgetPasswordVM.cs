namespace AirlineReservationSystem.ViewModels
{
    public class ForgetPasswordVM
    {
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}
