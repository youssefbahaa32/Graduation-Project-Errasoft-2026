
namespace AirlineReservationSystem.ViewModels.Seat
{
    public class SeatDetailsVM
    {
        public int Id { get; set; }

        public int AircraftId { get; set; }

        public string Aircraft { get; set; } = null!;

        public string SeatNumber { get; set; } = null!;

        public SeatClass SeatClass { get; set; }
    }
}