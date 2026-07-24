
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{

    [Area(SD.CUSTOMER_AREA)]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingCustomerService _bookingService;

        public BookingController(IBookingCustomerService bookingService)
        {
            _bookingService = bookingService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> SelectLuggage(int bookingId, CancellationToken cancellationToken)
        {
            var vm = await _bookingService.BuildSelectLuggageFormAsync(bookingId, cancellationToken);

            if (vm is null)
            {
                TempData["ErrorMessage"] = "This booking is no longer valid or has expired.";
                return RedirectToAction("Index", "Flight", new { area = "Customer" });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectLuggage(LuggageSelectionPostVM vm, CancellationToken cancellationToken)
        {
            var result = await _bookingService.AddBaggageAsync(vm.BookingId, vm.Selections, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
                return RedirectToAction("SelectLuggage", new { area = "Customer", bookingId = vm.BookingId });
            }

            return RedirectToAction("Checkout", "Payment", new { area = "Customer", bookingId = result.BookingId });
        }


        [HttpGet]
        public async Task<IActionResult> MyBookings(CancellationToken cancellationToken)
        {
            var bookings = await _bookingService.GetMyBookingsAsync(UserId, cancellationToken);
            return View(bookings);
        }
    }
}
