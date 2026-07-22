namespace AirlineReservationSystem.Repositories.Implementations
{
    public class AircraftImageRepository
    : GenericRepository<AircraftImage>,
      IAircraftImageRepository
    {
        public AircraftImageRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
