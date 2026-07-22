namespace AirlineReservationSystem.ViewModels.LoyaltyAccountVM
{
    public class LoyaltyCreateVM 
    {
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "Membership Number")]
        [StringLength(20)]
        public string MembershipNumber { get; set; } = null!;

        [Display(Name = "Tier")]
        public LoyaltyTier Tier { get; set; } = LoyaltyTier.Bronze;

        [Display(Name = "Total Points")]
        public int TotalPoints { get; set; } = 0;

        public IEnumerable<SelectListItem> Users { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}