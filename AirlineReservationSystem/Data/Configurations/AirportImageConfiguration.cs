using AirlineReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirlineReservationSystem.Data.Configurations
{
    public class AirportImageConfiguration
        : IEntityTypeConfiguration<AirportImage>
    {
        public void Configure(EntityTypeBuilder<AirportImage> builder)
        {
            builder.ConfigureImageProperties();

            builder.HasOne(x => x.Airport)
                   .WithMany(a => a.Images)
                   .HasForeignKey(x => x.AirportId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}