using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Queries;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Services.Interfaces;
using AirlineReservationSystem.ViewModels.Loyalty;
using AirlineReservationSystem.ViewModels.LoyaltyAccountVM;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Services.Implementations
{
    public class LoyaltyService : ILoyaltyService
    {
        private readonly ILoyaltyRepository _loyaltyRepository;
        private readonly IGenericRepository<ApplicationUser> _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoyaltyService(
            ILoyaltyRepository loyaltyRepository,
            IGenericRepository<ApplicationUser> userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _loyaltyRepository = loyaltyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Get All

        public async Task<LoyaltyIndexVM> GetAllAsync(
            LoyaltyIndexVM vm,
            CancellationToken cancellationToken = default)
        {
            var query = new LoyaltyQuery(vm);

            var accounts = await _loyaltyRepository.GetAllAsync(
                query,
                cancellationToken);

            vm.TotalCount = await _loyaltyRepository.CountAsync(
                query,
                cancellationToken);

            vm.LoyaltyAccounts =
                _mapper.Map<List<LoyaltyListVM>>(accounts);

            await LoadUsers(vm, cancellationToken);

            return vm;
        }

        #endregion

        #region Get By Id

        public async Task<LoyaltyDetailsVM?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var account = await _loyaltyRepository.GetByIdAsync(
                id,
                new BaseQuery<LoyaltyAccount>
                {
                    AsNoTracking = true,
                    Include = q => q.Include(x => x.User)
                },
                cancellationToken);

            return account == null
                ? null
                : _mapper.Map<LoyaltyDetailsVM>(account);
        }

        #endregion

        #region Get For Create

        public async Task<LoyaltyCreateVM> GetForCreateAsync(
            CancellationToken cancellationToken = default)
        {
            var vm = new LoyaltyCreateVM();

            await LoadUsers(vm, cancellationToken);

            return vm;
        }

        #endregion

        #region Get For Edit

        public async Task<LoyaltyUpdateVM?> GetForEditAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var account = await _loyaltyRepository.GetByIdAsync(
                id,
                new BaseQuery<LoyaltyAccount>
                {
                    AsNoTracking = true
                },
                cancellationToken);

            if (account == null)
                return null;

            var vm = _mapper.Map<LoyaltyUpdateVM>(account);

            await LoadUsers(vm, cancellationToken);

            return vm;
        }

        #endregion

        #region Create

        public async Task CreateAsync(
            LoyaltyCreateVM vm,
            CancellationToken cancellationToken = default)
        {
            var account = _mapper.Map<LoyaltyAccount>(vm);

            await _loyaltyRepository.AddAsync(
                account,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        #endregion

        #region Update

        public async Task<bool> UpdateAsync(
            LoyaltyUpdateVM vm,
            CancellationToken cancellationToken = default)
        {
            var account = await _loyaltyRepository.GetByIdAsync(
                vm.Id,
                cancellationToken: cancellationToken);

            if (account == null)
                return false;

            _mapper.Map(vm, account);

            _loyaltyRepository.Update(account);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Delete

        public async Task<bool> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var account = await _loyaltyRepository.GetByIdAsync(
                id,
                cancellationToken: cancellationToken);

            if (account == null)
                return false;

            _loyaltyRepository.Delete(account);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }

        #endregion

        #region Helpers

        private async Task LoadUsers(
            LoyaltyIndexVM vm,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(
                new BaseQuery<ApplicationUser>(),
                cancellationToken);

            vm.Users = users.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.UserName
            });
        }

        private async Task LoadUsers(
            LoyaltyCreateVM vm,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(
                new BaseQuery<ApplicationUser>(),
                cancellationToken);

            vm.Users = users.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.UserName
            });
        }

        private async Task LoadUsers(
            LoyaltyUpdateVM vm,
            CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(
                new BaseQuery<ApplicationUser>(),
                cancellationToken);

            vm.Users = users.Select(x => new SelectListItem
            {
                Value = x.Id,
                Text = x.UserName
            });
        }

        #endregion
    }
}