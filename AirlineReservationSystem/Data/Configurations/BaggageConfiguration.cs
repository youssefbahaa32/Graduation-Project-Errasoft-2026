

namespace AirlineReservationSystem.Data.Configurations;

public class BaggageConfiguration : IEntityTypeConfiguration<Baggage>
{
    public void Configure(EntityTypeBuilder<Baggage> builder)
    {
        builder.ToTable("Baggages");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.ExtraWeightKg)
               .HasColumnType("decimal(5,2)")
               .IsRequired();

        builder.Property(b => b.Price)
               .HasColumnType("decimal(18,2)")
               .IsRequired();

        // الحقيبة تابعة لمسافر واحد (Many Baggages to One BookingPassenger)
        // إذا حُذف المسافر من الحجز، تُحذف حقائبه تلقائياً (Cascade Delete)
        builder.HasOne(b => b.BookingPassenger)
            .WithMany(bp => bp.Baggages)
            .HasForeignKey(b => b.BookingPassengerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}