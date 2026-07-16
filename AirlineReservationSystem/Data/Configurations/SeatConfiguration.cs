public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.SeatNumber)
               .HasMaxLength(10)
               .IsRequired();

        builder.Property(s => s.SeatClass)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        //  الفهرس المركب (Composite Index) لمنع تكرار نفس رقم المقعد داخل نفس الطائرة
        builder.HasIndex(s => new
        {
            s.AircraftId,
            s.SeatNumber
        }).IsUnique();
        //  العلاقة بين المقعد والطائرة (One-to-Many)
        builder.HasOne(s => s.Aircraft)
               .WithMany(a => a.Seats)
               .HasForeignKey(s => s.AircraftId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.FlightSeats)
               .WithOne(fs => fs.Seat)
               .HasForeignKey(fs => fs.SeatId)
               .OnDelete(DeleteBehavior.Cascade);

    }
}