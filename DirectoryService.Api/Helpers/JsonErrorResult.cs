using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Helpers;

/// <summary>
/// For API methods that deviate away from the standard responses
/// </summary>
public class JsonErrorResult : JsonResult
{
    private readonly HttpStatusCode _statusCode;

    public JsonErrorResult(object json, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(json)
    {
        _statusCode = statusCode;
    }

    public override void ExecuteResult(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)_statusCode;
        base.ExecuteResult(context);
    }

    public override Task ExecuteResultAsync(ActionContext context)
    {
        context.HttpContext.Response.StatusCode = (int)_statusCode;
        return base.ExecuteResultAsync(context);
    }
}