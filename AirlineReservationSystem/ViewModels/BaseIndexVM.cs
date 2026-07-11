public abstract class BaseIndexVM
{
    public string? Search { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public int TotalCount { get; set; }

    public SortOrder SortOrder { get; set; } = SortOrder.Descending;
}