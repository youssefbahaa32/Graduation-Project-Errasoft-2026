

namespace AirlineReservationSystem.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.BookingReference)
               .IsRequired()
               .HasMaxLength(20);

        builder.HasIndex(b => b.BookingReference)
               .IsUnique();

        builder.Property(b => b.TotalAmount)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(b => b.User)
               .WithMany(u => u.Bookings)
               .HasForeignKey(b => b.UserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Flight)
               .WithMany()
               .HasForeignKey(b => b.FlightId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}