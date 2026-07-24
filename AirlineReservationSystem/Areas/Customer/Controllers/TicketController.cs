using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    [Authorize]
    public class TicketController : Controller
    {

        private readonly ITicketCustomerService _ticketCustomerService;

        public TicketController(ITicketCustomerService ticketCustomerService)
        {
            _ticketCustomerService = ticketCustomerService;
        }

        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;


        [HttpGet]
        public async Task<IActionResult> MyTickets(CancellationToken cancellationToken)
        {
            var tickets = await _ticketCustomerService.GetMyTicketsAsync(UserId, cancellationToken);
            return View(tickets);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var vm = await _ticketCustomerService.GetTicketDetailsAsync(id, UserId, cancellationToken);

            if (vm is null)
            {
                TempData["error-notification"] = "Ticket not found.";
                return RedirectToAction("MyTickets");
            }

            return View(vm);
        }
    }
 }

