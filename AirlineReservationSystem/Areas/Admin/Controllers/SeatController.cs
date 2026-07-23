using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Seat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class SeatController : Controller
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        #region Index

        public async Task<IActionResult> Index(
            SeatIndexVM vm,
            CancellationToken cancellationToken)
        {
            var seats = await _seatService.GetAllAsync(
                vm,
                cancellationToken);

            return View(seats);
        }

        #endregion

        #region Create

        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken)
        {
            var vm = await _seatService.GetForCreateAsync(
                cancellationToken);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(
            SeatCreateVM model,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                model = await _seatService.GetForCreateAsync(
                    cancellationToken);

                return View(model);
            }

            await _seatService.CreateAsync(
                model,
                cancellationToken);

            TempData["Success"] = "Seat created successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken cancellationToken)
        {
            var seat = await _seatService.GetForEditAsync(
                id,
                cancellationToken);

            if (seat == null)
                return NotFound();

            return View(seat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(
            SeatUpdateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var editVm = await _seatService.GetForEditAsync(
                vm.Id,
                cancellationToken);

                if (editVm == null)
                    return NotFound();

                return View(editVm);

            }

            var updated = await _seatService.UpdateAsync(
                vm,
                cancellationToken);

            if (!updated)
                return NotFound();

            TempData["Success"] = "Seat updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var deleted = await _seatService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
                return NotFound();

            TempData["Success"] = "Seat deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}