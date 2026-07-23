using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.PaymentVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Index(
            string? searchTransactionRef,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            var (payments, totalCount) = await _paymentService.GetAllPaymentsAsync(
                searchTransactionRef, pageNumber, pageSize, ct);

            var vm = new PagedResultVm<PaymentListItemVm>
            {
                Items = payments.Select(p => new PaymentListItemVm
                {
                    Id = p.Id,
                    BookingId = p.BookingId,
                    BookingReference = p.Booking?.BookingReference ?? "N/A",
                    Amount = p.Amount,
                    PaymentMethod = p.PaymentMethod,
                    Status = p.Status,
                    TransactionReference = p.TransactionReference,
                    CreatedAt = p.CreatedAt
                }).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                CurrentSearch = searchTransactionRef
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var payment = await _paymentService.GetDetailsAsync(id, ct);
            if (payment is null)
                return NotFound();

            var vm = new PaymentDetailsVm
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                BookingReference = payment.Booking?.BookingReference ?? "N/A",
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                TransactionReference = payment.TransactionReference,
                CreatedAt = payment.CreatedAt
            };

            return View(vm);
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var vm = new CreatePaymentVm();
            await PopulateBookingsAsync(vm, ct);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(CreatePaymentVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateBookingsAsync(vm, ct);
                return View(vm);
            }

            var entity = new Payment
            {
                BookingId = vm.BookingId,
                Amount = vm.Amount,
                PaymentMethod = vm.PaymentMethod,
                TransactionReference = vm.TransactionReference
            };

            try
            {
                await _paymentService.AddPaymentAsync(entity, ct);
                TempData["success-notification"] = "Payment added successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateBookingsAsync(vm, ct);
                return View(vm);
            }
        }

        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var payment = await _paymentService.GetByIdAsync(id, ct);
            if (payment is null)
                return NotFound();

            var vm = new EditPaymentVm
            {
                Id = payment.Id,
                BookingId = payment.BookingId,
                Amount = payment.Amount,
                PaymentMethod = payment.PaymentMethod,
                Status = payment.Status,
                TransactionReference = payment.TransactionReference
            };

            await PopulateBookingsAsync(vm, ct, vm.BookingId);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, EditPaymentVm vm, CancellationToken ct)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateBookingsAsync(vm, ct, vm.BookingId);
                return View(vm);
            }

            var entity = new Payment
            {
                Id = vm.Id,
                BookingId = vm.BookingId,
                Amount = vm.Amount,
                PaymentMethod = vm.PaymentMethod,
                Status = vm.Status,
                TransactionReference = vm.TransactionReference
            };

            try
            {
                await _paymentService.UpdatePaymentAsync(entity, ct);
                TempData["success-notification"] = "Payment updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await PopulateBookingsAsync(vm, ct, vm.BookingId);
                return View(vm);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            try
            {
                await _paymentService.DeletePaymentAsync(id, ct);
                TempData["success-notification"] = "Payment deleted successfully";
            }
            catch (KeyNotFoundException)
            {
                TempData["error-notification"] = "Payment not found";
            }

            return RedirectToAction(nameof(Index));
        }



        // ---------- Helper ----------

        private async Task PopulateBookingsAsync(CreatePaymentVm vm, CancellationToken ct)
        {
            var bookings = await _paymentService.GetBookingsForLookupAsync(null, ct);
            vm.Bookings = bookings.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.BookingReference
            });
        }

        private async Task PopulateBookingsAsync(EditPaymentVm vm, CancellationToken ct, int? selectedId = null)
        {
            var bookings = await _paymentService.GetBookingsForLookupAsync(selectedId, ct);
            vm.Bookings = bookings.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.BookingReference,
                Selected = selectedId.HasValue && b.Id == selectedId.Value
            });
        }
    }
}
