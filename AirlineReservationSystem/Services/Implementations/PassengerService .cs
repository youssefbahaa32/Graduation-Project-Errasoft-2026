using AirlineReservationSystem.Services.Interfaces;

namespace AirlineReservationSystem.Services.Implementations
{
    public class PassengerService : IPassengerService
    {
        private readonly IGenericRepository<Passenger> _passengerRepo;
        private readonly IPassengerImageRepository _imageRepository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public PassengerService(IGenericRepository<Passenger> passengerRepo, IPassengerImageRepository passengerImageRepo, IUnitOfWork unitOfWork, IFileService fileService, IMapper mapper)
        {
            _passengerRepo = passengerRepo;
            _imageRepository = passengerImageRepo;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _mapper = mapper;
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

        public async Task AddPassengerAsync(PassengerCreateVM vm, CancellationToken cancellationToken = default)
        {
            if (vm.PassportExpiryDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Passport expiry date is invalid (already expired).");

            if (vm.DateOfBirth >= DateTime.UtcNow)
                throw new InvalidOperationException("Date of birth is not valid.");

            var isDuplicate = await _passengerRepo.ExistsAsync(
                p => p.PassportNumber == vm.PassportNumber && !p.IsDeleted, cancellationToken);

            if (isDuplicate)
                throw new InvalidOperationException("This passport number is already registered.");
            var passenger = _mapper.Map<Passenger>(vm);
            // رفع الصور أولاً
            var uploadedImages = await _fileService.UploadAsync(
                vm.Images,
                "Images/Passengers",
                cancellationToken);

            // إنشاء PassengerImage
            foreach (var image in uploadedImages)
            {
                passenger.Images.Add(new PassengerImage
                {
                    ImageUrl = image.RelativePath,
                    FileName = image.FileName,
                    ContentType = image.ContentType
                });
            }

            await _passengerRepo.AddAsync(passenger, cancellationToken);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdatePassengerAsync(PassengerUpdateVM vm, CancellationToken cancellationToken = default)
        {
            var passengerInDB = await _passengerRepo.GetByIdAsync(vm.Id, cancellationToken);
            if (passengerInDB is null)
                throw new KeyNotFoundException($"Passenger with id {vm.Id} was not found.");

            if (vm.DateOfBirth >= DateTime.UtcNow)
                throw new InvalidOperationException("Date of birth is not valid.");

            if (vm.PassportExpiryDate <= DateTime.UtcNow)
                throw new InvalidOperationException("Passport expiry date is invalid (already expired).");

            if (vm.PassportNumber != passengerInDB.PassportNumber)
            {
                var isDuplicate = await _passengerRepo.ExistsAsync(
                    p => p.PassportNumber == vm.PassportNumber && !p.IsDeleted && p.Id != vm.Id, cancellationToken);

                if (isDuplicate)
                    throw new InvalidOperationException("This passport number is already used by another passenger.");
            }

            if (vm.ImagesToDelete != null && vm.ImagesToDelete.Any())
            {
                var imagesToDelete = passengerInDB.Images
                    .Where(i => vm.ImagesToDelete.Contains(i.Id))
                    .ToList();

                foreach (var image in imagesToDelete)
                {
                    // حذف الملف من wwwroot
                    _fileService.Delete(image.ImageUrl);

                    // حذف السجل من قاعدة البيانات
                    _imageRepository.Delete(image);
                }
            }
            // إضافة صور جديدة
            if (vm.Images != null && vm.Images.Any())
            {
                var uploadedFiles = await _fileService.UploadAsync(
                    vm.Images,
                    "Images/Passengers",
                    cancellationToken);

                foreach (var file in uploadedFiles)
                {
                    passengerInDB.Images.Add(new PassengerImage
                    {
                        ImageUrl = file.RelativePath,
                        FileName = file.FileName,
                        ContentType = file.ContentType
                    });
                }
            }
            _mapper.Map(vm, passengerInDB);

            _passengerRepo.Update(passengerInDB);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task SoftDeletePassengerAsync(int id, CancellationToken cancellationToken = default)
        {
            var passenger = await _passengerRepo.GetByIdAsync(id, cancellationToken);
            if (passenger is null)
                throw new KeyNotFoundException($"Passenger with id {id} was not found.");

            passenger.IsDeleted = true;
            _passengerRepo.Update(passenger);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}