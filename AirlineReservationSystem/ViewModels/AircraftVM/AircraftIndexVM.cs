
namespace AirlineReservationSystem.ViewModels.Aircraft
{
    public class AircraftIndexVM
    {
        public int Id { get; set; }

        public string Model { get; set; } = null!;

        public string Manufacturer { get; set; } = null!;

        public string RegistrationNumber { get; set; } = null!;

        public int Capacity { get; set; }

        public AircraftStatus Status { get; set; }
    }
}