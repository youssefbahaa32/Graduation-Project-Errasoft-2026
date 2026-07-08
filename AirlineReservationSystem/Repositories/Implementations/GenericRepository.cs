using AirlineReservationSystem.Data;
using AirlineReservationSystem.Repositories.Interfaces;

namespace AirlineReservationSystem.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T>
    where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }  
        

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)    
            => _dbSet.Remove(entity);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();
        

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>?[]? includes = null,
            bool tracked = true) // Get All
        {
            var entities = _dbSet.AsQueryable();

            if (expression is not null)
                entities = entities.Where(expression);

            if (includes is not null)
                foreach (var item in includes)
                    if (item is not null)
                        entities = entities.Include(item);

            if (!tracked)
                entities = entities.AsNoTracking();

            return await entities.ToListAsync();
        }

       public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>>? expression = null,
            Expression<Func<T, object>>?[]? includes = null,
            bool tracked = true)
        {
            return (await GetAllAsync(expression, includes, tracked)).FirstOrDefault();
        }
    }
}
