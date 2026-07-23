using AirlineReservationSystem.ViewModels.PassengerCustomerVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    [Authorize]
    public class PassengerController : Controller
    {
        private readonly IBookingCustomerService _bookingService;

        public PassengerController(IBookingCustomerService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet]
        public async Task<IActionResult> PassengerInfo(int flightId, string seatIds, CancellationToken cancellationToken)
        {
            var seatIdList = ParseSeatIds(seatIds);

            if (seatIdList.Count == 0)
            {
                TempData["ErrorMessage"] = "No seats selected.";
                return RedirectToAction("Index", "Flight", new { area = "Customer" });
            }

            var vm = await _bookingService.BuildPassengerInfoFormAsync(flightId, seatIdList, cancellationToken);

            if (vm is null)
            {
                TempData["ErrorMessage"] = "One or more selected seats are no longer available. Please select your seats again.";
                return RedirectToAction("SelectSeats", "Flight", new { area = "Customer", flightId, passengers = seatIdList.Count });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PassengerInfo(PassengerInfoVM vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var result = await _bookingService.CreateBookingAsync(vm.FlightId, vm.Passengers, userId, cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage ?? "Could not complete booking.");
                return View(vm);
            }

            return RedirectToAction("SelectLuggage", "Booking", new { area = "Customer", bookingId = result.BookingId });
        }

        private static List<int> ParseSeatIds(string? seatIds)
        {
            if (string.IsNullOrWhiteSpace(seatIds)) return new List<int>();

            return seatIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => int.TryParse(s, out var id) ? id : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();
        }
    }
}
