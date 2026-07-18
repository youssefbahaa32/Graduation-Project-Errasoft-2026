using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.ViewModels.Flight
{
    public class FlightIndexVM : BaseIndexVM
    {
        public int? AircraftId { get; set; }

        public int? DepartureAirportId { get; set; }

        public int? ArrivalAirportId { get; set; }

        public FlightStatus? Status { get; set; }

        public DateTime? DepartureFrom { get; set; }

        public DateTime? DepartureTo { get; set; }

        public FlightSortBy SortBy { get; set; }

        public List<FlightListVM> Flights { get; set; } = [];

        public IEnumerable<SelectListItem> Aircrafts { get; set; }
            = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Airports { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}