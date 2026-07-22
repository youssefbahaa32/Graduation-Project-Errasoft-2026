namespace AirlineReservationSystem.ViewModels.FlightCustomerVM
{
    public class FlightSearchVM
    {// البيانات اللى جايه من فورمة البحث
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public DateTime DepartureDate { get; set; } = DateTime.Today;
        public int PassengerCount { get; set; } = 1;
    }
}
