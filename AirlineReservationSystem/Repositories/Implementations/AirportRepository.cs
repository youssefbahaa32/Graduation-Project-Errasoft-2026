public class AirportRepository : GenericRepository<Airport>, IAirportRepository
{
    public AirportRepository(ApplicationDbContext context)
        : base(context)
    {
    }
}