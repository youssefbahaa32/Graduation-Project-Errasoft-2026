namespace AirlineReservationSystem.ViewModels.FlightCustomerVM
{
    public class FlightSearchResultVm  // نتيجة البحث لكل رحلة
    {
        public int Id { get; set; }
        public string FlightNumber { get; set; } = null!;
        public string DepartureAirport { get; set; } = null!;
        public string ArrivalAirport { get; set; } = null!;
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public decimal BasePrice { get; set; }
        public int AvailableSeatsCount { get; set; }
    }
}
