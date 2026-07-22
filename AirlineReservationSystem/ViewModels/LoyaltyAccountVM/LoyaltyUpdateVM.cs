

namespace AirlineReservationSystem.ViewModels.Loyalty
{
    public class LoyaltyUpdateVM
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; } = null!;

        [Required]
        [Display(Name = "Membership Number")]
        [StringLength(20)]
        public string MembershipNumber { get; set; } = null!;

        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "Total Points")]
        public int TotalPoints { get; set; }

        [Required]
        [Display(Name = "Tier")]
        public LoyaltyTier Tier { get; set; }

        public IEnumerable<SelectListItem> Users { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}