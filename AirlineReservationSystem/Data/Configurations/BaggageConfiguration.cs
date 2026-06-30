

namespace AirlineReservationSystem.Data.Configurations;

public class BaggageConfiguration : IEntityTypeConfiguration<Baggage>
{
    public void Configure(EntityTypeBuilder<Baggage> builder)
    {
        builder.ToTable("Baggages");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Weight)
               .HasColumnType("decimal(5,2)");

        builder.Property(b => b.Price)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.Booking)
               .WithMany(bk => bk.Baggages)
               .HasForeignKey(b => b.BookingId);
    }
}