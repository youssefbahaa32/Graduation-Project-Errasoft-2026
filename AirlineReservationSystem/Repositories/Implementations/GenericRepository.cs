

namespace AirlineReservationSystem.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : AuditableEntity
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        #region Read

        public virtual async Task<IEnumerable<T>> GetAllAsync(
            BaseQuery<T>? query = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = _dbSet;

            if (query != null)
                result = QueryEvaluator.Build(result, query);

            return await result.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(
            int id,
            BaseQuery<T>? query = null,
            CancellationToken cancellationToken = default)
        {
            query ??= new BaseQuery<T>();

            query.Filter.Add(e => e.Id == id);

            IQueryable<T> result =
                QueryEvaluator.Build(_dbSet, query, applyPaging: false);

            return await result.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<T?> GetOneAsync(
            BaseQuery<T> query,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> result =
                QueryEvaluator.Build(_dbSet, query, applyPaging: false);

            return await result.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(
            BaseQuery<T>? query = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = _dbSet;

            if (query != null)
                result = QueryEvaluator.Build(
                    result,
                    query,
                    applyPaging: false);

            return await result.CountAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(filter, cancellationToken);
        }

        #endregion

        #region Create

        public virtual async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public virtual async Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        #endregion

        #region Update

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        #endregion

        #region Delete

        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        #endregion
    }
}