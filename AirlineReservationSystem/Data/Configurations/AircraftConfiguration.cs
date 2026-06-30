public class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
{
    public void Configure(EntityTypeBuilder<Aircraft> builder)
    {
        builder.ToTable("Aircrafts");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.RegistrationNumber)
               .IsRequired()
               .HasMaxLength(30);

        builder.HasIndex(a => a.RegistrationNumber)
               .IsUnique();

        builder.Property(a => a.Manufacturer)
               .HasMaxLength(100);

        builder.Property(a => a.Model)
               .HasMaxLength(100);

        builder.Property(a => a.Capacity)
               .IsRequired();
    }
}