namespace AirlineReservationSystem.Data.Configurations
{
    public class PassengerImageConfiguration : IEntityTypeConfiguration<PassengerImage>
    {
        public void Configure(EntityTypeBuilder<PassengerImage> builder)
        {
            builder.ConfigureImageProperties();
            builder.HasOne(x => x.Passenger)
                   .WithMany(p => p.Images)
                   .HasForeignKey(x => x.PassengerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
    
}
