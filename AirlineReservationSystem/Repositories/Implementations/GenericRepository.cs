using AirlineReservationSystem.Data;
using AirlineReservationSystem.Repositories.Interfaces;
using AirlineReservationSystem.Queries; // سطر مهم عشان يقرأ ملفات زميلك
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace AirlineReservationSystem.Repositories.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        #region --- طرق زميلك (BaseQuery) ---

        public virtual async Task<IEnumerable<T>> GetAllAsync(BaseQuery<T>? query = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = _dbSet;
            if (query != null)
                result = QueryEvaluator.Build(result, query);
            return await result.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetByIdAsync(int id, BaseQuery<T>? query = null, CancellationToken cancellationToken = default)
        {
            query ??= new BaseQuery<T>();
            query.Filter.Add(e => EF.Property<int>(e, "Id") == id);
            IQueryable<T> result = QueryEvaluator.Build(_dbSet, query, applyPaging: false);
            return await result.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<T?> GetOneAsync(BaseQuery<T> query, CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = QueryEvaluator.Build(_dbSet, query, applyPaging: false);
            return await result.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(BaseQuery<T>? query = null, CancellationToken cancellationToken = default)
        {
            IQueryable<T> result = _dbSet;
            if (query != null)
                result = QueryEvaluator.Build(result, query, applyPaging: false);
            return await result.CountAsync(cancellationToken);
        }

        #endregion

        #region --- الطرق بتاعتك أنتِ (Expressions & Includes) ---

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>[]? includes = null, bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (expression is not null) query = query.Where(expression);
            if (includes is not null)
            {
                foreach (var include in includes)
                    if (include is not null) query = query.Include(include);
            }
            if (!tracked) query = query.AsNoTracking();
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>>? expression = null, Expression<Func<T, object>>?[]? includes = null, bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (expression is not null) query = query.Where(expression);
            if (includes is not null)
            {
                foreach (var include in includes)
                    if (include is not null) query = query.Include(include);
            }
            if (!tracked) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T?> GetOneWithIncludesAsync(Expression<Func<T, bool>>? expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null, bool tracked = true, CancellationToken cancellationToken = default)
        {
            IQueryable<T> query = _dbSet;
            if (expression is not null) query = query.Where(expression);
            if (include is not null) query = include(query);
            if (!tracked) query = query.AsNoTracking();
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
            => await _dbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);

        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default)
            => await _dbSet.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null, CancellationToken cancellationToken = default)
            => filter == null ? await _dbSet.CountAsync(cancellationToken) : await _dbSet.CountAsync(filter, cancellationToken);

        #endregion

        #region --- الطرق المشتركة للـ CRUD ---

        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken = default)
            => await _dbSet.AnyAsync(filter, cancellationToken);

        public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
            => await _dbSet.AddAsync(entity, cancellationToken);

        public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
            => await _dbSet.AddRangeAsync(entities, cancellationToken);

        public virtual void Update(T entity) => _dbSet.Update(entity);
        public virtual void UpdateRange(IEnumerable<T> entities) => _dbSet.UpdateRange(entities);
        public virtual void Delete(T entity) => _dbSet.Remove(entity);
        public virtual void DeleteRange(IEnumerable<T> entities) => _dbSet.RemoveRange(entities);

        #endregion
    }
}