using AirlineReservationSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AirlineReservationSystem.Data.Configurations;

public class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
{
    public void Configure(EntityTypeBuilder<Aircraft> builder)
    {
        builder.HasKey(a => a.Id);

        // يجب أن يكون فريداً (Unique Index) لأنه مثل لوحة السيارة لا تتكرر لطائرتين
        builder.HasIndex(a => a.RegistrationNumber)
            .IsUnique();

        builder.Property(a => a.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20); 

        //  الشركة المصنعة والطراز
        builder.Property(a => a.Manufacturer)
            .IsRequired()
            .HasMaxLength(50); 

        builder.Property(a => a.Model)
            .IsRequired()
            .HasMaxLength(50); // مثل 737-800 أو A320

        //  السعة وحالة الطائرة
        builder.Property(a => a.Capacity)
            .IsRequired();

        // تحويل الـ Enum إلى String في قاعدة البيانات لجعلها مقروءة (اختياري، أو اتركها تخرن كـ int)
        builder.Property(a => a.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        //  (Relationships)

        // طائرة واحدة لها العديد من المقاعد (One-to-Many)
        // عند حذف الطائرة يتم حذف مقاعدها تلقائياً (Cascade Delete)
        builder.HasMany(a => a.Seats)
            .WithOne(s => s.Aircraft)
            .HasForeignKey(s => s.AircraftId)
            .OnDelete(DeleteBehavior.Cascade);

        // طائرة واحدة يمكن أن تعمل في العديد من الرحلات (One-to-Many)
        // نستخدم Restrict لمنع حذف الطائرة إذا كانت مرتبطة برحلات مجدولة في النظام
        builder.HasMany(a => a.Flights)
            .WithOne(f => f.Aircraft)
            .HasForeignKey(f => f.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}