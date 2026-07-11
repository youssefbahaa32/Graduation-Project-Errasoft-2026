using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Seat;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Controllers
{
    public class SeatController : Controller
    {
        private readonly ISeatService _seatService;

        public SeatController(ISeatService seatService)
        {
            _seatService = seatService;
        }

        public async Task<IActionResult> Index()
        {
            var seats = await _seatService.GetAllAsync();
            return View(seats);
        }

        public async Task<IActionResult> Details(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);

            if (seat == null)
                return NotFound();

            return View(seat);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeatCreateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _seatService.CreateAsync(vm);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var seat = await _seatService.GetForEditAsync(id);

            if (seat == null)
                return NotFound();

            return View(seat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SeatUpdateVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var updated = await _seatService.UpdateAsync(vm);

            if (!updated)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var seat = await _seatService.GetByIdAsync(id);

            if (seat == null)
                return NotFound();

            return View(seat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _seatService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}