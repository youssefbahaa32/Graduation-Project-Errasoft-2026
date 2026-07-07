public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flights");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FlightNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(f => f.BasePrice)
               .HasColumnType("decimal(18,2)");

        builder.HasIndex(f => f.FlightNumber);

        builder.HasOne(f => f.DepartureAirport)
               .WithMany(a => a.DepartureFlights)
               .HasForeignKey(f => f.DepartureAirportId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.ArrivalAirport)
               .WithMany(a => a.ArrivalFlights)
               .HasForeignKey(f => f.ArrivalAirportId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(f => f.Aircraft)
               .WithMany(a => a.Flights)
               .HasForeignKey(f => f.AircraftId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasMany(f => f.FlightSeats)
               .WithOne(fs => fs.Flight)
               .HasForeignKey(fs => fs.FlightId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}