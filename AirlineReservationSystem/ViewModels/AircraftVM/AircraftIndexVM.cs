public class AircraftIndexVM : BaseIndexVM
{
    public string? Manufacturer { get; set; }

    public string? Model { get; set; }

    public string? RegistrationNumber { get; set; }

    public AircraftStatus? Status { get; set; }

    public int? MinCapacity { get; set; }

    public int? MaxCapacity { get; set; }

    public AircraftSortBy SortBy { get; set; }

    public List<AircraftListVM> Aircrafts { get; set; } = [];
}