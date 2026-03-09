namespace uts_api.Application.Common.Models;

public sealed class PagedRequest
{
    private const int MaxPageSize = 100;
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value <= 0 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value <= 0 ? 10 : Math.Min(value, MaxPageSize);
    }

    public string? Search { get; set; }
    public string? SortBy { get; set; } = "createdAtUtc";
    public string? SortDirection { get; set; } = "desc";
    public List<FilterRule> Filters { get; set; } = [];
    public string FilterLogic { get; set; } = "and";
}
