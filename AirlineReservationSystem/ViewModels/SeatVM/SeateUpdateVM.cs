
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.ViewModels.Seat
{
    public class SeatUpdateVM
    {
        public int Id { get; set; }

        [Required]
        public int AircraftId { get; set; }

        [Required]
        [StringLength(10)]
        [Display(Name = "Seat Number")]
        public string SeatNumber { get; set; } = null!;

        [Required]
        [Display(Name = "Seat Class")]
        public SeatClass SeatClass { get; set; }
        public IEnumerable<SelectListItem> Aircrafts { get; set; } = [];//عباره عن لستة من الطائرات المتاحه في النظام
    }
}