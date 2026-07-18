using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.ViewModels.Seat;
using static NuGet.Packaging.PackagingConstants;

namespace AirlineReservationSystem.Queries
{
    public class SeatQuery : BaseQuery<Seat>
    {
        public SeatQuery(SeatIndexVM vm)
        {
            AsNoTracking = true;

            Include = q => q.Include(s => s.Aircraft);

            #region Search

            if (!string.IsNullOrWhiteSpace(vm.Search))
            {
                Filter.Add(s =>
                    s.SeatNumber.Contains(vm.Search) ||
                    s.Aircraft.RegistrationNumber.Contains(vm.Search) ||
                    s.Aircraft.Model.Contains(vm.Search) ||
                    s.Aircraft.Manufacturer.Contains(vm.Search));
            }

            #endregion

            #region Filters

            if (vm.AircraftId.HasValue)
                Filter.Add(s => s.AircraftId == vm.AircraftId);

            if (vm.SeatClass.HasValue)
                Filter.Add(s => s.SeatClass == vm.SeatClass);


            #endregion

            #region Sorting

            OrderBy = vm.SortBy switch
            {
                SeatSortBy.SeatNumber =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(s => s.SeatNumber)
                        : q => q.OrderByDescending(s => s.SeatNumber),

                SeatSortBy.SeatClass =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(s => s.SeatClass)
                        : q => q.OrderByDescending(s => s.SeatClass),

                SeatSortBy.Aircraft =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(s => s.Aircraft.RegistrationNumber)
                        : q => q.OrderByDescending(s => s.Aircraft.RegistrationNumber),

                _ =>
                    q => q.OrderBy(s => s.Id)
            };

            #endregion

            PageNumber = vm.Page;
            PageSize = vm.PageSize;
        }
    }
}