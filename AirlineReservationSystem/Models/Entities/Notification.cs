

namespace AirlineReservationSystem.Models.Entities;

public class Notification : AuditableEntity
{
    public string UserId { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Message { get; set; } = null!;

    public NotificationType Type { get; set; }

    public bool IsRead { get; set; }

    public ApplicationUser User { get; set; } = null!;
}