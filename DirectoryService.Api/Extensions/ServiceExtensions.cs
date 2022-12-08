using System.Reflection;
using DirectoryService.Api.Providers;
using DirectoryService.Core.Services;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Core.Validators;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Microsoft.AspNetCore.Mvc;

namespace DirectoryService.Api.Extensions;

public static class ServiceExtensions
{
    /// <summary>
    /// Fetch a list of all assemblies referenced
    /// </summary>
    /// <returns>IEnumerable of Assembly</returns>
    private static IEnumerable<Assembly> GetListOfAssemblies()
    {
        var listOfAssemblies = new List<Assembly>();
        var mainAsm = Assembly.GetEntryAssembly();
        listOfAssemblies.Add(mainAsm!);
        listOfAssemblies.AddRange(mainAsm!.GetReferencedAssemblies().Select(Assembly.Load));
        return listOfAssemblies;
    }

    /// <summary>
    /// Register services for dependency injection
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configuration"></param>
    public static void RegisterServices(this IServiceCollection serviceCollection)
    {
        var assemblies = GetListOfAssemblies()
            .Append(Assembly.GetAssembly(typeof(SessionProvider)))
            .Append(Assembly.GetAssembly(typeof(UserService)))
            .Append(Assembly.GetAssembly(typeof(DatabaseMigrator)))
            .Append(Assembly.GetAssembly(typeof(RegisterUserValidator)))
            .Append(Assembly.GetAssembly(typeof(ServicesConfigContainer)))
            .Distinct().ToArray();

        serviceCollection.AddScoped<ISessionProvider, SessionProvider>();
        serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        
        serviceCollection.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });
        
        RegisterDiAttributedServices(assemblies!, serviceCollection);
    }
    
    /// <summary>
    /// Register all services with a DI attribute
    /// </summary>
    /// <param name="assemblies"></param>
    /// <param name="services"></param>
    private static void RegisterDiAttributedServices(IEnumerable<Assembly> assemblies, IServiceCollection services)
    {
        var scopedRegistration = typeof(ScopedRegistrationAttribute);
        var singletonRegistration = typeof(SingletonRegistrationAttribute);
        var transientRegistration = typeof(TransientRegistrationAttribute);

        var foundTypes = assemblies
            .SelectMany(s => s.GetTypes())
            .Where(p => p.IsDefined(scopedRegistration, true) ||
                        p.IsDefined(transientRegistration, true) ||
                        p.IsDefined(singletonRegistration, true) &&
                        !p.IsInterface);

        foreach (var type in foundTypes)
        {
            var typeInterface = type.GetInterface("I" + type.Name);
            if (type.IsDefined(scopedRegistration, false))
            {
                if (typeInterface != null)
                {
                    services.AddScoped(typeInterface, type);
                }
                else
                {
                    services.AddScoped(type);
                }
            }
            if (type.IsDefined(transientRegistration, false))
            {
                if (typeInterface != null)
                {
                    services.AddTransient(typeInterface, type);
                }
                else
                {
                    services.AddTransient(type);
                }
            }

            if (!type.IsDefined(singletonRegistration, false)) continue;
            
            if (typeInterface != null)
            {
                services.AddSingleton(typeInterface, type);
            }
            else
            {
                services.AddSingleton(type);
            }
        }
    }
}