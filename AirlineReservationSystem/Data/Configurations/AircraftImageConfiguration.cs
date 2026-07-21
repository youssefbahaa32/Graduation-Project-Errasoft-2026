namespace AirlineReservationSystem.Data.Configurations
{
    public class AircraftImageConfiguration :IEntityTypeConfiguration<AircraftImage>
    {
        public void Configure(EntityTypeBuilder<AircraftImage> builder)
        {
            builder.ConfigureImageProperties();
            builder.HasOne(x => x.Aircraft)
                   .WithMany(a => a.Images)
                   .HasForeignKey(x => x.AircraftId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
