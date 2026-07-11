public class SeatRepository
    : GenericRepository<Seat>, ISeatRepository
{
    public SeatRepository(ApplicationDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<Seat>> GetAvailableSeatsAsync(
        int flightId,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}