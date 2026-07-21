public class PassengerImage : BaseImage
{
    public int PassengerId { get; set; }

    public Passenger Passenger { get; set; } = null!;
}