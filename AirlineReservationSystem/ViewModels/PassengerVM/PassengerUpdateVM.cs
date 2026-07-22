namespace AirlineReservationSystem.ViewModels.PassengerVM
{
    public class PassengerUpdateVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Date of birth is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!;

        [Required(ErrorMessage = "Nationality is required")]
        [StringLength(50)]
        [Display(Name = "Nationality")]
        public string Nationality { get; set; } = null!;

        [Required(ErrorMessage = "Passport number is required")]
        [StringLength(20)]
        [Display(Name = "Passport Number")]
        public string PassportNumber { get; set; } = null!;

        [Required(ErrorMessage = "Passport expiry date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Passport Expiry Date")]
        public DateTime PassportExpiryDate { get; set; }
        // صور جديدة
        public List<IFormFile> Images { get; set; } = [];

        // الصور الحالية
        public List<AircraftImageVM> ExistingImages { get; set; } = [];

        // الصور التي اختار المستخدم حذفها
        public List<int> ImagesToDelete { get; set; } = [];
    }
}

