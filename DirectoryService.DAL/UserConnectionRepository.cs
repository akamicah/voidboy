using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserConnectionRepository : BaseRepository<UserConnection>, IUserConnectionRepository
{
    
    public UserConnectionRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "userConnections";
    }
    
    public async Task<UserConnection> Create(UserConnection entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO userConnections (userAId, userBId)
                VALUES( @userAId, @userBId)
                RETURNING id;",
            new
            {
                entity.UserAId,
                entity.UserBId,
            });
        
        return await Retrieve(id);
    }

    public async Task<UserConnection?> Update(UserConnection entity)
    {
        throw new NotImplementedException();
    }
}