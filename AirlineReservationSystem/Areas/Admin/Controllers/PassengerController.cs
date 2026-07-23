using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.PassengerVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirlineReservationSystem.Areas.Admin.Controllers
{
    [Area(SD.ADMIN_AREA)]
    [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE} , {SD.EMPLOYEE_ROLE}")]
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

            var vm = new PagedResultVm<PassengerListVM>
            {
                Items = passengers.Select(p => new PassengerListVM
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

            var vm = new PassengerDetailsVM
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


        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public IActionResult Create()
        {
            return View(new PassengerCreateVM());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Create(PassengerCreateVM vm, CancellationToken ct)
        {
            if (!ModelState.IsValid)
                return View(vm);

            await _passengerService.AddPassengerAsync(vm, ct);

            try
            {
                await _passengerService.AddPassengerAsync(vm, ct);
                TempData["success-notification"] = "Passenger Added successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }


        [HttpGet]
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var passenger = await _passengerService.GetByIdAsync(id, ct);
            if (passenger is null)
                return NotFound();

            var vm = new PassengerUpdateVM
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
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(int id, PassengerUpdateVM vm, CancellationToken ct)
        {
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            var isUpdated = await _passengerService.UpdatePassengerAsync(vm, ct);
            try
            {
                if (!isUpdated)
                    return NotFound();

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
        [Authorize(Roles = $"{SD.SUPER_ADMIN_ROLE}, {SD.ADMIN_ROLE}")]
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
