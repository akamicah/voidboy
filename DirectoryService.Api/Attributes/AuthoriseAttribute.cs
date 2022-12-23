using DirectoryService.Core.Entities;
using DirectoryService.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DirectoryService.Api.Attributes;

/// <summary>
/// User needs to be logged in. Specific user role and/or token scope
/// can be defined in the attribute parameters
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthoriseAttribute : Attribute, IAuthorizationFilter
{
    private readonly List<TokenScope>? _tokenScope;
    private readonly UserRole _userRole;

    public AuthoriseAttribute()
    {
        _userRole = UserRole.User;
        _tokenScope = null;
    }

    public AuthoriseAttribute(UserRole role)
    {
        _userRole = role;
        _tokenScope = null;
    }
    
    public AuthoriseAttribute(params TokenScope[] scope)
    {
        _userRole = UserRole.User;
        _tokenScope = scope.ToList();
    }
    
    public AuthoriseAttribute(UserRole role, params TokenScope[] scope)
    {
        _userRole = role;
        _tokenScope = scope.ToList();
    }

    /// <summary>
    /// Check to see if the session constructed from the bearer token
    /// fits the authentication rules
    /// </summary>
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        var unauthorised =
            new JsonResult(new { Status = "fail", Error = "Unauthorised", Message = "You are not authorised" })
                { StatusCode = StatusCodes.Status401Unauthorized };

        var session = (Session?)context.HttpContext.Items["Session"];
        if (session == null)
        {
            context.Result = unauthorised;
        }
        else
        {
            if (_tokenScope != null &&  !_tokenScope.Contains(session.Scope))
            {
                context.Result = unauthorised;
            }
            else
            {
                switch (_userRole)
                {
                    case UserRole.User:
                        if (session.Role != UserRole.User && session.Role != UserRole.Admin)
                            context.Result = unauthorised;
                        break;
                    case UserRole.Admin:
                        if (session.Role != UserRole.Admin)
                            context.Result = unauthorised;
                        break;
                    case UserRole.Invalid:
                    default:
                        context.Result = unauthorised;
                        break;
                }
            }
        }
    }
}