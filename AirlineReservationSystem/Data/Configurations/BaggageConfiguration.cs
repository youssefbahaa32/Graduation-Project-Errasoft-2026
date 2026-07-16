

namespace AirlineReservationSystem.Data.Configurations;

public class BaggageConfiguration : IEntityTypeConfiguration<Baggage>
{
    public void Configure(EntityTypeBuilder<Baggage> builder)
    {
        builder.ToTable("Baggages");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.BagTagNumber)
               .IsRequired()
               .HasMaxLength(50);


        builder.Property(b => b.Weight)
               .HasColumnType("decimal(5,2)");

        builder.Property(b => b.Price)
               .HasColumnType("decimal(18,2)");

        builder.Property(b => b.Type)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();
        // الحقيبة تابعة لمسافر واحد (Many Baggages to One Passenger)
        // إذا حُذف المسافر، تُحذف حقائبه تلقائياً (Cascade Delete)
        builder.HasOne(b => b.BookingPassenger)
            .WithMany(p => p.Baggages)
            .HasForeignKey(b => b.BookingPassengerId)
            .OnDelete(DeleteBehavior.Cascade);

        // الحقيبة مشحونة على رحلة معينة (Many Baggages to One Flight)
        // نستخدم Restrict هنا؛ لا يمكن حذف رحلة من النظام إذا كان هناك حقائب مسجلة عليها بالفعل
        builder.HasOne(b => b.Flight)
            .WithMany() // يمكنك تركها فارغة إذا لم تكن بحاجة لـ ICollection<Baggage> داخل الـ Flight Entity
            .HasForeignKey(b => b.FlightId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}