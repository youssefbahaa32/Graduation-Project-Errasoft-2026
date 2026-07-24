namespace AirlineReservationSystem.ViewModels.Loyalty
{
    public class LoyaltyListVM
    {
        public int Id { get; set; }

        public string MembershipNumber { get; set; } = null!;

        public string User { get; set; } = null!;

        public int TotalPoints { get; set; }

        public LoyaltyTier Tier { get; set; }
    }
}