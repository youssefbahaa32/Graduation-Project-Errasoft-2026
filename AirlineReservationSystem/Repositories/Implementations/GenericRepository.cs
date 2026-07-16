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
        //Read
        public async Task<IEnumerable<T>> GetAllAsync(
            CancellationToken cancellationToken = default)
            => await _dbSet.ToListAsync(cancellationToken);

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
           => await _dbSet
            .FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id);

        public async Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(filter, cancellationToken);

        public async Task<IEnumerable<T>> GetWhereAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default)
            => await _dbSet.Where(filter).ToListAsync(cancellationToken);

        public async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(filter, cancellationToken);

        public async Task<int> CountAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default)
            => filter == null
                ? await _dbSet.CountAsync(cancellationToken)
                : await _dbSet.CountAsync(filter, cancellationToken);
        public async Task<IEnumerable<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
            => await _dbSet
                .Skip(( pageNumber - 1 ) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);


        // Create
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbSet.AddAsync(entity, cancellationToken);

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await _dbSet.AddRangeAsync(entities, cancellationToken);

        // Update
        public void Update(T entity)
            => _dbSet.Update(entity);
        public void UpdateRange(IEnumerable<T> entities)
            => _dbSet.UpdateRange(entities);

        // Delete
        public void Delete(T entity)
            => _dbSet.Remove(entity);
        public void DeleteRange(IEnumerable<T> entities)
            => _dbSet.RemoveRange(entities);

        // BuildQuery method to construct the query with optional parameters
        protected virtual IQueryable<T> BuildQuery(
            bool asNoTracking = false,
            bool ignoreQueryFilters = false,
            Func<IQueryable<T>, IQueryable<T>>? include = null,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
            )
        {
            IQueryable<T> query = _dbSet;

            if (asNoTracking)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            if (include != null)
                query = include(query);

            return query;
        }
    }
}
