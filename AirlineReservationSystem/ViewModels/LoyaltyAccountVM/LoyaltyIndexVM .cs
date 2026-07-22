using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.ViewModels.Loyalty
{
    public class LoyaltyIndexVM : BaseIndexVM
    {
        public string? UserId { get; set; }

        public LoyaltySortBy SortBy { get; set; }
        public int? TotalPoints { get; set; }
        public List<LoyaltyListVM> LoyaltyAccounts { get; set; } = [];

        public IEnumerable<SelectListItem> Users { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}