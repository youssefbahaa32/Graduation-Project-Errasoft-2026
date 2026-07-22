namespace AirlineReservationSystem.Data.Configurations
{
    public class FlightImageConfiguration : IEntityTypeConfiguration<FlightImage>
    {
        public void Configure(EntityTypeBuilder<FlightImage> builder)
        {
            builder.ConfigureImageProperties();
            builder.HasOne(x => x.Flight)
                   .WithMany(f => f.Images)
                   .HasForeignKey(x => x.FlightId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
    
}
