
namespace AirlineReservationSystem.ViewModels.AirportVM
{
    public class AirportIndexVM : BaseIndexVM
    {
       

        // Filters
        public AirportStatus? Status { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }
       
        // Sorting
        public AirportSortBy SortBy { get; set; } = AirportSortBy.CreatedAt;

        public int TotalPages => (int) Math.Ceiling((double) TotalCount / PageSize);

        // Data
        public List<AirportListVM> Airports { get; set; } = new();
    }
}