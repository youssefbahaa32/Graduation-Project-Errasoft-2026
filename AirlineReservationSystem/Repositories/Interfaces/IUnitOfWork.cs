namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAirportRepository Airports { get; }

        IFlightRepository Flights { get; }

        IBookingRepository Bookings { get; }

        Task<int> SaveChangesAsync();
    }
}
