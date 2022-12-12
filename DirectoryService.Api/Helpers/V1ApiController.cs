using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
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

    public class Response<T>
    {
        public string? Status { get; set; }
        public T? Data { get; set; }
    }

    /// <summary>
    /// Restrict the action to be performed only to self, unless asAdmin is defined and caller is an admin
    /// </summary>
    protected void RestrictToSelfOrAdmin(Guid userId)
    {
        var queryParams =
            Request.Query.Keys.ToDictionary<string?, string, string>(key => key.ToLower(), key => Request.Query[key]!);

        var callAsAdmin = false;
        if (queryParams.ContainsKey("asadmin"))
        {
            if (bool.TryParse(queryParams["asadmin"], out var asAdmin))
                callAsAdmin = asAdmin;
        }
        
        var session = (Session?)Request.HttpContext.Items["Session"];
        if (session == null)
            throw new UnauthorisedApiException();
        
        if(callAsAdmin && session.Role != UserRole.Admin)
            throw new UnauthorisedApiException();
        
        if(session.AccountId != userId && !callAsAdmin)
            throw new UnauthorisedApiException();
    }

    protected PaginatedRequest PaginatedRequest(string? orderBy = null, bool orderAscending = true, string? searchOn = null)
    {
        var paginationFilter = new PaginatedRequest()
        {
            OrderBy = orderBy,
            OrderAscending = orderAscending,
            SearchOn = searchOn
        };
        
        var queryParams =
            Request.Query.Keys.ToDictionary<string?, string, string>(key => key.ToLower(), key => Request.Query[key]!);

        if (queryParams.ContainsKey("filter"))
        {
            foreach (var filter in queryParams["filter"].Split(","))
            {
                paginationFilter.Filter!.Add(filter);
            }
        }

        if (queryParams.ContainsKey("status"))
        {
            foreach (var status in queryParams["status"].Split(",").ToList())
            {
                paginationFilter.Status!.Add(status);
            }
        }

        if (queryParams.ContainsKey("search"))
            paginationFilter.Search = queryParams["search"]!;

        if (queryParams.ContainsKey("per_page"))
        {
            if (int.TryParse(queryParams["per_page"], out var perPage))
                paginationFilter.PageSize = perPage;
        }
        
        if (queryParams.ContainsKey("page"))
        {
            if (int.TryParse(queryParams["page"], out var page))
                paginationFilter.Page = page;
        }
        
        if (!queryParams.ContainsKey("asadmin")) 
            return paginationFilter;

        if (bool.TryParse(queryParams["asadmin"], out var asAdmin))
            paginationFilter.AsAdmin = asAdmin;
        
        return paginationFilter;
    }
}