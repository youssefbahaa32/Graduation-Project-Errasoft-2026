
namespace AirlineReservationSystem.ViewModels.Seat
{
    public class SeatIndexVM
    {
        public int Id { get; set; }

        public string SeatNumber { get; set; } = null!;

        public SeatClass SeatClass { get; set; }

        public int AircraftId { get; set; }

        public string Aircraft { get; set; } = null!;
    }
}