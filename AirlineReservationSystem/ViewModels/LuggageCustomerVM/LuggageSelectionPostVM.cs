namespace AirlineReservationSystem.ViewModels.LuggageCustomerVM
{
    public class LuggageSelectionPostVM
    {
        public int BookingId { get; set; }
        public List<PassengerBaggageSelectionVM> Selections { get; set; } = new();
    }
}
