using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.BookingVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class BookingController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        [HttpGet]
        public async Task<IActionResult> Index(BookingFilterVM filter, CancellationToken cancellationToken)
        {
            var model = await _bookingService.GetPagedListAsync(filter, cancellationToken);
            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.GetDetailsAsync(id, cancellationToken);
            if (booking == null) return NotFound();

            // Mapping من Entity لـ BookingDetailsVM
            var model = new BookingDetailsVM
            {
                Id = booking.Id,
                BookingReference = booking.BookingReference,
                CustomerName = booking.User?.FullName ?? "N/A",
                CustomerEmail = booking.User?.Email ?? "N/A",
                FlightNumber = booking.Flight?.FlightNumber ?? "N/A",
                TotalAmount = booking.TotalAmount,
                Status = booking.Status,
                CreatedAt = booking.CreatedAt,
                Payment = booking.Payment == null ? null : new PaymentVM
                {
                    Amount = booking.Payment.Amount,
                    PaymentMethod = booking.Payment.PaymentMethod,
                    Status = booking.Payment.Status,
                    TransactionReference = booking.Payment.TransactionReference
                },
                Passengers = booking.BookingPassengers.Select(bp => new BookingPassengerVM
                {
                    FullName = bp.Passenger != null ? $"{bp.Passenger.FirstName} {bp.Passenger.LastName}" : "N/A",
                    PassportNumber = bp.Passenger?.PassportNumber ?? "N/A",
                    SeatNumber = bp.FlightSeat?.Seat?.SeatNumber ?? "N/A",
                    SeatClass = bp.FlightSeat?.Seat?.SeatClass.ToString() ?? "N/A",
                    TicketNumber = bp.Ticket?.TicketNumber,
                    BaggagesCount = bp.Baggages?.Count ?? 0
                })
            };

            return View(model);
        }


        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var booking = await _bookingService.GetDetailsAsync(id, cancellationToken);
            if (booking == null) return NotFound();

            var model = new EditBookingVM
            {
                Id = booking.Id,
                BookingReference = booking.BookingReference,
                CustomerName = booking.User?.FullName ?? "N/A",
                FlightNumber = booking.Flight?.FlightNumber ?? "N/A",
                Status = booking.Status
            };

            return View(model);
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, EditBookingVM model, CancellationToken cancellationToken)
        {
            if (id != model.Id) return BadRequest();

            if (!ModelState.IsValid) return View(model);

            try
            {
                await _bookingService.ChangeStatusAsync(model.Id, model.Status, cancellationToken);
                TempData["success-notification"] = "Booking status updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
        {
            try
            {
                await _bookingService.CancelAsync(id, cancellationToken);
                TempData["success-notification"] = "Booking cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["error-notification"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
