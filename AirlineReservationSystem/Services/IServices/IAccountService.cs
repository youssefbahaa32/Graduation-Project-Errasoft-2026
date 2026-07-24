namespace AirlineReservationSystem.Services.IServices
{
    public interface IAccountService
    {
        Task<bool> SendOTPMailAsync(ApplicationUser user);

        Task<bool> SendConfirmationMailAsync(ApplicationUser user, IUrlHelper url, HttpRequest request);
    }

}
