

namespace AirlineReservationSystem.Data.Configurations;

public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.ToTable("Passengers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.FirstName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.LastName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Nationality)
               .HasMaxLength(100);

        builder.Property(p => p.PassportNumber)
               .IsRequired()
               .HasMaxLength(30);

        builder.HasIndex(p => p.PassportNumber)
               .IsUnique();
        builder.Property(p => p.Nationality)
              .HasMaxLength(100)
              .IsRequired();

        builder.Property(p => p.Gender)
               .IsRequired();

        builder.Property(p => p.DateOfBirth)
               .IsRequired();
        builder.HasMany(p => p.BookingPassengers)
               .WithOne(bp => bp.Passenger)
               .HasForeignKey(bp => bp.PassengerId)
               .OnDelete(DeleteBehavior.Cascade);
        
    }
}