namespace AirlineReservationSystem.ViewModels.LuggageCustomerVM
{
    public class SelectLuggageVM
    {
        public int BookingId { get; set; }
        public decimal PricePerKg { get; set; } // بيتعرض في الصفحة عشان المستخدم يعرف التسعيرة
        public decimal CurrentTotal { get; set; } // سعر الرحلة + المقاعد (قبل إضافة الأمتعة)
        public List<PassengerLuggageCardVM> Passengers { get; set; } = new();
    }
}
