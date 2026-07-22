

namespace AirlineReservationSystem.ViewModels.Aircraft
{
    public class AircraftUpdateVM
    {
        public int Id { get; set; }

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
        // صور جديدة
        public List<IFormFile>? Images { get; set; } = [];

        // الصور الحالية
        public List<AircraftImageVM> ExistingImages { get; set; } = [];

        // الصور التي اختار المستخدم حذفها
        public List<int> ImagesToDelete { get; set; } = [];
    }
}