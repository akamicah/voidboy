using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserGroupRepository : BaseRepository<UserGroup>, IUserGroupRepository
{
    public UserGroupRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "userGroups";
    }

    public async Task<UserGroup> Create(UserGroup entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO userGroups (ownerUserId, internal, name, description, rating)
                VALUES( @ownerUserId, @internal, @name, @description, @rating )
                RETURNING id;",
            new
            {
                entity.OwnerUserId,
                entity.Internal,
                entity.Name,
                entity.Description,
                entity.Rating
            });

        var result = await Retrieve(id);

        if (result is not null)
            await con.ExecuteAsync(@"INSERT INTO userGroupMembers(userGroupId, userId, isOwner) 
                                    VALUES (@groupId, @userId, true);",
                new
                {
                    GroupId = result.Id,
                    UserId = entity.OwnerUserId
                });

        return result;
    }
    
    public async Task<UserGroup?> Update(UserGroup entity)
    {
        throw new NotImplementedException();
    }

   
}