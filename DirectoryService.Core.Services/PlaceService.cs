using DirectoryService.Core.Dto;
using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Core.Services.Interfaces;
using DirectoryService.Core.Validators;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using DirectoryService.Shared.Helpers;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

[ScopedDependency]
public class PlaceService
{
    private readonly ILogger<PlaceService> _logger;
    private readonly IPlaceRepository _placeRepository;
    private readonly IDomainRepository _domainRepository;
    private readonly ISessionProvider _sessionProvider;
    private readonly RegisterPlaceValidator _registerPlaceValidator;
    private readonly ISessionTokenRepository _sessionTokenRepository;
    private readonly ServiceConfiguration _serviceConfiguration;

    public PlaceService(ILogger<PlaceService> logger,
        IPlaceRepository placeRepository,
        IDomainRepository domainRepository,
        ISessionProvider sessionProvider,
        RegisterPlaceValidator registerPlaceValidator,
        ISessionTokenRepository sessionTokenRepository)
    {
        _logger = logger;
        _placeRepository = placeRepository;
        _domainRepository = domainRepository;
        _sessionProvider = sessionProvider;
        _registerPlaceValidator = registerPlaceValidator;
        _sessionTokenRepository = sessionTokenRepository;
        _serviceConfiguration = ServiceConfigurationContainer.Config;
    }

    private async Task<string> GenerateUniquePlaceName(string requestedName)
    {
        var found = await _placeRepository.FindByName(requestedName);
        if (found is null)
            return requestedName;
        var uniqueName = requestedName;
        while (found is not null)
        {
            uniqueName = requestedName + StringHelper.GenerateRandomAlphanumericString(5);
            found = await _placeRepository.FindByName(uniqueName);    
        }
        return uniqueName;
    }
    
    public async Task<Place> RegisterNewPlace(RegisterPlaceDto registerPlaceDto)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();
        
        await _registerPlaceValidator.ValidateAndThrowAsync(registerPlaceDto);

        var domain = await _domainRepository.Retrieve(registerPlaceDto.DomainId);

        if (domain is null)
            throw new DomainNotFoundApiException();

        if (session.UserId != domain.OwnerUserId && !session.AsAdmin)
            throw new UnauthorisedApiException();

        var placeName = await GenerateUniquePlaceName(registerPlaceDto.Name!);
        
        _logger.LogInformation("Registering new place: {placeName} in domain {domainName}", placeName, domain.Name);

        var placeSessionToken = await _sessionTokenRepository.Create(new SessionToken()
        {
            Scope = TokenScope.Place,
            UserId = domain.OwnerUserId,
            Expires = DateTime.Now.Add(TimeSpan.FromHours(_serviceConfiguration.Tokens.DomainTokenLifetimeHours))
        });

        var placeEntity = await _placeRepository.Create(new Place()
        {
            Name = placeName,
            Description = "A place in " + domain.Name,
            Visibility = domain.Visibility,
            Maturity = domain.Maturity,
            Tags = domain.Tags,
            ThumbnailUrl = domain.ThumbnailUrl,
            ImageUrls = domain.ImageUrls,
            Attendance = 0,
            SessionToken = placeSessionToken.Id,
            CreatorIp = registerPlaceDto.CreatorIp?.ToString(),
            DomainId = domain.Id,
            LastActivity = DateTime.Now,
            Path = registerPlaceDto.Path
        });

        return placeEntity;
    }

    public async Task DeletePlace(Guid placeId)
    {
        var session = await _sessionProvider.GetRequesterSession();
        if (session is null ) throw new UnauthorisedApiException();

        var place = await _placeRepository.Retrieve(placeId);
        
        if (place is null) throw new PlaceNotFoundApiException();

        var domain = await _domainRepository.Retrieve(place.DomainId);

        if (domain is null)
        {
            _logger.LogWarning("Place {placeName}'s parent domain no longer exists! Deleting place.", place.Name);
            await _placeRepository.Delete(placeId);
            await _sessionTokenRepository.Delete(place.SessionToken);
            return;
        }
        
        if (session.UserId != domain.OwnerUserId && !session.AsAdmin)
            throw new UnauthorisedApiException();
        
        await _placeRepository.Delete(placeId);
        await _sessionTokenRepository.Delete(place.SessionToken);
    }
}