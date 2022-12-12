using System.Reflection;
using DirectoryService.Api.Providers;
using DirectoryService.Core.Services;
using DirectoryService.Core.Validators;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Config;

namespace DirectoryService.Api.Helpers;

public class GetProjectAssemblies
{
    public static IEnumerable<Assembly?> GetAssemblies()
    {
        var assemblies = new List<Assembly>()
            .Append(Assembly.GetAssembly(typeof(SessionProvider)))
            .Append(Assembly.GetAssembly(typeof(UserService)))
            .Append(Assembly.GetAssembly(typeof(DatabaseMigrator)))
            .Append(Assembly.GetAssembly(typeof(RegisterUserValidator)))
            .Append(Assembly.GetAssembly(typeof(ServicesConfigContainer)));
        return assemblies;
    }
}