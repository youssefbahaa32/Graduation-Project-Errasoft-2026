namespace AirlineReservationSystem.ViewModels.PassengerCustomerVM
{
    public class BookingCreationResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public int? BookingId { get; set; }
    }
}
