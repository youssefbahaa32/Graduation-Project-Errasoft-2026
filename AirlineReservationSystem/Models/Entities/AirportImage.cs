public class AirportImage : BaseImage
{
    public int AirportId { get; set; }

    public Airport Airport { get; set; } = null!;
}