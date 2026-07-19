using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.ViewModels.AirportVM;
using static NuGet.Packaging.PackagingConstants;

namespace AirlineReservationSystem.Queries
{
    public class AirportQuery : BaseQuery<Airport>
    {
        public AirportQuery(AirportIndexVM vm)
        {
            AsNoTracking = true;

            Include = q => q
                .Include(a => a.DepartureFlights)
                .Include(a => a.ArrivalFlights);

            if (!string.IsNullOrWhiteSpace(vm.Search))
            {
                var search = vm.Search.Trim().ToLower();

                Filter.Add(a =>
                    a.Name.ToLower().Contains(search)
                    || a.City.ToLower().Contains(search)
                    || a.Country.ToLower().Contains(search)
                    || a.IATACode.ToLower().Contains(search));
            }

            if (vm.Status.HasValue)
                Filter.Add(a => a.Status == vm.Status);

            if (!string.IsNullOrWhiteSpace(vm.Country))
                Filter.Add(a => a.Country == vm.Country);

            OrderBy = vm.SortBy switch
            {
                AirportSortBy.Name =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.Name)
                        : q => q.OrderByDescending(a => a.Name),

                AirportSortBy.City =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.City)
                        : q => q.OrderByDescending(a => a.City),

                AirportSortBy.Country =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.Country)
                        : q => q.OrderByDescending(a => a.Country),

                _ =>
                    q => q.OrderByDescending(a => a.CreatedAt)
            };

            PageNumber = vm.Page;

            PageSize = vm.PageSize;
        }
    }
}