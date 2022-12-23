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

    public async Task<RegisteredDomainDto> RegisterNewDomain(RegisterDomainDto registerDomainDto, bool createPlace = true)
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

        var domain = await _domainRepository.Create(newDomain);

        await _domainManagerRepository.Add(domain.Id, session.UserId);

        if (!createPlace)
            return new RegisteredDomainDto()
            {
                RegisteredDomain = domain,
                RegisteredPlace = null
            };
        
        var place = await _placeService.RegisterNewPlace(new RegisterPlaceDto()
        {
            CreatorIp = registerDomainDto.OriginIp ?? IPAddress.Any,
            Name = domain.Name,
            Description = "A place in " + domain.Name,
            DomainId = domain.Id,
            Path = "/0,0,0/0,0,0,1"
        });
        
        return new RegisteredDomainDto()
        {
            RegisteredDomain = domain,
            RegisteredPlace = place
        };
    }

    public async Task DeleteDomain(Guid domainId)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        var domain = await _domainRepository.Retrieve(domainId);
        
        if (domain is null)
            throw new DomainNotFoundApiException();
        
        if (session.UserId != domain.OwnerUserId && !session.AsAdmin)
            throw new UnauthorisedApiException();
        
        _logger.LogInformation("Deleting domain: {name}", domain.Name);
        
        // Get places belonging to domain and delete them
        var page = PaginatedRequest.All();
        page.Where.Add("domainId", domain.Id);
        var places = await _placeService.List(page);
        
        foreach(var place in places.Data!)
        {
            await _placeService.DeletePlace(place.Id);
        }

        await _domainRepository.Delete(domain.Id);
        await _sessionTokenRepository.Delete(domain.SessionToken);
    }

    public async Task<List<User>> GetDomainManagers(Guid domainId)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        var domain = await _domainRepository.Retrieve(domainId);

        if (domain is null)
            throw new DomainNotFoundApiException();

        if (session.UserId != domain.OwnerUserId && !session.AsAdmin)
            throw new UnauthorisedApiException();

        var managers = await _domainManagerRepository.List(domainId, PaginatedRequest.All());

        return managers.Data!.ToList();
    }
}