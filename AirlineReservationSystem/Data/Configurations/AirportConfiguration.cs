

namespace AirlineReservationSystem.Data.Configurations;

public class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.ToTable("Airports");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
               .IsRequired()
               .HasMaxLength(150);

        builder.Property(a => a.City)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.Country)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(a => a.IATACode)
               .IsRequired()
               .HasMaxLength(3);

        builder.Property(a => a.ICAOCode)
               .IsRequired()
               .HasMaxLength(4);

        builder.HasIndex(a => a.IATACode).IsUnique();

        builder.HasIndex(a => a.ICAOCode).IsUnique();
    }
}