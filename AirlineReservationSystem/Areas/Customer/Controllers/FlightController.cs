using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Areas.Customer.Controllers
{
    [Area(SD.CUSTOMER_AREA)]
    public class FlightController : Controller
    {
        private readonly ICustomerFlightService _customerFlightService;
        private readonly IAirportRepository _airportRepository;

        public FlightController(ICustomerFlightService customerFlightService, IAirportRepository airportRepository)
        {
            _customerFlightService = customerFlightService;
            _airportRepository = airportRepository;
        }

        // صفحة البحث الرئيسية
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await PopulateAirportsViewBag();
            return View(new FlightSearchVM { DepartureDate = DateTime.Today });
        }

        [HttpGet]
        public async Task<IActionResult> Search(FlightSearchVM vm, CancellationToken cancellationToken)
        {
         
            if (!ModelState.IsValid)
            {
                await PopulateAirportsViewBag();
                return View("Index", vm);
            }

          
            if (vm.DepartureAirportId == vm.ArrivalAirportId)
            {
                ModelState.AddModelError("", "The departure airport cannot be the same as the arrival airport.");
                await PopulateAirportsViewBag();
                return View("Index", vm);
            }

         
            if (vm.DepartureDate.Date < DateTime.Today)
            {
                ModelState.AddModelError("", "Departure date cannot be in the past.");
                await PopulateAirportsViewBag();
                return View("Index", vm);
            }
     
            if (vm.PassengerCount < 1)
            {
                ModelState.AddModelError("", "Passenger count must be at least 1.");
                await PopulateAirportsViewBag();
                return View("Index", vm);
            }

            var results = await _customerFlightService.SearchAsync(vm, cancellationToken);

            ViewBag.PassengerCount = vm.PassengerCount;

            return View(results);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, int passengers, CancellationToken cancellationToken)
        {
           
            if (passengers < 1) passengers = 1;

            var flight = await _customerFlightService.GetDetailsAsync(id, passengers, cancellationToken);

            if (flight is null)
            {
                TempData["ErrorMessage"] = "This flight is not available for booking, or does not have enough seats for your passenger count.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PassengerCount = passengers;

            return View(flight);
        }

        [HttpGet]
        public async Task<IActionResult> SelectSeats(int flightId, int passengers, CancellationToken cancellationToken)
        {
            if (passengers < 1) passengers = 1;

            var flight = await _customerFlightService.GetDetailsAsync(flightId, passengers, cancellationToken);

            if (flight is null)
            {
                TempData["ErrorMessage"] = "This flight is not available for booking.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PassengerCount = passengers;
            return View(flight); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectSeats(SelectSeatsVM vm, CancellationToken cancellationToken)
        {
           
            if (vm.SelectedSeatIds is null || vm.SelectedSeatIds.Count != vm.Passengers)
            {
                ModelState.AddModelError("", $"Please select exactly {vm.Passengers} seat(s).");
                return await ReturnToSeatMapWithError(vm.FlightId, vm.Passengers, cancellationToken);
            }

           
            var validation = await _customerFlightService.ValidateSelectedSeatsAsync(
                vm.FlightId, vm.SelectedSeatIds, cancellationToken);

            if (!validation.IsValid)
            {
                ModelState.AddModelError("", validation.ErrorMessage ?? "Invalid seat selection.");
                return await ReturnToSeatMapWithError(vm.FlightId, vm.Passengers, cancellationToken);
            }

            
            var seatIdsJoined = string.Join(",", vm.SelectedSeatIds);

            return RedirectToAction("PassengerInfo", "Passenger", new
            {
                flightId = vm.FlightId,
                seatIds = seatIdsJoined
            });
        }



        // مساعد بيرجع نفس صفحة خريطة المقاعد لما يحصل خطأ في التحقق، عشان المستخدم يختار تاني
        private async Task<IActionResult> ReturnToSeatMapWithError(int flightId, int passengers, CancellationToken cancellationToken)
        {
            var flight = await _customerFlightService.GetDetailsAsync(flightId, passengers, cancellationToken);

            if (flight is null)
            {
                TempData["ErrorMessage"] = "This flight is not available for booking.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.PassengerCount = passengers;
            return View("SelectSeats", flight);
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

