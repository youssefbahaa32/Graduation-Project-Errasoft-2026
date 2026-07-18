namespace AirlineReservationSystem.Models.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public String? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public String? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public String? DeletedBy { get; private set; }
    }
}
