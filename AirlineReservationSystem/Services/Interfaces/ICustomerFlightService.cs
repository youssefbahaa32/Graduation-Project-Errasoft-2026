using AirlineReservationSystem.ViewModels.FlightCustomerVM;
using FluentValidation.TestHelper;

namespace AirlineReservationSystem.Services.Interfaces
{
    public interface ICustomerFlightService
    {

        Task<List<FlightSearchResultVm>> SearchAsync(FlightSearchVM vm, CancellationToken cancellationToken = default);
        Task<FlightDetailsVm?> GetDetailsAsync(int id, int passengerCount, CancellationToken cancellationToken = default);

        // جديد: يتحقق إن المقاعد المختارة لسه Available فعليًا في اللحظة دي (مش وقت ما الصفحة اتفتحت)
        Task<SeatValidationResult> ValidateSelectedSeatsAsync(int flightId, List<int> selectedSeatIds, CancellationToken cancellationToken = default);
    }
}

