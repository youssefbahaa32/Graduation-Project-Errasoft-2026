
namespace AirlineReservationSystem.ViewModels.Aircraft
{
    public class AircraftCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Model { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Manufacturer { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string RegistrationNumber { get; set; } = null!;

        [Range(1, 1000)]
        public int Capacity { get; set; }

        public AircraftStatus Status { get; set; }
        public List<IFormFile> Images { get; set; } = [];
    }
}