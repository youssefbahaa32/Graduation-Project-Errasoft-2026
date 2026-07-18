using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.PassengerVM;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    public class PassengerController : Controller
    {
        private readonly IPassengerService _passengerService;

        public PassengerController(IPassengerService passengerService)
        {
            _passengerService = passengerService;
        }

        public async Task<IActionResult> Index(
    string? searchPassport,
    int pageNumber = 1,
    int pageSize = 10,
    CancellationToken cancellationToken = default)
        {
            var (passengers, totalCount) = await _passengerService.GetAllActivePassengersAsync(
                searchPassport, pageNumber, pageSize, cancellationToken);

            var vm = new PagedResultVm<PassengerListItemVm>
            {
                Items = passengers.Select(p => new PassengerListItemVm
                {
                    Id = p.Id,
                    FullName = $"{p.FirstName} {p.LastName}",
                    PassportNumber = p.PassportNumber,
                    Nationality = p.Nationality,
                    PassportExpiryDate = p.PassportExpiryDate
                }).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                CurrentSearch = searchPassport
            };

            return View(vm);
        }



        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var passenger = await _passengerService.GetPassengerDetailsWithHistoryAsync(id, ct);
            if (passenger is null)
                return NotFound();

            var vm = new PassengerDetailsVm
            {
                Id = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                DateOfBirth = passenger.DateOfBirth,
                Gender = passenger.Gender,
                Nationality = passenger.Nationality,
                PassportNumber = passenger.PassportNumber,
                PassportExpiryDate = passenger.PassportExpiryDate,
                BookingReferences = passenger.BookingPassengers
                    .Select(bp => bp.Booking?.BookingReference ?? "N/A")
                    .ToList()
            };

            return View(vm);
        }


        public IActionResult Create()
        {
            return View(new CreatePassengerVm());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePassengerVm vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var entity = new Passenger
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DateOfBirth = vm.DateOfBirth,
                Gender = vm.Gender,
                Nationality = vm.Nationality,
                PassportNumber = vm.PassportNumber,
                PassportExpiryDate = vm.PassportExpiryDate
            };

            try
            {
                await _passengerService.AddPassengerAsync(entity, ct);
                TempData["success-notification"] = "Passenger Added successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }


        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var passenger = await _passengerService.GetByIdAsync(id, ct);
            if (passenger is null)
                return NotFound();

            var vm = new EditPassengerVm
            {
                Id = passenger.Id,
                FirstName = passenger.FirstName,
                LastName = passenger.LastName,
                DateOfBirth = passenger.DateOfBirth,
                Gender = passenger.Gender,
                Nationality = passenger.Nationality,
                PassportNumber = passenger.PassportNumber,
                PassportExpiryDate = passenger.PassportExpiryDate
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditPassengerVm vm, CancellationToken ct)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            var entity = new Passenger
            {
                Id = vm.Id,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                DateOfBirth = vm.DateOfBirth,
                Gender = vm.Gender,
                Nationality = vm.Nationality,
                PassportNumber = vm.PassportNumber,
                PassportExpiryDate = vm.PassportExpiryDate
            };

            try
            {
                await _passengerService.UpdatePassengerAsync(entity, ct);
                TempData["success-notification"] = "Passenger Updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }



        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            try
            {
                await _passengerService.SoftDeletePassengerAsync(id, ct);
                TempData["success-notification"] = "Passenger deleted successfully";
            }
            catch (KeyNotFoundException)
            {
                TempData["error-notification"] = "Passenger not found";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
