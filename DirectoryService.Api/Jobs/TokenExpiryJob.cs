using DirectoryService.Core.Services;
using FluentScheduler;

namespace DirectoryService.Api.Jobs;

/// <summary>
/// Will periodically call upon the TokenExpiryService to expire tokens.
/// </summary>
public class TokenExpiryJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public TokenExpiryJob (IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async void Execute()
    {
        using var serviceScope = _serviceScopeFactory.CreateScope();
        var tokenExpiryService = serviceScope.ServiceProvider.GetRequiredService<TokenExpiryService>();
        await tokenExpiryService.ExpireTokens();
    }
}