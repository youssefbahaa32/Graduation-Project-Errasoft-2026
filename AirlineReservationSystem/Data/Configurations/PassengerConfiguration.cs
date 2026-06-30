

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
    }
}