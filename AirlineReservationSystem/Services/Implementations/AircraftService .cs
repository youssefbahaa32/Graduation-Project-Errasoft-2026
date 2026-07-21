using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Aircraft;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace AirlineReservationSystem.Services.Implementations
{
    public class AircraftService : IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IAircraftImageRepository _imageRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AircraftService(
            IAircraftRepository aircraftRepository,
            IAircraftImageRepository imageRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _aircraftRepository = aircraftRepository;
            _imageRepository = imageRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get All

        public async Task<AircraftIndexVM> GetAllAsync(
            AircraftIndexVM vm,
            CancellationToken cancellationToken = default)
        {
            var query = new AircraftQuery(vm);
           
            var aircrafts = await _aircraftRepository.GetAllAsync(
                query,
                cancellationToken);

            vm.TotalCount = await _aircraftRepository.CountAsync(
                query,
                cancellationToken);

            vm.Aircrafts = _mapper.Map<List<AircraftListVM>>(aircrafts);

            return vm;
        }

        #endregion

        #region Get By Id

        public async Task<AircraftDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var query = new BaseQuery<Aircraft>
            {
                Include = q => q
                    .Include(a => a.Seats)
                    .Include(a => a.Flights),

                AsNoTracking = true
            };

            var aircraft = await _aircraftRepository.GetByIdAsync(
                id,
                query,
                cancellationToken);

            if (aircraft == null)
                return null;

            return _mapper.Map<AircraftDetailsVM>(aircraft);
        }

        #endregion

        #region Get For Edit

        public async Task<AircraftUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var aircraft = await _aircraftRepository.GetByIdAsync(
                id,
                new BaseQuery<Aircraft>
                {
                    AsNoTracking = true
                },
                cancellationToken);

            if (aircraft == null)
                return null;

            return _mapper.Map<AircraftUpdateVM>(aircraft);
        }

        #endregion

        #region Create

        public async Task CreateAsync(
            AircraftCreateVM vm,
            CancellationToken cancellationToken = default)
        {
            var aircraft = _mapper.Map<Aircraft>(vm);
            var uploadedFiles = await _fileService.UploadAsync(
               vm.Images,
               "Images/Aircrafts",
               cancellationToken);

            foreach (var file in uploadedFiles)
            {
                aircraft.Images.Add(new AircraftImage
                {
                    ImageUrl = file.RelativePath,
                    FileName = file.FileName,
                    ContentType = file.ContentType
                });
            }
            await _aircraftRepository.AddAsync(
                aircraft,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(
            AircraftUpdateVM vm,
            CancellationToken cancellationToken = default)
        {
            var aircraft = await _aircraftRepository.GetByIdAsync(
                vm.Id,
                cancellationToken: cancellationToken);

            if (aircraft == null)
                return false;
            if (vm.ImagesToDelete != null && vm.ImagesToDelete.Any())
            {
                var imagesToDelete = aircraft.Images
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
                    "Images/Aircrafts",
                    cancellationToken);

                foreach (var file in uploadedFiles)
                {
                    aircraft.Images.Add(new AircraftImage
                    {
                        ImageUrl = file.RelativePath,
                        FileName = file.FileName,
                        ContentType = file.ContentType
                    });
                }
            }
            _mapper.Map(vm, aircraft);

            _aircraftRepository.Update(aircraft);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var aircraft = await _aircraftRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (aircraft == null)
                return false;

            // إذا كان لديك HasFlightsAsync في AircraftRepository
            if (await _aircraftRepository.HasFlightsAsync(id, cancellationToken))
                return false;

            _aircraftRepository.Delete(aircraft);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion
    }
}