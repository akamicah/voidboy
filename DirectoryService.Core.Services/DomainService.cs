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
using DirectoryService.Shared.Extensions;
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
    private readonly UpdateDomainValidator _updateDomainValidator;
    private readonly UserService _userService;
    private readonly CryptographyService _cryptographyService;

    public DomainService(ILogger<DomainService> logger,
        IDomainRepository domainRepository,
        IDomainManagerRepository domainManagerRepository,
        ISessionProvider sessionProvider,
        RegisterDomainValidator registerDomainValidator,
        ISessionTokenRepository sessionTokenRepository,
        PlaceService placeService,
        UpdateDomainValidator updateDomainValidator,
        UserService userService,
        CryptographyService cryptographyService)
    {
        _logger = logger;
        _domainRepository = domainRepository;
        _domainManagerRepository = domainManagerRepository;
        _sessionProvider = sessionProvider;
        _registerDomainValidator = registerDomainValidator;
        _sessionTokenRepository = sessionTokenRepository;
        _serviceConfiguration = ServiceConfigurationContainer.Config;
        _placeService = placeService;
        _updateDomainValidator = updateDomainValidator;
        _userService = userService;
        _cryptographyService = cryptographyService;
    }

    private static bool SessionValidForDomain(Domain domain, Session session)
    {
        switch (session.Scope)
        {
            case TokenScope.Domain:
                if (session.Token == domain.SessionToken) return true;
                break;
            case TokenScope.Owner:
                if (session.UserId == domain.OwnerUserId || session.AsAdmin) return true;
                break;
        }
        return false;
    }

    public async Task<Domain?> FindById(Guid id)
    {
        return await _domainRepository.Retrieve(id);
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
            Visibility = DomainVisibility.Open,
            Active = true,
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

    public async Task<Domain> UpdateDomain(UpdateDomainDto updateDomainDto)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        await _updateDomainValidator.ValidateAndThrowAsync(updateDomainDto);
        
        var domain = await _domainRepository.Retrieve(updateDomainDto.DomainId);
        
        if (domain is null)
            throw new DomainNotFoundApiException();

        if (!SessionValidForDomain(domain, session)) throw new UnauthorisedApiException();

        domain.Name = updateDomainDto.Name ?? domain.Name;
        domain.Visibility =
            updateDomainDto.Visibility != null &&
            updateDomainDto.Visibility.ToDomainVisibility() != DomainVisibility.Invalid
                ? updateDomainDto.Visibility.ToDomainVisibility()
                : domain.Visibility;
        domain.Version = updateDomainDto.Version ?? domain.Version;
        domain.ProtocolVersion = updateDomainDto.Protocol ?? domain.ProtocolVersion;
        domain.NetworkAddress = updateDomainDto.NetworkAddress ?? domain.NetworkAddress;
        domain.Restricted = updateDomainDto.Restricted ?? domain.Restricted;
        domain.Capacity = updateDomainDto.Capacity ?? domain.Capacity;
        domain.Description = updateDomainDto.Description ?? domain.Description;
        domain.Maturity = updateDomainDto.Maturity?.ToMaturityRating() ?? domain.Maturity;
        domain.Restriction = updateDomainDto.Restriction?.ToDomainRestriction() ?? domain.Restriction;
        domain.Tags = updateDomainDto.Tags ?? domain.Tags;
        
        if (updateDomainDto.Heartbeat != null)
        {
            domain.UserCount = updateDomainDto.Heartbeat.NumUsers;
            domain.AnonCount = updateDomainDto.Heartbeat.NumAnonUsers;
            domain.LastHeartbeat = DateTime.Now;
        }
        
        if (updateDomainDto.Managers != null)
        {
            var newManagers = new List<User>();
            foreach (var manager in updateDomainDto.Managers)
            {
                var user = await _userService.FindUser(manager);
                if (user is null)
                    throw new BaseApiException("InvalidUser", "Managers contains one or more unknown users");
                newManagers.Add(user);
            }

            await SetDomainManagers(domain.Id, newManagers);
        }

        await _domainRepository.Update(domain);
        return domain;
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

    /// <summary>
    /// Replace current domain managers with provided list
    /// </summary>
    private async Task SetDomainManagers(Guid domainId, List<User> managers)
    {
        // Remove old managers
        var currentManagers = await _domainManagerRepository.List(domainId, PaginatedRequest.All());
        await _domainManagerRepository.Delete(domainId, currentManagers.Data!.Select(m => m.Id));

        foreach (var user in managers)
        {
            await _domainManagerRepository.Add(domainId, user.Id);
        }
    }

    /// <summary>
    /// Retrieve a domain field
    /// </summary>
    public async Task<object?> GetDomainField(Guid domainId, string field)
    {
        var domain = await _domainRepository.Retrieve(domainId);
        
        if (domain is null)
            throw new DomainNotFoundApiException();
        
        switch (field.ToLower())
        {
            case "domainid":
                return domain.Id;
            case "name":
                return domain.Name;
            case "visibility":
                return domain.Visibility.ToDomainVisibilityString();
            case "public_key":
                return domain.PublicKey;
            case "sponsor_account_id":
                return domain.OwnerUserId;
            case "version":
                return domain.Version;
            case "protocol":
                return domain.ProtocolVersion;
            case "network_address":
                return domain.NetworkAddress;
            case "automatic_networking":
                return domain.NetworkingMode.ToNetworkingModeString();
            case "num_users":
                return domain.UserCount;
            case "num_anon_users":
                return domain.AnonCount;
            case "restricted":
                return domain.Restricted;
            case "capacity":
                return domain.Capacity;
            case "description":
                return domain.Description;
            case "contact_info":
                return domain.ContactInfo;
            case "maturity":
                return domain.Maturity.ToMaturityRatingString();
            case "restriction":
                return domain.Restriction.ToDomainRestrictionString();
            case "managers":
                var managers = await GetDomainManagers(domainId);
                return managers.ToArray();
            case "tags":
                return domain.Tags?.ToArray();
            case "thumbnail":
                return domain.ThumbnailUrl;
            case "images":
                return domain.ImageUrls?.ToArray();
            case "addr_of_first_contact":
                return domain.CreatorIp;
            case "when_domain_entry_created":
                return domain.CreatedAt.ToUniversalIso8601();
            case "when_domain_entry_created_s":
                return domain.CreatedAt.ToMilliSecondsTimestamp();
            case "time_of_last_heartbeat":
                return domain.LastHeartbeat.ToUniversalIso8601();
            case "time_of_last_heartbeat_s":
                return domain.LastHeartbeat.ToMilliSecondsTimestamp();
            case "last_sender_key": //TODO: Required?
                return null;
        }

        return null;
    }

    /// <summary>
    /// Update the domain's public key. Expects key in PKCS1 DER (binary) form
    /// </summary>
    public async Task UpdatePublicKey(Guid domainId, Stream publicKey)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        var domain = await _domainRepository.Retrieve(domainId);
        
        if (domain is null)
            throw new DomainNotFoundApiException();
        
        if (!SessionValidForDomain(domain, session)) throw new UnauthorisedApiException();
        
        var pemPublicKey = await _cryptographyService.ConvertPkcs1Key(publicKey.ToByteArray());

        domain.PublicKey = pemPublicKey;
        await _domainRepository.Update(domain);
    }
    
    public async Task UpdateIceServerAddress(Guid domainId, string iceServerAddress)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        var domain = await _domainRepository.Retrieve(domainId);
        
        if (domain is null)
            throw new DomainNotFoundApiException();
        
        if (!SessionValidForDomain(domain, session)) throw new UnauthorisedApiException();

        domain.IceServerAddress = iceServerAddress;
        await _domainRepository.Update(domain);
    }
}