using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.ViewModels.Loyalty;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Queries
{
    public class LoyaltyQuery : BaseQuery<LoyaltyAccount>
    {
        public LoyaltyQuery(LoyaltyIndexVM vm)
        {
            AsNoTracking = true;

            Include = q => q.Include(l => l.User);

            #region Search

            if (!string.IsNullOrWhiteSpace(vm.Search))
            {
                Filter.Add(l =>
                    l.User.UserName!.Contains(vm.Search) ||
                    l.User.Email!.Contains(vm.Search));
            }

            #endregion

            #region Filters

            if (!string.IsNullOrWhiteSpace(vm.UserId))
                Filter.Add(l => l.UserId == vm.UserId);

            if (vm.TotalPoints.HasValue)
                Filter.Add(l => l.TotalPoints == vm.TotalPoints);

            #endregion

            #region Sorting

            OrderBy = vm.SortBy switch
            {
                LoyaltySortBy.User =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(l => l.User.UserName)
                        : q => q.OrderByDescending(l => l.User.UserName),

                LoyaltySortBy.TotalPoints =>
                    vm.SortOrder == SortOrder.Ascending
                        ? q => q.OrderBy(l => l.TotalPoints)
                        : q => q.OrderByDescending(l => l.TotalPoints),

                _ =>
                    q => q.OrderByDescending(l => l.Id)
            };

            #endregion

            PageNumber = vm.Page;
            PageSize = vm.PageSize;
        }
    }
}