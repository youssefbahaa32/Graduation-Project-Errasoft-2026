
namespace AirlineReservationSystem.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGenericRepository<ApplicationUserOTP> _applicationUserOTPRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUnitOfWork _unitOfWork;
        public AccountService(UserManager<ApplicationUser> userManager,
            IEmailSender emailSender, IUnitOfWork unitOfWork,
            IGenericRepository<ApplicationUserOTP> applicationUserOTPRepository )
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
            _applicationUserOTPRepository = applicationUserOTPRepository;
        }

        public async Task<bool> SendOTPMailAsync(ApplicationUser user)
        {
            try
            {
                var otp = new Random().Next(1000, 9999);

                await _emailSender.SendEmailAsync(user.Email, "Reset Password Your Account",
                    $"<h1>OTP: <b>{otp}</b> .Don't Shar it.</h1>");

                //هخزن الotp فى الداتا بيز
                await _applicationUserOTPRepository.AddAsync(new ApplicationUserOTP()
                {
                    OTP = otp.ToString(),
                    ApplicationUserId = user.Id
                });
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }

        }


        public async Task<bool> SendConfirmationMailAsync(ApplicationUser user, IUrlHelper url, HttpRequest request)
        {
            try
            {
                //send confirm

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user); //by default  => token valid to 24h

                var link = url.Action("Confirm", "Account", new
                {
                    area = "Identity",
                    token = token,
                    userId = user.Id
                }, request.Scheme);


                await _emailSender.SendEmailAsync(user.Email, "Confirmation Yor Account",
                    $"<h1>Confirm Your Account By Clicking</h1><a href='{link}'> here</a> ");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

    }
}
