
namespace AirlineReservationSystem.Models.Entities;

public class PointTransaction : AuditableEntity
{
    public int LoyaltyAccountId { get; set; }

    public int Points { get; set; }

    public string Description { get; set; } = null!;

    public LoyaltyAccount LoyaltyAccount { get; set; } = null!;
}