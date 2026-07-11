using Microsoft.EntityFrameworkCore;

namespace AirlineReservationSystem.Queries
{
    public static class QueryEvaluator // تطبيق البيانات المخزنه في BaseQuery على استعلام IQueryable
    {
        public static IQueryable<T> Build<T>(
            IQueryable<T> query,                    // الاستعلام اللي حنطبق عليه البيانات المخزنه في BaseQuery
            BaseQuery<T> baseQuery,                 // البيانات المخزنه في BaseQuery
            bool applyPaging = true)                // هل نطبق التقسيم للصفحات ام لا
            where T : class
        {
            // AsNoTracking
            if (baseQuery.AsNoTracking)
                query = query.AsNoTracking();

            // Ignore Global Query Filters
            if (baseQuery.IgnoreQueryFilters)
                query = query.IgnoreQueryFilters();

            // Filters
            foreach (var filter in baseQuery.Filter)
            {
                query = query.Where(filter);
            }

            // Includes
            if (baseQuery.Include != null)
            {
                query = baseQuery.Include(query);
            }

            // Sorting
            if (baseQuery.OrderBy != null)
            {
                query = baseQuery.OrderBy(query);
            }

            // Pagination
            if (applyPaging && baseQuery.IsPagingEnabled)
            {
                query = query
                    .Skip(( baseQuery.PageNumber!.Value - 1 ) * baseQuery.PageSize!.Value)
                    .Take(baseQuery.PageSize.Value);
            }

            return query;
        }
    }
}