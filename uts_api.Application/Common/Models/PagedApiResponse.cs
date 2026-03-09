namespace uts_api.Application.Common.Models;

public sealed class PagedApiResponse<T> : ApiResponse<IReadOnlyCollection<T>>
{
    public PaginationMeta Pagination { get; init; } = new();

    public static PagedApiResponse<T> Ok(PagedResult<T> result, string message) => new()
    {
        Success = true,
        Message = message,
        Data = result.Items,
        Pagination = new PaginationMeta
        {
            PageNumber = result.PageNumber,
            PageSize = result.PageSize,
            TotalCount = result.TotalCount,
            TotalPages = result.TotalPages,
            HasPreviousPage = result.PageNumber > 1,
            HasNextPage = result.PageNumber < result.TotalPages
        }
    };
}
