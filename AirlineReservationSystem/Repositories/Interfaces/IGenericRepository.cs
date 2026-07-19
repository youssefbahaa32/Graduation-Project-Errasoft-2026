
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using AirlineReservationSystem.Queries; // سطر مهم عشان يقرأ ملفات زميلك

namespace AirlineReservationSystem.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        // --- طرق زميلك (BaseQuery) ---
        Task<IEnumerable<T>> GetAllAsync(BaseQuery<T>? query = null, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, BaseQuery<T>? query = null, CancellationToken cancellationToken = default);
        Task<T?> GetOneAsync(BaseQuery<T> query, CancellationToken cancellationToken = default);
        Task<int> CountAsync(BaseQuery<T>? query = null, CancellationToken cancellationToken = default);

        // --- الطرق بتاعتك أنتِ (Expressions & Includes) ---
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true, CancellationToken cancellationToken = default);
        Task<T?> GetOneAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>?[]? includes = null, bool tracked = true, CancellationToken cancellationToken = default);
        Task<T?> GetOneWithIncludesAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool tracked = true, CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default);

        // --- الطرق المشتركة ---
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default);
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
    }
}