using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{
    public class FlightController : Controller
    {
        private readonly ICustomerFlightService _customerFlightService;
        private readonly IAirportRepository _airportRepository;

        public FlightController(ICustomerFlightService customerFlightService, IAirportRepository airportRepository)
        {
            _customerFlightService = customerFlightService;
            _airportRepository = airportRepository;
        }

       //صفحة البحث الرئيسية
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await PopulateAirportsViewBag();
            return View(new FlightSearchVM { DepartureDate = DateTime.Today });
        }

     
        [HttpGet]
        public async Task<IActionResult> Search(FlightSearchVM vm, CancellationToken cancellationToken)
        {
            if (vm.DepartureAirportId == vm.ArrivalAirportId)
            {
                ModelState.AddModelError("", "The departure airport cannot be the same as the arrival airport..");
                await PopulateAirportsViewBag();
                return View("Index", vm);
            }

            // الاحتفاظ بعدد الركاب لنقله عبر الصفحات حتى إتمام الحجز
            TempData["PassengerCount"] = vm.PassengerCount;

            var results = await _customerFlightService.SearchAsync(vm, cancellationToken);
            return View(results);
        }

       
        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var flight = await _customerFlightService.GetDetailsAsync(id, cancellationToken);
            if (flight is null) return NotFound();

            return View(flight);
        }

        // مساعد لتعبئة المطارات في قائمة البحث المنسدلة
        private async Task PopulateAirportsViewBag()
        {
            var airports = await _airportRepository.GetAllAsync(new BaseQuery<Airport>());
            ViewBag.Airports = airports.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.City} ({a.IATACode}) - {a.Name}"
            }).ToList();
        }
    }
}

