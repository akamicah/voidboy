using DirectoryService.Shared;
using Microsoft.AspNetCore.Mvc;
// ReSharper disable StringLiteralTypo

namespace DirectoryService.Api.Helpers;

public abstract class V1ApiController : ControllerBase
{
    protected static JsonResult Success()
    {
        return new JsonResult(new
        {
            Status = "success"
        });
    }

    protected static JsonResult Success(object result)
    {
        return new JsonResult(new
        {
            Status = "success",
            Data = result
        });
    }
    
    protected static JsonResult Failure()
    {
        return new JsonResult(new
        {
            Status = "fail",
        });
    }

    protected PaginatedRequest PaginatedRequest()
    {
        var paginationFilter = new PaginatedRequest();
        var query = Request.Query;
        
        if (query.ContainsKey("filter"))
        {
            foreach (var filters in query["filter"].Select(filterParam => 
                         filterParam?.Split(",").ToList()).Where(filters => filters is not null))
            {
                if(filters is not null)
                    paginationFilter.Filter!.AddRange(filters);
            }
        }

        if (query.ContainsKey("status"))
        {
            foreach (var statuses in query["status"].Select(statusParam => 
                         statusParam?.Split(",").ToList()).Where(statuses => statuses is not null))
            {
                if(statuses is not null)
                    paginationFilter.Status!.AddRange(statuses);
            }
        }

        if (query.ContainsKey("search"))
        {
            paginationFilter.Search = query["search"]!;
        }

        if (!query.ContainsKey("isadmin")) return paginationFilter;
        
        if(bool.TryParse(query["isadmin"], out var isAdmin))
            paginationFilter.IsAdmin = isAdmin;

        return paginationFilter;
    }
}