using AirlineReservationSystem.Services.Interfaces;

namespace AirlineReservationSystem.Services.Implementations
{
    public class PassengerService : IPassengerService
    {
        private readonly IGenericRepository<Passenger> _passengerRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PassengerService(IGenericRepository<Passenger> passengerRepo, IUnitOfWork unitOfWork)
        {
            _passengerRepo = passengerRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<(IEnumerable<Passenger> Items, int TotalCount)> GetAllActivePassengersAsync(
            string? searchPassport = null,
            int pageNumber = 1,
            int pageSize = 10,
            CancellationToken ct = default)
        {
            Expression<Func<Passenger, bool>> filter = string.IsNullOrEmpty(searchPassport)
                ? p => !p.IsDeleted
                : p => !p.IsDeleted && p.PassportNumber.Contains(searchPassport);

            var totalCount = await _passengerRepo.CountAsync(filter, ct);

            var all = await _passengerRepo.GetAllAsync(expression: filter, tracked: false, cancellationToken: ct);

            var items = all
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return (items, totalCount);
        }

        public async Task<Passenger?> GetPassengerDetailsWithHistoryAsync(int id, CancellationToken ct = default)
        {
            return await _passengerRepo.GetOneWithIncludesAsync(
                expression: p => p.Id == id && !p.IsDeleted,
                include: q => q.Include(p => p.BookingPassengers)
                                .ThenInclude(bp => bp.Booking), 
                tracked: false,
                cancellationToken: ct
            );
        }


        public async Task<Passenger?> GetByIdAsync(int id, CancellationToken ct = default)
            => await _passengerRepo.GetOneAsync(
                expression: p => p.Id == id && !p.IsDeleted,
                tracked: false,
                cancellationToken: ct
            );

        public async Task AddPassengerAsync(Passenger passenger, CancellationToken ct = default)
        {
            if (passenger.PassportExpiryDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Passport expiry date is invalid (already expired).");

            if (passenger.DateOfBirth >= DateTime.UtcNow)
                throw new InvalidOperationException("Date of birth is not valid.");

            var isDuplicate = await _passengerRepo.ExistsAsync(
                p => p.PassportNumber == passenger.PassportNumber && !p.IsDeleted, ct);

            if (isDuplicate)
                throw new InvalidOperationException("This passport number is already registered.");

            await _passengerRepo.AddAsync(passenger, ct);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdatePassengerAsync(Passenger passenger, CancellationToken ct = default)
        {
            var passengerInDB = await _passengerRepo.GetByIdAsync(passenger.Id, ct);
            if (passengerInDB is null)
                throw new KeyNotFoundException($"Passenger with id {passenger.Id} was not found.");

            if (passenger.DateOfBirth >= DateTime.UtcNow)
                throw new InvalidOperationException("Date of birth is not valid.");

            if (passenger.PassportExpiryDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Passport expiry date is invalid (already expired).");

            if (passenger.PassportNumber != passengerInDB.PassportNumber)
            {
                var isDuplicate = await _passengerRepo.ExistsAsync(
                    p => p.PassportNumber == passenger.PassportNumber && !p.IsDeleted && p.Id != passenger.Id, ct);

                if (isDuplicate)
                    throw new InvalidOperationException("This passport number is already used by another passenger.");
            }

            passengerInDB.FirstName = passenger.FirstName;
            passengerInDB.LastName = passenger.LastName;
            passengerInDB.DateOfBirth = passenger.DateOfBirth;
            passengerInDB.Gender = passenger.Gender;
            passengerInDB.Nationality = passenger.Nationality;
            passengerInDB.PassportNumber = passenger.PassportNumber;
            passengerInDB.PassportExpiryDate = passenger.PassportExpiryDate;

            _passengerRepo.Update(passengerInDB);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SoftDeletePassengerAsync(int id, CancellationToken ct = default)
        {
            var passenger = await _passengerRepo.GetByIdAsync(id, ct);
            if (passenger is null)
                throw new KeyNotFoundException($"Passenger with id {id} was not found.");

            passenger.IsDeleted = true;
            _passengerRepo.Update(passenger);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}