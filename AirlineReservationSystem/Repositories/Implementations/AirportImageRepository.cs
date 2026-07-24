namespace AirlineReservationSystem.Repositories.Implementations
{
    public class AirportImageRepository : GenericRepository<AirportImage>, IAirportImageRepository
    {
        public AirportImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
