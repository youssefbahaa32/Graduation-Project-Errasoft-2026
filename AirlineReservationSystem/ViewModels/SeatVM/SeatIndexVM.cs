using Microsoft.AspNetCore.Mvc.Rendering;

public class SeatIndexVM : BaseIndexVM
{
    public int? AircraftId { get; set; }

    public SeatClass? SeatClass { get; set; }

    public SeatSortBy SortBy { get; set; }

    public List<SeatListVM> Seats { get; set; } = [];

    public IEnumerable<SelectListItem> Aircrafts { get; set; }
        = Enumerable.Empty<SelectListItem>();
}