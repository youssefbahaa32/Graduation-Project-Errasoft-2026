

namespace AirlineReservationSystem.Data.Configurations;

public class LoyaltyAccountConfiguration
    : IEntityTypeConfiguration<LoyaltyAccount>
{
    public void Configure(EntityTypeBuilder<LoyaltyAccount> builder)
    {
        builder.ToTable("LoyaltyAccounts");

        builder.HasKey(l => l.Id);

        builder.HasOne(l => l.User)
               .WithMany()
               .HasForeignKey(l => l.UserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}