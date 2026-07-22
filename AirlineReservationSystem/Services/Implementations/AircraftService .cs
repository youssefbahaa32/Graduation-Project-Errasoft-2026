using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Aircraft;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Services.Implementations
{
    public class AircraftService : IAircraftService
    {
        private readonly IAircraftRepository _aircraftRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AircraftService(
            IAircraftRepository aircraftRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _aircraftRepository = aircraftRepository;
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

            await _aircraftRepository.AddAsync(
                aircraft,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // توليد 60 مقعد ثابت للطائرة (15 صف، كل صف 4 مقاعد A, B, C, D)
            var seats = new List<Seat>();
            char[] seatLetters = { 'A', 'B', 'C', 'D' }; 
            int totalRows = 15; 

            for (int row = 1; row <= totalRows; row++)
            {
                foreach (var letter in seatLetters)
                {
                    seats.Add(new Seat
                    {
                        AircraftId = aircraft.Id,
                        SeatNumber = $"{row}{letter}", // هيولد: 1A, 1B, 1C, 1D
                        SeatClass = row <= 3 ? SeatClass.Business : SeatClass.Economy
                    });
                }
            }
            await _unitOfWork.Seats.AddRangeAsync(seats, cancellationToken);
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