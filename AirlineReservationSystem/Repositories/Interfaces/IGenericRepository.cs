

namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface IGenericRepository<T>
        where T : AuditableEntity
    {
        #region Read

        Task<IEnumerable<T>> GetAllAsync(
            BaseQuery<T>? query = null,
            CancellationToken cancellationToken = default);

        Task<T?> GetByIdAsync(
            int id,
            BaseQuery<T>? query = null,
            CancellationToken cancellationToken = default);

        Task<T?> GetOneAsync(
            BaseQuery<T> query,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            BaseQuery<T>? query = null,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Expression<Func<T, bool>> filter,
            CancellationToken cancellationToken = default);

        #endregion

        #region Create

        Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default);

        Task AddRangeAsync(
            IEnumerable<T> entities,
            CancellationToken cancellationToken = default);

        #endregion

        #region Update

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);

        #endregion

        #region Delete

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        #endregion
    }
}