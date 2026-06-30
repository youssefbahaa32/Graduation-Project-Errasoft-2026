

namespace AirlineReservationSystem.Models.Entities;

public class LoyaltyAccount : AuditableEntity
{
    public string UserId { get; set; } = null!;

    public int TotalPoints { get; set; }

    public ApplicationUser User { get; set; } = null!;

    public ICollection<PointTransaction> PointTransactions { get; set; }
        = new HashSet<PointTransaction>();
}