using Microsoft.EntityFrameworkCore.Storage;

public interface IUnitOfWork : IDisposable
{
    IAirportRepository Airports { get; }

    IAircraftRepository Aircrafts { get; }

    ISeatRepository Seats { get; }

    IFlightRepository Flights { get; }

    IBookingRepository Bookings { get; }

    IImageRepository Images { get; }

    IGenericRepository<Passenger> Passengers { get; }
    IGenericRepository<BookingPassenger> BookingPassengers { get; }
    IGenericRepository<FlightSeat> FlightSeats { get; }
    IGenericRepository<Baggage> Baggages { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}