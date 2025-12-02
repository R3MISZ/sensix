namespace Sensix.Api.Dtos.Common;

public class PagingQuery
{
    // page = 1, pageSize = 100 -> fist 100 values
    // page = 2, pageSize = 100 -> values from/to 101–200
    // page = 3, pageSize = 100 -> values from/to 201–300

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;
}
