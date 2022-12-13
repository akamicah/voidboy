namespace DirectoryService.Shared;

public class PaginatedResult
{
    public int Total;
    public int TotalPages;
    public int Page;
    public int PageSize;
}

public class PaginatedResult<T> : PaginatedResult
{
    public PaginatedResult()
    {
        Total = 0;
        TotalPages = 0;
        Page = 0;
        PageSize = 0;
    }
    public PaginatedResult(PaginatedResult originalPaginatedResult)
    {
        Total = originalPaginatedResult.Total;
        TotalPages = originalPaginatedResult.TotalPages;
        Page = originalPaginatedResult.Page;
        PageSize = originalPaginatedResult.PageSize;
    }
    
    public PaginatedResult(IEnumerable<T> newData, PaginatedResult originalPaginatedResult)
    {
        Total = originalPaginatedResult.Total;
        TotalPages = originalPaginatedResult.TotalPages;
        Page = originalPaginatedResult.Page;
        PageSize = originalPaginatedResult.PageSize;
        Data = newData;
    }
    
    public IEnumerable<T>? Data;
}