

namespace AirlineReservationSystem.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class AccountController : Controller
    {
        private readonly IGenericRepository<ApplicationUserOTP> _applicationUserOTPRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountService _accountService;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAccountService accountService,
            IGenericRepository<ApplicationUserOTP> applicationUserOTPRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountService = accountService;
            _applicationUserOTPRepository = applicationUserOTPRepository;
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = new ApplicationUser()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
        
            };


            var result = await _userManager.CreateAsync(user, registerVM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                return View(registerVM);
            }

            //send confirm

            await _accountService.SendConfirmationMailAsync(user, Url, Request);

            TempData["success-notification"] = "Create Account Successfully , please check your email to verfiy";
            await _userManager.AddToRoleAsync(user, SD.CUSTOMER_ROLE);

            return RedirectToAction(nameof(Login));
        }


        public async Task<IActionResult> Confirm(string token, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return NotFound();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                TempData["error-notification"] = string.Join(",",
                    result.Errors.Select(e => e.Description));

                return RedirectToAction(nameof(Login));
            }

            TempData["success-notification"] = "Active Account Successfully";

            return RedirectToAction(nameof(Login));
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail) ??
                await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);

            if (user is null)
            {
                ModelState.AddModelError("UserNameOrEmail", "Email or UserName Incorrect");
                ModelState.AddModelError("Password", "Password Incorrect");

                return View(loginVM);
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password,
                loginVM.RememberMe, true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("UserNameOrEmail", "Email or UserName Incorrect");
                ModelState.AddModelError("Password", "Password Incorrect");

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Too many attempts , please try again later.");
                }
                return View(loginVM);
            }

            TempData["success-notification"] = $"Welcome Back {user.FirstName} {user.LastName}";


            return RedirectToAction("Index", "Flight", new { area = "Customer" });

        }


        [HttpGet]
        public IActionResult ResendEmailConfirmation()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationVM resendEmailConfirmationVM)
        {
            if (!ModelState.IsValid)
                return View(resendEmailConfirmationVM);

            var user = await _userManager.FindByEmailAsync(resendEmailConfirmationVM.EmailOrUserName) ??
            await _userManager.FindByNameAsync(resendEmailConfirmationVM.EmailOrUserName);

            if (user is not null)
            {
                await _accountService.SendConfirmationMailAsync(user, Url, Request);
            }


            TempData["success-notification"] = "Send Confirmation Mail ,Check Your Mail";
            return RedirectToAction(nameof(Login));
        }








        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM forgetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(forgetPasswordVM);

            var user = await _userManager.FindByEmailAsync(forgetPasswordVM.EmailOrUserName) ??
                     await _userManager.FindByNameAsync(forgetPasswordVM.EmailOrUserName);

            if (user is not null)
            {
                await _accountService.SendOTPMailAsync(user);
            }

            TempData["success-notification"] = "Send OTP Number To Your Mail ,Check Your Mail";

            TempData["userId"] = user.Id;
            return RedirectToAction(nameof(ValidateOTP));
        }

        [HttpGet]
        public IActionResult ValidateOTP()
        {
            if (TempData.Peek("userId") is null)
                return NotFound();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ValidateOTP(ValidateOTPVM validateOTPVM)
        {
            if (!ModelState.IsValid)
                return View(validateOTPVM);

            var userId = TempData.Peek("userId");

            var otpInDB = (await _applicationUserOTPRepository.GetAllAsync(e => e.ApplicationUserId ==
            userId.ToString() && !e.IsUsed))
            .OrderByDescending(e => e.CreateAt).FirstOrDefault();

            if (otpInDB.OTP != validateOTPVM.OTP)
            {
                TempData["error-notification"] = "In Valid or Expire OTP.";
                return View();
            }
            return RedirectToAction(nameof(ChangePassword));

        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            if (TempData.Peek("userId") is null)
                return NotFound();

            return View();

        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM changePasswordVM)
        {
            if (!ModelState.IsValid)
                return View(changePasswordVM);

            var user = await _userManager.FindByIdAsync(TempData["userId"].ToString());
            if (user == null) return NotFound();

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, changePasswordVM.Password);

            if (!result.Succeeded)
            {
                TempData["error-notification"] = string.Join(",",
                  result.Errors.Select(e => e.Description));

                TempData["userId"] = user.Id;
                return View();
            }
            TempData["success-notification"] = "Change Password Successfully";
            return RedirectToAction(nameof(Login));

        }


        // 1. الأكشن الأول: توجيه المستخدم لجوجل
        [HttpPost]
        public IActionResult ExternalLogin(string provider = "Google", string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return Challenge(properties, provider);
        }


        // 2. الأكشن الثاني: استقبال المستخدم بعد ما يرجع من جوجل
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null)
        {
            // المعلومات اللى جايه من جوجل
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Login");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return LocalRedirect(returnUrl);

                return RedirectToAction("Index", "Home", new { area = "Customer" });
            }

            // 1) لازم يكون فيه إيميل جاي من جوجل قبل ما نكمل
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
            var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);

            // أول مرة يدخل بالطريقة دي → اعمله حساب جديد أو اربطه بإيميله لو موجود
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = true // جاي من Google يبقى موثوق
                };

                // 2) نتأكد إن الإنشاء نجح فعلاً قبل ما نكمل أي خطوة تانية
                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    return RedirectToAction("Login");
                }
            }

            // AspNetUserLoginsاربط الحساب بمزوّدانى بضيفه فى جدول ال Google عشان المرات الجاية يدخل على طول
            await _userManager.AddLoginAsync(user, info);

            await _signInManager.SignInAsync(user, isPersistent: false);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Flight", new { area = "Customer" });
        }




        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            TempData["success-notification"] = "Sign Out Successfully.";

            return RedirectToAction(nameof(Login));
        }

    }
}
