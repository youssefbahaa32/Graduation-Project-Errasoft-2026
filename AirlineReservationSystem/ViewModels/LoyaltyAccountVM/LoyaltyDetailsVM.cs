namespace AirlineReservationSystem.ViewModels.Loyalty
{
    public class LoyaltyDetailsVM
    {
        public int Id { get; set; }

        public string MembershipNumber { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string User { get; set; } = null!;

        public int TotalPoints { get; set; }

        public LoyaltyTier Tier { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}