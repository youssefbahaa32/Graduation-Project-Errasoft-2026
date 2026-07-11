using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Seat;
using AutoMapper;

namespace AirlineReservationSystem.Services.Implementations
{
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeatService(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SeatIndexVM>> GetAllAsync(
        CancellationToken cancellationToken = default)
        {
            var seats = await _unitOfWork.Seats.GetAllAsync(
                include: q => q.Include(s => s.Aircraft),
                asNoTracking: true,
                cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<SeatIndexVM>>(seats);
        }

        public async Task<SeatDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var seat = await _unitOfWork.Seats
                .GetByIdAsync(id,include: q => q.Include(s => s.Aircraft),asNoTracking: true,cancellationToken: cancellationToken);

            if (seat == null)
                return null;

            return _mapper.Map<SeatDetailsVM>(seat);
        }

    public async Task<SeatUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var seat = await _unitOfWork.Seats
                .GetByIdAsync(id, include: q => q.Include(s => s.Aircraft), asNoTracking: true, cancellationToken:   cancellationToken);

            if (seat == null)
                return null;

            return _mapper.Map<SeatUpdateVM>(seat);
        }

        public async Task CreateAsync(
            SeatCreateVM vm,
            CancellationToken cancellationToken = default)
        {
            var seat = _mapper.Map<Seat>(vm);

            await _unitOfWork.Seats.AddAsync(seat, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(
            SeatUpdateVM vm,
            CancellationToken cancellationToken = default)
        {
            var seat = await _unitOfWork.Seats
                .GetByIdAsync(vm.Id, include: q => q.Include(s => s.Aircraft), asNoTracking: true, cancellationToken: cancellationToken);

            if (seat == null)
                return false;

            _mapper.Map(vm, seat);

            _unitOfWork.Seats.Update(seat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var seat = await _unitOfWork.Seats
                .GetByIdAsync(id, include: q => q.Include(s => s.Aircraft), asNoTracking: true, cancellationToken: cancellationToken);

            if (seat == null)
                return false;

            _unitOfWork.Seats.Delete(seat);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}