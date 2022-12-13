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

    /// <summary>
    /// Create a new usergroup where userId is the owner
    /// </summary>
    public async Task<UserGroup?> Create(Guid userId, UserGroup userGroup)
    {
        var user = await _userRepository.Retrieve(userId);
        if (user == null)
            throw new UserNotFoundApiException();
        
        userGroup.OwnerUserId = userId;
        userGroup.Internal = false;
        _logger.LogInformation("User Group Created: {name} owned by {username}", userGroup.Name, user.Username);
        var newGroup = await _userGroupRepository.Create(userGroup);
        await _userGroupMembersRepository.Add(newGroup.Id, userId);
        return newGroup;
    }
    
    /// <summary>
    /// Find a user group by id
    /// </summary>
    public async Task<UserGroup?> FindById(Guid userGroupId)
    {
        return await _userGroupRepository.Retrieve(userGroupId);
    }

    /// <summary>
    /// Update a user group. The only fields that can be changed are Name, Description and Rating
    /// </summary>
    public async Task<UserGroup?> Update(UserGroup userGroup)
    {
        return await _userGroupRepository.Update(userGroup);
    }
    
    /// <summary>
    /// Delete a user group
    /// </summary>
    public async Task Delete(UserGroup userGroup)
    {
        await _userGroupRepository.Delete(userGroup.Id);
    }
    
    /// <summary>
    /// Fetch a list of user group members
    /// </summary>
    public async Task<PaginatedResult<User>> GetGroupMembers(Guid userGroupId, PaginatedRequest page)
    {
        var result = await _userGroupMembersRepository.List(userGroupId, page);
        return result;
    }

    /// <summary>
    /// Add userId as a member of userGroupId. If the member exists then it will fail silently.
    /// </summary>
    public async Task AddGroupMember(Guid userGroupId, Guid userId)
    {
        await _userGroupMembersRepository.Add(userGroupId, userId);
    }

    /// <summary>
    /// Add a list of user ids to the user group.  If a member exists then it will fail silently.
    /// </summary>
    public async Task AddGroupMembers(Guid userGroupId, IEnumerable<Guid> userIds)
    {
        await _userGroupMembersRepository.Add(userGroupId, userIds);
    }

    /// <summary>
    /// Remove a user group member by id. Group owners cannot be removed.
    /// </summary>
    public async Task RemoveGroupMember(Guid userGroupId, Guid userId)
    {
        await _userGroupMembersRepository.Delete(userGroupId, userId);
    }
    
    /// <summary>
    /// Remove a group members by id. Group owners cannot be removed.
    /// </summary>
    public async Task RemoveGroupMembers(Guid userGroupId, IEnumerable<Guid> userIds)
    {
        await _userGroupMembersRepository.Delete(userGroupId, userIds);
    }
}