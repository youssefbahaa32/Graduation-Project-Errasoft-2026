
namespace AirlineReservationSystem.Queries
{
    public class BaseQuery<T>//تخزين بيانات الاستعلام الاساسية مثل الفلاتر، الترتيب، التضمين، والتقسيم للصفحات.
    {
        public List<Expression<Func<T, bool>>> Filter { get; } = [];

        public Func<IQueryable<T>, IQueryable<T>>? Include { get; set; }

        public Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy { get; set; }

        public bool AsNoTracking { get; set; } = true;

        public bool IgnoreQueryFilters { get; set; } = false;

        public int? PageNumber { get; set; }

        public int? PageSize { get; set; }

        public bool IsPagingEnabled => PageNumber.HasValue && PageSize.HasValue;
    }
}