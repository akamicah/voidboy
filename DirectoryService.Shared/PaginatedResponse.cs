namespace DirectoryService.Shared;

public class PaginatedResponse<T>
{
    public IEnumerable<T>? Data;
    public int Total;
    public int Page;
    public int PageSize;
}