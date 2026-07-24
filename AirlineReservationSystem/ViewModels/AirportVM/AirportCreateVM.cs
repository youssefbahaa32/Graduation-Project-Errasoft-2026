
namespace AirlineReservationSystem.ViewModels.Airport
{
    public class AirportCreateVM
    {
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
        public List<IFormFile>? Images { get; set; } = [];

    }
}