using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Flight;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace AirlineReservationSystem.Services.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IFlightImageRepository _imageRepository;
        private readonly IFileService _fileService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILookupService _lookupService;

        public FlightService(
            IFlightRepository flightRepository,
            IAirportRepository airportRepository,
            IAircraftRepository aircraftRepository,
            IFlightImageRepository imageRepository,
            IFileService fileService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILookupService lookupService)
        {
            _flightRepository = flightRepository;
            _airportRepository = airportRepository;
            _aircraftRepository = aircraftRepository;
            _imageRepository = imageRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _lookupService = lookupService;
        }

        #region Get All

        public async Task<FlightIndexVM> GetAllAsync(
            FlightIndexVM vm,
            CancellationToken cancellationToken = default)
        {
            var query = new FlightQuery(vm);

            var flights = await _flightRepository.GetAllAsync(
                query,
                cancellationToken);

            vm.TotalCount = await _flightRepository.CountAsync(
                query,
                cancellationToken);

            vm.Flights = _mapper.Map<List<FlightListVM>>(flights);

            await LoadDropDowns(vm, cancellationToken);

            return vm;
        }

        #endregion

        #region Get By Id

        public async Task<FlightDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var query = new BaseQuery<Flight>
            {
                AsNoTracking = true,

                Include = q => q
                    .Include(f => f.Aircraft)
                    .Include(f => f.DepartureAirport)
                    .Include(f => f.ArrivalAirport)
            };

            var flight = await _flightRepository.GetByIdAsync(
                id,
                query,
                cancellationToken);

            return flight == null
                ? null
                : _mapper.Map<FlightDetailsVM>(flight);
        }

        #endregion

        #region Get For Create

        public async Task<FlightCreateVM> GetForCreateAsync(
            CancellationToken cancellationToken = default)
        {
            var vm = new FlightCreateVM();

            await LoadDropDowns(vm, cancellationToken);

            return vm;
        }

        #endregion

        #region Get For Edit

        public async Task<FlightUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var flight = await _flightRepository.GetByIdAsync(
                id,
                new BaseQuery<Flight>
                {
                    AsNoTracking = true
                },
                cancellationToken);

            if (flight == null)
                return null;

            var vm = _mapper.Map<FlightUpdateVM>(flight);

            await LoadDropDowns(vm, cancellationToken);

            return vm;
        }

        #endregion
        #region Create

        public async Task CreateAsync(
            FlightCreateVM vm,
            CancellationToken cancellationToken = default)
        {
            var flight = _mapper.Map<Flight>(vm);

        
            List<FileUploadResult> uploadedImages = [];
            if (vm.Images != null && vm.Images.Any())
            {
                uploadedImages = await _fileService.UploadAsync(
                    vm.Images,
                    "Images/Flights",
                    cancellationToken);

                foreach (var image in uploadedImages)
                {
                    flight.Images!.Add(new FlightImage
                    {
                        ImageUrl = image.RelativePath,
                        FileName = image.FileName,
                        ContentType = image.ContentType
                    });
                }
            }

            try
            {
                // الكراسي الثابتة الخاصة بالطائرة المختارة للرحلة دي
                var staticSeats = await _unitOfWork.Seats.GetAllAsync(
                    expression: s => s.AircraftId == flight.AircraftId,
                    tracked: false,
                    cancellationToken: cancellationToken);

                decimal baseRate = 0.85m; // سعر الـ 1km
                decimal routeBasePrice = baseRate * (decimal)flight.DistanceKm;

                // تحويل الكراسي الثابتة وتعبئتها مباشرة داخل الرحلة
                flight.FlightSeats = staticSeats.Select(ss =>
                {
                    // تحديد الـ Multiplier بناءً على درجة الكرسي
                    decimal classMultiplier = ss.SeatClass == SeatClass.Business ? 2.2m : 1.0m;
                    decimal finalSeatPrice = routeBasePrice * classMultiplier;

                    return new FlightSeat
                    {
                        SeatId = ss.Id,
                        Price = finalSeatPrice,
                        Status = FlightSeatStatus.Available
                    };
                }).ToList();

                await _flightRepository.AddAsync(
                    flight,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception)
            {
                foreach (var file in uploadedImages)
                {
                    _fileService.Delete(file.RelativePath);
                }

                throw;
            }
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(
            FlightUpdateVM vm,
            CancellationToken cancellationToken = default)
        {
            var flight = await _flightRepository.GetByIdAsync(
                vm.Id,
                cancellationToken: cancellationToken);

            if (flight == null)
                return false;

            if (vm.ImagesToDelete != null && vm.ImagesToDelete.Any())
            {
                var imagesToDelete = flight.Images
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
                    "Images/Flights",
                    cancellationToken);

                foreach (var file in uploadedFiles)
                {
                    flight.Images.Add(new FlightImage
                    {
                        ImageUrl = file.RelativePath,
                        FileName = file.FileName,
                        ContentType = file.ContentType
                    });
                }
            }
            _mapper.Map(vm, flight);

            _flightRepository.Update(flight);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var flight = await _flightRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (flight == null)
                return false;

            _flightRepository.Delete(flight);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Helpers

        private async Task LoadDropDowns(
            FlightIndexVM vm,
            CancellationToken cancellationToken)
        {
            var airports = await _airportRepository.GetAllAsync(
                new BaseQuery<Airport>(),
                cancellationToken);

            var aircrafts = await _aircraftRepository.GetAllAsync(
                new BaseQuery<Aircraft>(),
                cancellationToken);

            vm.Airports = airports.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.IATACode} - {a.Name}"
            });

            vm.Aircrafts = aircrafts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.RegistrationNumber} - {a.Model}"
            });
        }

        private async Task LoadDropDowns(
            FlightCreateVM vm,
            CancellationToken cancellationToken)
        {
            var airports = await _airportRepository.GetAllAsync(new BaseQuery<Airport>(), cancellationToken);
            var aircrafts = await _aircraftRepository.GetAllAsync(new BaseQuery<Aircraft>(), cancellationToken);

            vm.Airports = await _lookupService.GetAirportsAsync(cancellationToken);
            vm.Aircrafts = await _lookupService.GetAircraftsAsync(cancellationToken);
        }

        private async Task LoadDropDowns(
            FlightUpdateVM vm,
            CancellationToken cancellationToken)
        {
            var airports = await _airportRepository.GetAllAsync(new BaseQuery<Airport>(), cancellationToken);
            var aircrafts = await _aircraftRepository.GetAllAsync(new BaseQuery<Aircraft>(), cancellationToken);

            vm.Airports = await _lookupService.GetAirportsAsync(cancellationToken);
            vm.Aircrafts = await _lookupService.GetAircraftsAsync(cancellationToken);
        }

        #endregion
    }
}