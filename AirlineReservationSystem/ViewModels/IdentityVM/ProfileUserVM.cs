namespace AirlineReservationSystem.ViewModels.IdentityVM
{
    public class ProfileUserVM
    {
        public required ApplicationUserVM ApplicationUserVM { get; set; }
        public required ChangeCurrentPasswordVM ChangeCurrentPasswordVM { get; set; }
    }
}
