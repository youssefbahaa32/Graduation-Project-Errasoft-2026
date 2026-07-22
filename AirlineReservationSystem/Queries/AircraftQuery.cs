using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.ViewModels.Aircraft;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace AirlineReservationSystem.Queries
{
    public class AircraftQuery : BaseQuery<Aircraft>
    {
        public AircraftQuery(AircraftIndexVM vm)
        {
            AsNoTracking = true;
            Include = q => q.Include(a => a.Images);
            Include = q => q
                .Include(a => a.Seats)
                .Include(a => a.Flights);

            #region Search

            if (!string.IsNullOrWhiteSpace(vm.Search))
            {
                var search = vm.Search.Trim().ToLower();

                Filter.Add(a =>
                    a.RegistrationNumber.ToLower().Contains(search) ||
                    a.Manufacturer.ToLower().Contains(search) ||
                    a.Model.ToLower().Contains(search));
            }

            #endregion

            #region Filters

            if (!string.IsNullOrWhiteSpace(vm.RegistrationNumber))
            {
                var registration = vm.RegistrationNumber.Trim().ToLower();

                Filter.Add(a =>
                    a.RegistrationNumber.ToLower().Contains(registration));
            }

            if (!string.IsNullOrWhiteSpace(vm.Manufacturer))
            {
                var manufacturer = vm.Manufacturer.Trim().ToLower();

                Filter.Add(a =>
                    a.Manufacturer.ToLower().Contains(manufacturer));
            }

            if (!string.IsNullOrWhiteSpace(vm.Model))
            {
                var model = vm.Model.Trim().ToLower();

                Filter.Add(a =>
                    a.Model.ToLower().Contains(model));
            }

            if (vm.Status.HasValue)
            {
                Filter.Add(a => a.Status == vm.Status.Value);
            }

            if (vm.MinCapacity.HasValue)
            {
                Filter.Add(a => a.Capacity >= vm.MinCapacity.Value);
            }

            if (vm.MaxCapacity.HasValue)
            {
                Filter.Add(a => a.Capacity <= vm.MaxCapacity.Value);
            }

            #endregion

            #region Sorting

            OrderBy = vm.SortBy switch
            {
                AircraftSortBy.RegistrationNumber =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.RegistrationNumber)
                        : q => q.OrderByDescending(a => a.RegistrationNumber),

                AircraftSortBy.Manufacturer =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.Manufacturer)
                        : q => q.OrderByDescending(a => a.Manufacturer),

                AircraftSortBy.Model =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.Model)
                        : q => q.OrderByDescending(a => a.Model),

                AircraftSortBy.Capacity =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.Capacity)
                        : q => q.OrderByDescending(a => a.Capacity),

                AircraftSortBy.Status =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.Status)
                        : q => q.OrderByDescending(a => a.Status),

                _ =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(a => a.CreatedAt)
                        : q => q.OrderByDescending(a => a.CreatedAt)
            };

            #endregion

            #region Paging

            PageNumber = vm.Page;
            PageSize = vm.PageSize;

            #endregion
        }
    }
}