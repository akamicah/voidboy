// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace DirectoryService.Shared;

public class PaginatedRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<string>? Filter { get; set; }
    public List<string>? Status { get; set; }
    public string? Search { get; set; }
    public bool AsAdmin { get; set; }
    public string? SearchOn { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderAscending { get; set; }
    public WhereCollection Where { get; } = new();

    public PaginatedRequest()
    {
        Filter = new List<string>();
        Status = new List<string>();
        AsAdmin = false;
        Page = 1;
        PageSize = 10;
        OrderBy = "createdAt";
        OrderAscending = true;
    }
}