namespace AirlineReservationSystem.Repositories.Implementations
{
    public class PassengerImageRepository : GenericRepository<PassengerImage>, IPassengerImageRepository
    {
        public PassengerImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
