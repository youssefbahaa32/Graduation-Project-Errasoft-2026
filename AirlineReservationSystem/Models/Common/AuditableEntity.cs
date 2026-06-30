namespace AirlineReservationSystem.Models.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public DateTime? UpdatedBy { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? DeletedBy { get; private set; }
    }
}
