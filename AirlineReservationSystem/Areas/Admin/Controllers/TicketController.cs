using AirlineReservationSystem.Common;
using AirlineReservationSystem.ViewModels.TicketVM;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{

    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

  
        [HttpGet]
        public async Task<IActionResult> Index(
        int page = 1,
        string? search = null,
        string? passengerName = null,
        TicketStatus? status = null,
        SortOrder sortOrder = SortOrder.Descending,
        CancellationToken cancellationToken = default)
        {
            var vm = await _ticketService.SearchAsync(
                search, passengerName, status, page, 10, sortOrder, cancellationToken);

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var ticket = await _ticketService.GetDetailsVmAsync(id, cancellationToken);
            if (ticket is null) return NotFound();
            return View(ticket);
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Cancel(int id, CancellationToken cancellationToken)
        {
            try
            {
                var cancelled = await _ticketService.CancelTicketAsync(id, cancellationToken);
                TempData[cancelled ? "success-notification" : "error-notification"] =
                    cancelled ? "Ticket Canceled successfully ." : "Ticket not Found.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["error-notification"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Reinstate(int id, CancellationToken cancellationToken)
        {
            try
            {
                var done = await _ticketService.ReinstateAsync(id, cancellationToken);
                TempData[done ? "success-notification" : "error-notification"] =
                    done ? "The ticket has been successfully reactivated.." : "Ticket not Found.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["error-notification"] = ex.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
