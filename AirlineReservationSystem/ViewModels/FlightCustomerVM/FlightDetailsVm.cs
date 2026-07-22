namespace AirlineReservationSystem.ViewModels.FlightCustomerVM
{
    public class FlightDetailsVm  // تفاصيل الرحلة وخريطة المقاعد
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = null!;
        public string AircraftModel { get; set; } = null!;
        public List<SeatMapItemVm> Seats { get; set; } = new();
    }
}
