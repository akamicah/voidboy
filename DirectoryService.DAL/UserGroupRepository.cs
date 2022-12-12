using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
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

        return await Retrieve(id);
    }
    
    public async Task<UserGroup?> Update(UserGroup entity)
    {
        throw new NotImplementedException();
    }
}