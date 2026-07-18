using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Aircraft;
using AirlineReservationSystem.ViewModels.Seat;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AirlineReservationSystem.Services.Implementations
{
    public class SeatService : ISeatService
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeatService(
            ISeatRepository seatRepository,
            IAircraftRepository aircraftRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _seatRepository = seatRepository;
            _aircraftRepository = aircraftRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get All

        public async Task<SeatIndexVM> GetAllAsync(
            SeatIndexVM vm,
            CancellationToken cancellationToken = default)
        {
            var query = new SeatQuery(vm);

            var seats = await _seatRepository.GetAllAsync(
                query,
                cancellationToken);

            vm.TotalCount = await _seatRepository.CountAsync(
                query,
                cancellationToken);

            vm.Seats = _mapper.Map<List<SeatListVM>>(seats);

            var aircrafts = await _aircraftRepository.GetAllAsync(
                new BaseQuery<Aircraft>(),
                cancellationToken);

            vm.Aircrafts = aircrafts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.RegistrationNumber} - {a.Model}"
            });

            return vm;
        }

        #endregion

        #region Get For Create

        public async Task<SeatCreateVM> GetForCreateAsync(
            CancellationToken cancellationToken = default)
        {
            var vm = new SeatCreateVM();

            var aircrafts = await _aircraftRepository.GetAllAsync(
                new BaseQuery<Aircraft>(),
                cancellationToken);

            vm.Aircrafts = aircrafts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.RegistrationNumber} - {a.Model}"
            });

            return vm;
        }

        #endregion

        #region Get For Edit

        public async Task<SeatUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var seat = await _seatRepository.GetByIdAsync(
                id,
                new BaseQuery<Seat>
                {
                    AsNoTracking = true
                },
                cancellationToken);

            if (seat == null)
                return null;

            var vm = _mapper.Map<SeatUpdateVM>(seat);

            var aircrafts = await _aircraftRepository.GetAllAsync(
                new BaseQuery<Aircraft>(),
                cancellationToken);

            vm.Aircrafts = aircrafts.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.RegistrationNumber} - {a.Model}"
            });

            return vm;
        }

        #endregion

        #region Create

        public async Task CreateAsync(
            SeatCreateVM vm,
            CancellationToken cancellationToken = default)
        {
            var seat = _mapper.Map<Seat>(vm);

            await _seatRepository.AddAsync(
                seat,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(
            SeatUpdateVM vm,
            CancellationToken cancellationToken = default)
        {
            var seat = await _seatRepository.GetByIdAsync(
                vm.Id,
                cancellationToken: cancellationToken);

            if (seat == null)
                return false;

            _mapper.Map(vm, seat);

            _seatRepository.Update(seat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var seat = await _seatRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (seat == null)
                return false;

            _seatRepository.Delete(seat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion
    }
}