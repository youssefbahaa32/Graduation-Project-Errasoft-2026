namespace AirlineReservationSystem.ViewModels.PassengerCustomerVM
{
    public class PassengerInfoVM
    {
        public int FlightId { get; set; }
        public List<PassengerFormItemVM> Passengers { get; set; } = new();

    }
}
