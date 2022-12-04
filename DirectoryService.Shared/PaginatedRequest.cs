namespace DirectoryService.Shared;

public class PaginatedRequest
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<string>? Filter { get; set; }
    public List<string>? Status { get; set; }
    public string Search { get; set; }
    public bool IsAdmin { get; set; }

    public PaginatedRequest()
    {
        Filter = new List<string>();
        Status = new List<string>();
        Search = "";
        IsAdmin = false;
        Page = 1;
        PageSize = 10;
    }
}