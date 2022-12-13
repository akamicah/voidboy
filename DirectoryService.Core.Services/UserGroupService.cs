using DirectoryService.Core.Entities;
using DirectoryService.Core.Exceptions;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;
using Microsoft.Extensions.Logging;

namespace DirectoryService.Core.Services;

[ScopedDependency]
public class UserGroupService
{
    private readonly ILogger<UserGroupService> _logger;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IUserGroupMembersRepository _userGroupMembersRepository;
    private readonly IUserRepository _userRepository;

    public UserGroupService(ILogger<UserGroupService> logger,
        IUserGroupRepository userGroupRepository,
        IUserGroupMembersRepository userGroupMembersRepository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userGroupRepository = userGroupRepository;
        _userGroupMembersRepository = userGroupMembersRepository;
        _userRepository = userRepository;
    }

    public async Task<UserGroup?> Create(Guid userId, UserGroup userGroup)
    {
        var user = await _userRepository.Retrieve(userId);
        if (user == null)
            throw new UserNotFoundApiException();
        
        userGroup.OwnerUserId = userId;
        _logger.LogInformation("User Group Created: {name} owned by {username}", userGroup.Name, user.Username);
        var newGroup = await _userGroupRepository.Create(userGroup);
        await _userGroupMembersRepository.Add(newGroup.Id, userId);
        return newGroup;
    }
    
    public async Task<UserGroup?> FindById(Guid userGroupId)
    {
        return await _userGroupRepository.Retrieve(userGroupId);
    }
    
    public async Task Delete(UserGroup userGroup)
    {
        await _userGroupRepository.Delete(userGroup.Id);
    }
    
    public async Task<PaginatedResponse<User>> GetGroupMembers(Guid userGroupId, PaginatedRequest page)
    {
        var result = await _userGroupMembersRepository.List(userGroupId, page);
        return result;
    }

    public async Task AddGroupMember(Guid userGroupId, Guid userId)
    {
        await _userGroupMembersRepository.Add(userGroupId, userId);
    }

    public async Task AddGroupMembers(Guid userGroupId, IEnumerable<Guid> userIds)
    {
        await _userGroupMembersRepository.Add(userGroupId, userIds);
    }

    public async Task RemoveGroupMember(Guid userGroupId, Guid userId)
    {
        await _userGroupMembersRepository.Delete(userGroupId, userId);
    }
    
    public async Task RemoveGroupMembers(Guid userGroupId, IEnumerable<Guid> userIds)
    {
        await _userGroupMembersRepository.Delete(userGroupId, userIds);
    }
}