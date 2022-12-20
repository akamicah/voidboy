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
    private readonly IPlaceRepository _placeRepository;
    private readonly IPlaceManagerRepository _placeManagerRepository;
    private readonly ISessionProvider _sessionProvider;
    private readonly RegisterDomainValidator _registerDomainValidator;
    private readonly ISessionTokenRepository _sessionTokenRepository;
    private readonly ServiceConfiguration _serviceConfiguration;

    public DomainService(ILogger<DomainService> logger,
        IDomainRepository domainRepository,
        IDomainManagerRepository domainManagerRepository,
        IPlaceRepository placeRepository,
        IPlaceManagerRepository placeManagerRepository,
        ISessionProvider sessionProvider,
        RegisterDomainValidator registerDomainValidator,
        ISessionTokenRepository sessionTokenRepository)
    {
        _logger = logger;
        _domainRepository = domainRepository;
        _domainManagerRepository = domainManagerRepository;
        _placeRepository = placeRepository;
        _placeManagerRepository = placeManagerRepository;
        _sessionProvider = sessionProvider;
        _registerDomainValidator = registerDomainValidator;
        _sessionTokenRepository = sessionTokenRepository;
        _serviceConfiguration = ServiceConfigurationContainer.Config;
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
        
        _logger.LogInformation("Registering new default place in: {name}", registerDomainDto.Name);
        
        var placeSessionToken = await _sessionTokenRepository.Create(new SessionToken()
        {
            Scope = TokenScope.Place,
            UserId = session.UserId,
            Expires = DateTime.Now.Add(TimeSpan.FromHours(_serviceConfiguration.Tokens.DomainTokenLifetimeHours))
        });

        var placeEntity = await _placeRepository.Create(new Place()
        {
            Name = domainEntity.Name,
            Description = "A place in " + domainEntity.Name,
            Visibility = domainEntity.Visibility,
            Maturity = domainEntity.Maturity,
            Tags = domainEntity.Tags,
            ThumbnailUrl = domainEntity.ThumbnailUrl,
            ImageUrls = domainEntity.ImageUrls,
            Attendance = 0,
            SessionToken = placeSessionToken.Id,
            CreatorIp = registerDomainDto.OriginIp?.ToString(),
            DomainId = domainEntity.Id,
            LastActivity = DateTime.Now,
            Path = "/0,0,0/0,0,0,1"
        });

        await _placeManagerRepository.Add(placeEntity.Id, session.UserId);

        return newDomain;
    } 
}