

namespace AirlineReservationSystem.Models.Entities;

public class Ticket : AuditableEntity
{
	public DateTime IssuedAt { get; set; }// تاريخ إصدار التذكرة

	public decimal Fare { get; set; } // السعر الذي دفعه المسافر مقابل التذكرة

	public string? Barcode { get; set; }    // رمز شريطي فريد للتذكرة، يمكن استخدامه للتحقق من صحة التذكرة عند الصعود إلى الطائرة
	public string TicketNumber { get; set; } = null!;
    public TicketStatus Status { get; set; }

    public int BookingPassengerId { get; set; }

    public BookingPassenger BookingPassenger { get; set; } = null!;

}