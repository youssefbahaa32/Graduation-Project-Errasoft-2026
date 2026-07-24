namespace AirlineReservationSystem.Repositories.Implementations
{
    public class FlightImageRepository :GenericRepository<FlightImage>, IFlightImageRepository
    {
        public FlightImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
