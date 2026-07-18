using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.AirportVM;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AirportController : Controller
    {
        private readonly IAirportService _airportService;

        public AirportController(IAirportService airportService)
        {
            _airportService = airportService;
        }

        #region Index

        [HttpGet]
        public async Task<IActionResult> Index(
            AirportIndexVM vm,
            CancellationToken cancellationToken)
        {
            var model = await _airportService.GetAllAsync(
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

            var airport = await _airportService.GetByIdAsync(
                id,
                cancellationToken);

            if (airport == null)
                return NotFound();

            return View(airport);
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
            AirportCreateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _airportService.CreateAsync(
                vm,
                cancellationToken);

            TempData["Success"] = "Airport created successfully.";

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

            var airport = await _airportService.GetForEditAsync(
                id,
                cancellationToken);

            if (airport == null)
                return NotFound();

            return View(airport);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            AirportUpdateVM vm,
            CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var updated = await _airportService.UpdateAsync(
                vm,
                cancellationToken);

            if (!updated)
                return NotFound();

            TempData["Success"] = "Airport updated successfully.";

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

            var deleted = await _airportService.DeleteAsync(
                id,
                cancellationToken);

            if (!deleted)
            {
                TempData["Error"] = "Airport could not be deleted.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] = "Airport deleted successfully.";

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}