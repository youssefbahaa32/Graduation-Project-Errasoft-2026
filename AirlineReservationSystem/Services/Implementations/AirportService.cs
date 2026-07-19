using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.AirportVM;
using AutoMapper;

namespace AirlineReservationSystem.Services.Implementations
{
    public class AirportService : IAirportService
    {
        private readonly IAirportRepository _airportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AirportService(
            IAirportRepository airportRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _airportRepository = airportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get All

        public async Task<AirportIndexVM> GetAllAsync(
     AirportIndexVM vm,
     CancellationToken cancellationToken = default)
        {
            var query = new AirportQuery(vm);

            var airports = await _airportRepository.GetAllAsync(
                query,
                cancellationToken);

            vm.TotalCount = await _airportRepository.CountAsync(
                query,
                cancellationToken);

            vm.Airports =
                _mapper.Map<List<AirportListVM>>(airports);

            return vm;
        }

        #endregion

        #region Get By Id

        public async Task<AirportDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var query = new BaseQuery<Airport>
            {
                Include = q => q
                    .Include(a => a.DepartureFlights)
                    .Include(a => a.ArrivalFlights),

                AsNoTracking = true
            };

            var airport = await _airportRepository.GetByIdAsync(
                id,
                query,
                cancellationToken);

            if (airport == null)
                return null;

            return _mapper.Map<AirportDetailsVM>(airport);
        }

        #endregion

        #region Get For Edit

        public async Task<AirportUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var airport = await _airportRepository.GetByIdAsync(
                id,
                new BaseQuery<Airport>
                {
                    AsNoTracking = true
                },
                cancellationToken);

            if (airport == null)
                return null;

            return _mapper.Map<AirportUpdateVM>(airport);
        }

        #endregion

        #region Create

        public async Task CreateAsync(
            AirportCreateVM vm,
            CancellationToken cancellationToken = default)
        {
            var airport = _mapper.Map<Airport>(vm);

            await _airportRepository.AddAsync(
                airport,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(
            AirportUpdateVM vm,
            CancellationToken cancellationToken = default)
        {
            var airport = await _airportRepository.GetByIdAsync(
                vm.Id,
                cancellationToken: cancellationToken);

            if (airport == null)
                return false;

            _mapper.Map(vm, airport);

            _airportRepository.Update(airport);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var airport = await _airportRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (airport == null)
                return false;

            _airportRepository.Delete(airport);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion
    }
}