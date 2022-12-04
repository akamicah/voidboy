namespace DirectoryService.Api.Attributes;

/// <summary>
/// For use when the API method does not need a logged in user
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class AllowAnonymousAttribute : Attribute
{ }