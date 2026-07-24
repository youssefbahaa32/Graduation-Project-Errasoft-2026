    using AirlineReservationSystem.Models.Entities;
    using AirlineReservationSystem.ViewModels.Flight;
    using Microsoft.EntityFrameworkCore;

    namespace AirlineReservationSystem.Queries
    {
        public class FlightQuery : BaseQuery<Flight>
        {
            public FlightQuery(FlightIndexVM vm)
            {
                AsNoTracking = true;
            Include = q => q.Include(a => a.Images);
                Include = q => q
                    .Include(f => f.Aircraft)
                    .Include(f => f.DepartureAirport)
                    .Include(f => f.ArrivalAirport);

                #region Search

                if (!string.IsNullOrWhiteSpace(vm.Search))
                {
                    Filter.Add(f =>
                        f.FlightNumber.Contains(vm.Search) ||
                        f.Aircraft.RegistrationNumber.Contains(vm.Search) ||
                        f.Aircraft.Model.Contains(vm.Search) ||
                        f.DepartureAirport.Name.Contains(vm.Search) ||
                        f.ArrivalAirport.Name.Contains(vm.Search));
                }

                #endregion

                #region Filters

                if (vm.AircraftId.HasValue)
                    Filter.Add(f => f.AircraftId == vm.AircraftId);

                if (vm.DepartureAirportId.HasValue)
                    Filter.Add(f => f.DepartureAirportId == vm.DepartureAirportId);

                if (vm.ArrivalAirportId.HasValue)
                    Filter.Add(f => f.ArrivalAirportId == vm.ArrivalAirportId);

                if (vm.Status.HasValue)
                    Filter.Add(f => f.Status == vm.Status);

                if (vm.DepartureFrom.HasValue)
                    Filter.Add(f => f.DepartureTime >= vm.DepartureFrom.Value);

                if (vm.DepartureTo.HasValue)
                    Filter.Add(f => f.DepartureTime <= vm.DepartureTo.Value);

                #endregion

                #region Sorting

                OrderBy = vm.SortBy switch
                {
                    FlightSortBy.FlightNumber =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.FlightNumber)
                            : q => q.OrderByDescending(f => f.FlightNumber),

                    FlightSortBy.DepartureTime =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.DepartureTime)
                            : q => q.OrderByDescending(f => f.DepartureTime),

                    FlightSortBy.ArrivalTime =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.ArrivalTime)
                            : q => q.OrderByDescending(f => f.ArrivalTime),

                    FlightSortBy.BasePrice =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.BasePrice)
                            : q => q.OrderByDescending(f => f.BasePrice),

                    FlightSortBy.Aircraft =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.Aircraft.Model)
                            : q => q.OrderByDescending(f => f.Aircraft.Model),

                    FlightSortBy.DepartureAirport =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.DepartureAirport.Name)
                            : q => q.OrderByDescending(f => f.DepartureAirport.Name),

                    FlightSortBy.ArrivalAirport =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.ArrivalAirport.Name)
                            : q => q.OrderByDescending(f => f.ArrivalAirport.Name),

                    FlightSortBy.Status =>
                        vm.SortOrder == SortOrder.Ascending
                            ? q => q.OrderBy(f => f.Status)
                            : q => q.OrderByDescending(f => f.Status),

                    _ =>
                        q => q.OrderByDescending(f => f.DepartureTime)
                };

                #endregion

                PageNumber = vm.Page;
                PageSize = vm.PageSize;
            }
        }
    }