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
                                  f.DepartureTime >= DateTime.Now && // منع عرض رحلات معادها فات فعلاً في نفس اليوم
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

        public async Task<FlightDetailsVm?> GetDetailsAsync(int id, int passengerCount, CancellationToken cancellationToken = default)
        {
            var flight = await _flightRepository.GetOneWithIncludesAsync(
                expression: f => f.Id == id,
                include: q => q
                    .Include(f => f.Aircraft)
                    .Include(f => f.FlightSeats).ThenInclude(fs => fs.Seat),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (flight is null) return null;


            if (flight.Status != FlightStatus.Scheduled)
                return null;

            var availableSeatsCount = flight.FlightSeats.Count(fs => fs.Status == FlightSeatStatus.Available);

            // لازم المقاعد المتاحة تكفي عدد الركاب المطلوب
            if (availableSeatsCount < passengerCount)
                return null;

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

             public async Task<SeatValidationResult> ValidateSelectedSeatsAsync(
            int flightId,
            List<int> selectedSeatIds,
            CancellationToken cancellationToken = default)
        {
            if (selectedSeatIds is null || selectedSeatIds.Count == 0)
            {
                return new SeatValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "You must select at least one seat."
                };
            }

         
            if (selectedSeatIds.Distinct().Count() != selectedSeatIds.Count)
            {
                return new SeatValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Duplicate seat selection detected."
                };
            }

           
            var flight = await _flightRepository.GetOneWithIncludesAsync(
                expression: f => f.Id == flightId,
                include: q => q.Include(f => f.FlightSeats),
                tracked: false,
                cancellationToken: cancellationToken
            );

            if (flight is null || flight.Status != FlightStatus.Scheduled)
            {
                return new SeatValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "This flight is no longer available."
                };
            }

            //كلهFlightSeatوالقيمه هى الIdالمفتاح هو الDictionaryحولناها ل
            var flightSeatsDict = flight.FlightSeats.ToDictionary(fs => fs.Id);

            foreach (var seatId in selectedSeatIds)
            {
                // هتشوفه موجود فى الTryGetValue
                if (!flightSeatsDict.TryGetValue(seatId, out var flightSeat))
                {
                    return new SeatValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "One or more selected seats do not belong to this flight."
                    };
                }

                //  المقعد لسه Available فعليًا دلوقتي (Concurrency check)
                if (flightSeat.Status != FlightSeatStatus.Available)
                {
                    return new SeatValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = $"Seat has just been booked by someone else. Please choose a different seat."
                    };
                }
            }

            return new SeatValidationResult { IsValid = true };
        }
    }
    }

