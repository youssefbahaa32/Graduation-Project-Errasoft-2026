

namespace AirlineReservationSystem.Data.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TicketNumber)
               .HasMaxLength(30)
               .IsRequired();

        builder.HasIndex(t => t.TicketNumber)
               .IsUnique();

        builder.HasOne(t => t.BookingPassenger)
               .WithOne(b => b.Ticket)
               .HasForeignKey<Ticket>(t => t.BookingPassengerId);


    }
}