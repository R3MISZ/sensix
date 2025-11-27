namespace Sensix.Api.Dtos;

public class PagingQuery
{
    // page = 1, pageSize = 100 -> fist 100 values
    // page = 2, pageSize = 100 -> values from/to 101–200
    // page = 3, pageSize = 100 -> values from/to 201–300
    
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}

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