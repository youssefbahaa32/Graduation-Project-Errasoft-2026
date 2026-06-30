

namespace AirlineReservationSystem.Data.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
               .HasColumnType("decimal(18,2)");

        builder.Property(p => p.TransactionReference)
               .HasMaxLength(100);

        builder.HasIndex(p => p.TransactionReference)
               .IsUnique();

        builder.HasOne(p => p.Booking)
               .WithMany(b => b.Payments)
               .HasForeignKey(p => p.BookingId);
    }
}