namespace AirlineReservationSystem.ViewModels.FlightCustomerVM
{
    public class SelectSeatsVM
    {
        public int FlightId { get; set; }
        public int Passengers { get; set; }
        public List<int> SelectedSeatIds { get; set; } = new();
    }
}
