using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Core.Validators;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

[ScopedDependency]
public class DomainService
{
    private readonly ILogger<DomainService> _logger;
    private readonly IDomainRepository _domainRepository;
    private readonly IDomainManagerRepository _domainManagerRepository;
    private readonly ISessionProvider _sessionProvider;
    private readonly RegisterDomainValidator _registerDomainValidator;

    public DomainService(ILogger<DomainService> logger,
        IDomainRepository domainRepository,
        IDomainManagerRepository domainManagerRepository,
        ISessionProvider sessionProvider,
        RegisterDomainValidator registerDomainValidator)
    {
        _logger = logger;
        _domainRepository = domainRepository;
        _domainManagerRepository = domainManagerRepository;
        _sessionProvider = sessionProvider;
        _registerDomainValidator = registerDomainValidator;
    }

    public async Task<Domain> RegisterNewDomain(RegisterDomainDto registerDomainDto)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        await _registerDomainValidator.ValidateAndThrowAsync(registerDomainDto);
        
        //TODO: Domain name uniqueness not enforced currently. Should it be?

        _logger.LogInformation("Registering new domain: {name}", registerDomainDto.Name);
        
        var newDomain = new Domain()
        {
            ApiKey = Guid.NewGuid(),
            Name = registerDomainDto.Name,
            CreatorIp = registerDomainDto.OriginIp?.ToString(),
            NetworkAddress = registerDomainDto.NetworkAddress,
            NetworkPort = registerDomainDto.NetworkPort,
            SponsorUserId = session.UserId,
            Tags = new List<string>(),
            Images = new List<string>(),
            HostNames = new List<string>(),
            Restricted = false,
            Restriction = DomainRestriction.Open,
            Maturity = MaturityRating.Unrated,
            LastHeartbeat = DateTime.Now
        };

        var entity = await _domainRepository.Create(newDomain);

        await _domainManagerRepository.Add(entity.Id, session.UserId);
        
        // TODO: Create place
        
        return entity;
    } 
}