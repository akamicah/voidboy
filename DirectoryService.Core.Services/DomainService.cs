using System.Net;
using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Core.Validators;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
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
    private readonly ISessionTokenRepository _sessionTokenRepository;
    private readonly ServiceConfiguration _serviceConfiguration;
    private readonly PlaceService _placeService;

    public DomainService(ILogger<DomainService> logger,
        IDomainRepository domainRepository,
        IDomainManagerRepository domainManagerRepository,
        ISessionProvider sessionProvider,
        RegisterDomainValidator registerDomainValidator,
        ISessionTokenRepository sessionTokenRepository,
        PlaceService placeService)
    {
        _logger = logger;
        _domainRepository = domainRepository;
        _domainManagerRepository = domainManagerRepository;
        _sessionProvider = sessionProvider;
        _registerDomainValidator = registerDomainValidator;
        _sessionTokenRepository = sessionTokenRepository;
        _serviceConfiguration = ServiceConfigurationContainer.Config;
        _placeService = placeService;
    }

    public async Task<Domain> RegisterNewDomain(RegisterDomainDto registerDomainDto, bool createPlace = true)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        await _registerDomainValidator.ValidateAndThrowAsync(registerDomainDto);
        
        //TODO: Domain name uniqueness not enforced currently. Should it be?

        _logger.LogInformation("Registering new domain: {name}", registerDomainDto.Name);

        var domainSessionToken = await _sessionTokenRepository.Create(new SessionToken()
        {
            Scope = TokenScope.Domain,
            UserId = session.UserId,
            Expires = DateTime.Now.Add(TimeSpan.FromHours(_serviceConfiguration.Tokens.DomainTokenLifetimeHours)) 
        });
        
        var newDomain = new Domain()
        {
            SessionToken = domainSessionToken.Id,
            Name = registerDomainDto.Name,
            CreatorIp = registerDomainDto.OriginIp?.ToString(),
            NetworkAddress = registerDomainDto.NetworkAddress,
            NetworkPort = registerDomainDto.NetworkPort,
            OwnerUserId = session.UserId,
            Tags = new List<string>(),
            ImageUrls = new List<string>(),
            ThumbnailUrl = "",
            Restricted = false,
            Restriction = DomainRestriction.Open,
            Maturity = MaturityRating.Unrated,
            LastHeartbeat = DateTime.Now
        };

        var domainEntity = await _domainRepository.Create(newDomain);

        await _domainManagerRepository.Add(domainEntity.Id, session.UserId);

        if (!createPlace)
            return domainEntity;
        
        await _placeService.RegisterNewPlace(new RegisterPlaceDto()
        {
            CreatorIp = registerDomainDto.OriginIp ?? IPAddress.Any,
            Name = domainEntity.Name,
            Description = "A place in " + domainEntity.Name,
            DomainId = domainEntity.Id,
            Path = "/0,0,0/0,0,0,1"
        });
        
        return newDomain;
    } 
}