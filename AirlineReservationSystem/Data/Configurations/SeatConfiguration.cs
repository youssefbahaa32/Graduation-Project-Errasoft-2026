public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SeatNumber)
               .HasMaxLength(10)
               .IsRequired();

        builder.HasOne(s => s.Aircraft)
               .WithMany(a => a.Seats)
               .HasForeignKey(s => s.AircraftId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(s => new
        {
            s.AircraftId,
            s.SeatNumber
        }).IsUnique();
    }
}