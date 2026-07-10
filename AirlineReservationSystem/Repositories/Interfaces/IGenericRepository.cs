
namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {

        // Read
        Task<IEnumerable<T>> GetAllAsync(
           CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(int id
            , CancellationToken cancellationToken = default);

        Task<T?> GetOneAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetWhereAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default);

       
        Task<bool> ExistsAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<T, bool>>? filter = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<T>> GetPagedAsync(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        // Create
        Task AddAsync(T entity, CancellationToken cancellationToken = default);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);

        // Update
        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        // Delete
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}