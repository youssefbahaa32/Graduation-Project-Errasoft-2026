using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Flight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class FlightController : Controller
    {
        private readonly IFlightService _flightService;

        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;
        }

        #region Index

        public async Task<IActionResult> Index(
            FlightIndexVM vm,
            CancellationToken cancellationToken)
        {
            vm = await _flightService.GetAllAsync(
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
            var flight = await _flightService.GetByIdAsync(
                id,
                cancellationToken);

            if (flight == null)
                return NotFound();

            return View(flight);
        }

        #endregion

        #region Create

        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(
            CancellationToken cancellationToken)
        {
            var vm = await _flightService.GetForCreateAsync(
                cancellationToken);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(
            FlightCreateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var freshData  = await _flightService.GetForCreateAsync(
                    cancellationToken);
                vm.Airports = freshData.Airports;
                vm.Aircrafts = freshData.Aircrafts;

                return View(vm);
            }

            await _flightService.CreateAsync(
                vm,
                cancellationToken);

            TempData["Success"] = "Flight created successfully.";

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
            var vm = await _flightService.GetForEditAsync(
                id,
                cancellationToken);

            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(
            FlightUpdateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                var model = await _flightService.GetForEditAsync(
                    vm.Id,
                    cancellationToken);

                return View(model);
            }

            var updated = await _flightService.UpdateAsync(
                vm,
                cancellationToken);

            if (!updated)
                return NotFound();

            TempData["Success"] = "Flight updated successfully.";

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
            var deleted = await _flightService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
                return NotFound();

            TempData["Success"] = "Flight deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}