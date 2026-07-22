public class FlightImage : BaseImage
{
    public int FlightId { get; set; }

    public Flight Flight { get; set; } = null!;
}