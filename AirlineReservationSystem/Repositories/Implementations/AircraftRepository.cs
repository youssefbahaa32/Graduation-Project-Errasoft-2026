public class AircraftRepository
    : GenericRepository<Aircraft>, IAircraftRepository
{
    public AircraftRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public Task<bool> HasFlightsAsync(int aircraftId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}