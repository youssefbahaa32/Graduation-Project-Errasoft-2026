
namespace AirlineReservationSystem.ViewModels.Airport
{
    public class AirportCreateVM
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string Code { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = null!;

        public string? IATACode { get; set; }
        public string? ICAOCode { get; set; }
    }
}