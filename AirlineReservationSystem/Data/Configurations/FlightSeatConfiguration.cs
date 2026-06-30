public class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
{
    public void Configure(EntityTypeBuilder<FlightSeat> builder)
    {
        builder.ToTable("FlightSeats");

        builder.HasKey(fs => fs.Id);

        builder.Property(fs => fs.Price)
               .HasColumnType("decimal(18,2)");

        builder.HasOne(fs => fs.Flight)
               .WithMany(f => f.FlightSeats)
               .HasForeignKey(fs => fs.FlightId);

        builder.HasOne(fs => fs.Seat)
               .WithMany(s => s.FlightSeats)
               .HasForeignKey(fs => fs.SeatId);

        builder.HasIndex(fs => new
        {
            fs.FlightId,
            fs.SeatId
        }).IsUnique();
    }
}