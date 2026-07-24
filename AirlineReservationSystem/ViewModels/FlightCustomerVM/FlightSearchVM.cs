namespace AirlineReservationSystem.ViewModels.FlightCustomerVM
{
    public class FlightSearchVM// البيانات اللى جايه من فورمة البحث
    {
        [Required(ErrorMessage = "Please select a departure airport.")]
        public int DepartureAirportId { get; set; }

        [Required(ErrorMessage = "Please select an arrival airport.")]
        public int ArrivalAirportId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DepartureDate { get; set; } = DateTime.Today;

        [Range(1, 9, ErrorMessage = "Passenger count must be between 1 and 9.")]
        public int PassengerCount { get; set; } = 1;

    }
}
