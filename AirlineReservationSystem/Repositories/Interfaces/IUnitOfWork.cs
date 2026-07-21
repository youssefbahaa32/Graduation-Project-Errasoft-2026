public interface IUnitOfWork : IDisposable
{
    IAirportRepository Airports { get; }

    IAircraftRepository Aircrafts { get; }

    ISeatRepository Seats { get; }

    IFlightRepository Flights { get; }

    IBookingRepository Bookings { get; }

    IImageRepository Images { get; }
   
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}