using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

public class DomainService
{
    private readonly ILogger<DomainService> _logger;
    private readonly IDomainRepository _domainRepository;
    private readonly ISessionProvider _sessionProvider;

    public DomainService(ILogger<DomainService> logger,
        IDomainRepository domainRepository,
        ISessionProvider sessionProvider)
    {
        _logger = logger;
        _domainRepository = domainRepository;
        _sessionProvider = sessionProvider;
    }


}