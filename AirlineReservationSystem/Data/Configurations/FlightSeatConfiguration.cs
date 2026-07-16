public class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
{
    public void Configure(EntityTypeBuilder<FlightSeat> builder)
    {
        builder.ToTable("FlightSeats");

        builder.HasKey(fs => fs.Id);

        builder.Property(fs => fs.Price)
               .HasColumnType("decimal(18,2)");


        builder.HasIndex(fs => new
        {
            fs.FlightId,
            fs.SeatId
        }).IsUnique();
        //relationships
        builder.HasOne(fs => fs.Flight)
           .WithMany(f => f.FlightSeats)
           .HasForeignKey(fs => fs.FlightId)
           .OnDelete(DeleteBehavior.Cascade);//حذفت رحلة حذف جميع المقاعد الخاصة بهذه الرحلة أيضًا.

        builder.HasOne(fs => fs.Seat)
           .WithMany(s => s.FlightSeats)
           .HasForeignKey(fs => fs.SeatId)
           .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(fs => fs.BookingPassenger)
            .WithOne(bp => bp.FlightSeat)
            .HasForeignKey(bp => bp.FlightSeatId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}