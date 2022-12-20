using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class PlaceManagerRepository : BaseRepository<User>, IPlaceManagerRepository
{
    public PlaceManagerRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "placeManagers";
    }
    
     public async Task<PaginatedResult<User>> List(Guid placeId, PaginatedRequest page)
    {
        const string sqlTemplate = @"SELECT * FROM (
            SELECT u.* FROM users u JOIN placeManagers pm ON u.id = pm.userId
            WHERE pm.placeId = @_pPlaceId) AS placeManagers";
        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("_pPlaceId", placeId);
        var result = await QueryDynamic(sqlTemplate, "placeManagers", page, dynamicParameters);
        return result;
    }

    public async Task Add(Guid placeId, Guid userId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"INSERT INTO placeManagers(placeId, userId) 
                                    VALUES (@placeId, @userId) ON CONFLICT(placeId, userId) DO NOTHING;",
            new
            {
                placeId,
                userId
            });
    }

    public async Task Add(Guid placeId, IEnumerable<Guid> userIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"INSERT INTO placeManagers(placeId, userId) 
                                    VALUES (@placeId, @userId) ON CONFLICT(placeId, userId) DO NOTHING;",
            userIds.Select(x => new { placeId, userId = x }).ToList());
    }

    public async Task Delete(Guid placeId, Guid userId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM placeManagers WHERE placeId = @placeId AND userId = @userIds",
            new
            {
                placeId,
                userId
            });
    }

    public async Task Delete(Guid placeId, IEnumerable<Guid> userIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM placeManagers WHERE placeId = @placeId AND userId = @userId",
            userIds.Select(x => new { placeId, userId = x }).ToList());
    }

    [Obsolete("Use List(Guid, PaginatedRequest) instead")]
    public override Task<PaginatedResult<User>> List(PaginatedRequest request)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use Add instead")]
    public Task<User> Create(User entity)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use List(Guid, PaginatedRequest) instead")]
    public override Task<User?> Retrieve(Guid id)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Not available for this repository")]
    public Task<User?> Update(User entity)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use Delete(Guid, Guid) instead")]
    public override Task Delete(Guid entityId)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Use Delete(Guid, IEnumerable<Guid>) instead")]
    public override Task Delete(IEnumerable<Guid> entityIds)
    {
        throw new NotImplementedException();
    }
}