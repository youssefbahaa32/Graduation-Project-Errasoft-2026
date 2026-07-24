namespace AirlineReservationSystem.ViewModels.FlightCustomerVM
{
    public class SeatMapItemVm  // المقعد داخل خريطة المقاعد
    {
        public int FlightSeatId { get; set; }
        public string SeatNumber { get; set; } = null!;
        public SeatClass SeatClass { get; set; }
        public decimal Price { get; set; }
        public FlightSeatStatus Status { get; set; }
    }
}
