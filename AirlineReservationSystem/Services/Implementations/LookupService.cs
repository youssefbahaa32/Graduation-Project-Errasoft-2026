using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Services.Implementations
{
    public class LookupService : ILookupService
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IAircraftRepository _aircraftRepository;

        public LookupService(
            IAirportRepository airportRepository,
            IAircraftRepository aircraftRepository)
        {
            _airportRepository = airportRepository;
            _aircraftRepository = aircraftRepository;
        }

        public async Task<IEnumerable<SelectListItem>> GetAirportsAsync(
            CancellationToken cancellationToken = default)
        {
           
            var airports = await _airportRepository.GetAllAsync(
                expression: null,
                includes: null,
                tracked: false,
                cancellationToken: cancellationToken);

           
            return airports.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.IATACode} - {a.Name}"
            }).ToList();
        }

        public async Task<IEnumerable<SelectListItem>> GetAircraftsAsync(
            CancellationToken cancellationToken = default)
        {
          
            var aircrafts = await _aircraftRepository.GetAllAsync(
                expression: null,
                includes: null,
                tracked: false,
                cancellationToken: cancellationToken);

            return aircrafts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.RegistrationNumber} - {a.Model}"
            }).ToList();
        }
    }
}