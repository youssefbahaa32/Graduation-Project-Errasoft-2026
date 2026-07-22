using AirlineReservationSystem.ViewModels.FlightCustomerVM;

namespace AirlineReservationSystem.Services.Implementations
{
    public class CustomerFlightService : ICustomerFlightService
    {
        private readonly IFlightRepository _flightRepository;

        public CustomerFlightService(IFlightRepository flightRepository)
        {
            _flightRepository = flightRepository;
        }

        public async Task<List<FlightSearchResultVm>> SearchAsync(FlightSearchVM vm, CancellationToken cancellationToken = default)
        {
         
            var flights = await _flightRepository.GetAllAsync(
                expression: f => f.DepartureAirportId == vm.DepartureAirportId &&
                                  f.ArrivalAirportId == vm.ArrivalAirportId &&
                                  f.DepartureTime.Date == vm.DepartureDate.Date &&
                                  f.Status == FlightStatus.Scheduled,
                includes: [
                    f => f.DepartureAirport,
                    f => f.ArrivalAirport,
                    f => f.FlightSeats
                ],
                tracked: false,
                cancellationToken: cancellationToken
            );

            return flights.Select(f => new FlightSearchResultVm
            {
                Id = f.Id,
                FlightNumber = f.FlightNumber,
                DepartureAirport = f.DepartureAirport.Name,
                ArrivalAirport = f.ArrivalAirport.Name,
                DepartureTime = f.DepartureTime,
                ArrivalTime = f.ArrivalTime,
                BasePrice = f.BasePrice,
                AvailableSeatsCount = f.FlightSeats.Count(fs => fs.Status == FlightSeatStatus.Available)
            })
            .Where(f => f.AvailableSeatsCount >= vm.PassengerCount)
            .ToList();
        }

        public async Task<FlightDetailsVm?> GetDetailsAsync(int id, CancellationToken cancellationToken = default)
        {
            // جلب تفاصيل الطائرة والمقاعد  المرتبطة بها
            var flight = await _flightRepository.GetOneWithIncludesAsync(
                expression: f => f.Id == id,
                include: q => q
                    .Include(f => f.Aircraft)
                    .Include(f => f.FlightSeats).ThenInclude(fs => fs.Seat),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (flight is null) return null;

            return new FlightDetailsVm
            {
                Id = flight.Id,
                FlightNumber = flight.FlightNumber,
                AircraftModel = flight.Aircraft.Model,
                Seats = flight.FlightSeats.Select(fs => new SeatMapItemVm
                {
                    FlightSeatId = fs.Id,
                    SeatNumber = fs.Seat.SeatNumber,
                    SeatClass = fs.Seat.SeatClass,
                    Price = fs.Price,
                    Status = fs.Status
                })
                .OrderBy(s => s.SeatNumber)
                .ToList()
            };
        }

    }
}
