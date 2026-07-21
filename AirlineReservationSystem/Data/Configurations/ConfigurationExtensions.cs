using AirlineReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirlineReservationSystem.Data.Configurations
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureImageProperties<T>(
            this EntityTypeBuilder<T> builder)
            where T : BaseImage
        {
            builder.Property(x => x.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.FileName)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(x => x.ContentType)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}