using AirlineReservationSystem.ViewModels.IdentityVM;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Identity.Controllers
{
    [Area(SD.IDENTITY_AREA)]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null) return NotFound();
            var userData = new ApplicationUserVM()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,         
            };

            return View(new ProfileUserVM
            {
                ApplicationUserVM = userData,
                ChangeCurrentPasswordVM = new ChangeCurrentPasswordVM()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(ApplicationUserVM applicationUserVM)
        {
            if (!ModelState.IsValid)
                return View("Index", applicationUserVM);

            var user = await _userManager.GetUserAsync(User);

            if (user is null) return NotFound();

            user.Email = applicationUserVM.Email;
            user.FirstName = applicationUserVM.FirstName;
            user.LastName = applicationUserVM.LastName;
            user.PhoneNumber = applicationUserVM.PhoneNumber;
            await _userManager.UpdateAsync(user);

            TempData["success-notification"] = "Update Profile Successfully .";

            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> ChangeCurrentPassword(ChangeCurrentPasswordVM changeCurrentPasswordVM)
        {
            if (!ModelState.IsValid)
                return View("Index", changeCurrentPasswordVM);

            var user = await _userManager.GetUserAsync(User);
            if (user is null) return NotFound();


            var result = await _userManager.ChangePasswordAsync(user,
                 changeCurrentPasswordVM.CurrentPassword,
               changeCurrentPasswordVM.NewPassword);

            if (!result.Succeeded)
            {
                TempData["error-notification"] = string.Join(",",
                  result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(Index));
            }
            TempData["success-notification"] = "Change Password Successfully";
            return RedirectToAction(nameof(Index));

        }
    }
}
