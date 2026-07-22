using System.ComponentModel.DataAnnotations;

namespace AirlineReservationSystem.ViewModels.Airport
{
    public class AirportUpdateVM
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public string Status { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string IATACode { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string ICAOCode { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = null!;
        // صور جديدة
        public List<IFormFile> Images { get; set; } = [];

        // الصور الحالية
        public List<AircraftImageVM> ExistingImages { get; set; } = [];

        // الصور التي اختار المستخدم حذفها
        public List<int> ImagesToDelete { get; set; } = [];
    }
}