using AirlineReservationSystem.ViewModels.PaymentCustomerVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IBookingCustomerService _bookingService;

        public PaymentController(IBookingCustomerService bookingService)
        {
            _bookingService = bookingService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        [HttpGet]
        public async Task<IActionResult> Checkout(int bookingId, CancellationToken cancellationToken)
        {
            var vm = await _bookingService.BuildCheckoutFormAsync(bookingId, UserId, cancellationToken);

            if (vm is null)
            {
                TempData["error-notification"] = "This booking is no longer valid or has expired.";
                return RedirectToAction("Index", "Flight", new { area = "Customer" });
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(ProcessPaymentVM vm, CancellationToken cancellationToken)
        {
            var (resultUrl, error) = await _bookingService.ProcessPaymentAsync(vm, UserId, cancellationToken);

            if (error is not null)
            {
             
                TempData["error-notification"] = error;
                return RedirectToAction("Checkout", new { area = "Customer", bookingId = vm.BookingId });
            }

            // الدفع بالنقاط نجح فورًا
            if (resultUrl == "PointsSuccess")
            {
                return RedirectToAction("PaymentSuccess", new { area = "Customer", bookingId = vm.BookingId });
            }

            
            return Redirect(resultUrl!);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentSuccess(int bookingId, string? sessionId, CancellationToken cancellationToken)
        {
           
            if (!string.IsNullOrEmpty(sessionId))
            {
                var confirmed = await _bookingService.ConfirmStripePaymentAsync(bookingId, sessionId, cancellationToken);

                if (!confirmed)
                {
                    TempData["error-notification"] = "We couldn't confirm your payment. Please contact support if the amount was deducted.";
                    return RedirectToAction("Index", "Flight", new { area = "Customer" });
                }
            }

            var vm = await _bookingService.GetPaymentSuccessDetailsAsync(bookingId, UserId, cancellationToken);

            if (vm is null)
            {
                TempData["error-notification"] = "Booking not found.";
                return RedirectToAction("Index", "Flight", new { area = "Customer" });
            }

            return View(vm);
        }
    }
}
