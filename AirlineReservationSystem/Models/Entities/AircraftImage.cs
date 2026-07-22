public class AircraftImage : BaseImage
{
    public int AircraftId { get; set; }

    public Aircraft Aircraft { get; set; } = null!;
}