namespace Sensix.Api.Dtos.Common;

public class PagedResult<T>
{
    public IEnumerable<T> Data { get; set; } = Enumerable.Empty<T>();

    public int Page { get; set; }
    public int PageSize { get; set; }

    public int TotalCount { get; set; }
    public int TotalPages { get; set; }

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
