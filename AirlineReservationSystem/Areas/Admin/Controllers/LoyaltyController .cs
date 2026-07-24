using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Loyalty;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoyaltyController : Controller
    {
        private readonly ILoyaltyService _loyaltyService;

        public LoyaltyController(ILoyaltyService loyaltyService)
        {
            _loyaltyService = loyaltyService;
        }

        #region Index

        public async Task<IActionResult> Index(
            LoyaltyIndexVM vm,
            CancellationToken cancellationToken)
        {
            vm = await _loyaltyService.GetAllAsync(
                vm,
                cancellationToken);

            return View(vm);
        }

        #endregion

        #region Details

        public async Task<IActionResult> Details(
            int id,
            CancellationToken cancellationToken)
        {
            var vm = await _loyaltyService.GetByIdAsync(
                id,
                cancellationToken);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        #endregion

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken)
        {
            var vm = await _loyaltyService.GetForCreateAsync(
                cancellationToken);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            LoyaltyCreateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                vm = await _loyaltyService.GetForCreateAsync(
                    cancellationToken);

                return View(vm);
            }

            await _loyaltyService.CreateAsync(
                vm,
                cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken cancellationToken)
        {
            var vm = await _loyaltyService.GetForEditAsync(
                id,
                cancellationToken);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            LoyaltyUpdateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var editVm = await _loyaltyService.GetForEditAsync(
                    vm.Id,
                    cancellationToken);

                if (editVm == null)
                    return NotFound();

                return View(editVm);
            }

            var updated = await _loyaltyService.UpdateAsync(
                vm,
                cancellationToken);

            if (!updated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var deleted = await _loyaltyService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}