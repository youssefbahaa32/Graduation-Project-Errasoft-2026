using AirlineReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirlineReservationSystem.Data.Configurations;

public class PointTransactionConfiguration
    : IEntityTypeConfiguration<PointTransaction>
{
    public void Configure(EntityTypeBuilder<PointTransaction> builder)
    {
        builder.ToTable("PointTransactions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Description)
               .HasMaxLength(500);

        builder.HasOne(p => p.LoyaltyAccount)
               .WithMany(l => l.PointTransactions)
               .HasForeignKey(p => p.LoyaltyAccountId);
    }
}