using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.ViewModels.PaymentVM
{
    public class EditPaymentVm
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Booking is required")]
        [Display(Name = "Booking")]
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [Display(Name = "Status")]
        public PaymentStatus Status { get; set; }

        [Required(ErrorMessage = "Transaction reference is required")]
        [StringLength(100)]
        [Display(Name = "Transaction Reference")]
        public string TransactionReference { get; set; } = null!;

        public IEnumerable<SelectListItem>? Bookings { get; set; }
    }
}
