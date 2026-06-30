

namespace AirlineReservationSystem.Data.Configurations;

public class BookingPassengerConfiguration
    : IEntityTypeConfiguration<BookingPassenger>
{
    public void Configure(EntityTypeBuilder<BookingPassenger> builder)
    {
        builder.ToTable("BookingPassengers");

        builder.HasKey(bp => bp.Id);

        builder.HasOne(bp => bp.Booking)
               .WithMany(b => b.BookingPassengers)
               .HasForeignKey(bp => bp.BookingId);

        builder.HasOne(bp => bp.Passenger)
               .WithMany(p => p.BookingPassengers)
               .HasForeignKey(bp => bp.PassengerId);

        builder.HasOne(bp => bp.FlightSeat)
               .WithMany()
               .HasForeignKey(bp => bp.FlightSeatId);

        builder.HasIndex(bp => new
        {
            bp.BookingId,
            bp.PassengerId
        }).IsUnique();
    }
}