using AirlineReservationSystem.Data;
using AirlineReservationSystem.Models.Entities;
using AirlineReservationSystem.Repositories.Interfaces;

namespace AirlineReservationSystem.Repositories.Implementations
{
    public class LoyaltyRepository
        : GenericRepository<LoyaltyAccount>, ILoyaltyRepository
    {
        public LoyaltyRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}