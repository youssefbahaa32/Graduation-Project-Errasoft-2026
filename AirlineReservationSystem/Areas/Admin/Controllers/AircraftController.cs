using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Aircraft;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AircraftController : Controller
    {
        private readonly IAircraftService _aircraftService;

        public AircraftController(IAircraftService aircraftService)
        {
            _aircraftService = aircraftService;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(
            AircraftIndexVM vm,
            CancellationToken cancellationToken)
        {
            var model = await _aircraftService.GetAllAsync(
                vm,
                cancellationToken);

            return View(model);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(
            int id,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest();

            var aircraft = await _aircraftService.GetByIdAsync(
                id,
                cancellationToken);

            if (aircraft == null)
                return NotFound();

            return View(aircraft);
        }

        #endregion

        #region Create

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            AircraftCreateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _aircraftService.CreateAsync(
                vm,
                cancellationToken);

            TempData["Success"] = "Aircraft created successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> Edit(
            int id,
            CancellationToken cancellationToken)
        {
            if (id <= 0)
                return BadRequest();

            var aircraft = await _aircraftService.GetForEditAsync(
                id,
                cancellationToken);

            if (aircraft == null)
                return NotFound();

            return View(aircraft);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            AircraftUpdateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var updated = await _aircraftService.UpdateAsync(
                vm,
                cancellationToken);

            if (!updated)
                return NotFound();

            TempData["Success"] = "Aircraft updated successfully.";

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
            if (id <= 0)
                return BadRequest();

            var deleted = await _aircraftService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
            {
                TempData["Error"] = "Aircraft could not be deleted.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Aircraft deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}