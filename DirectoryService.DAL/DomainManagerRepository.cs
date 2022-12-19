using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class DomainManagerRepository : BaseRepository<User>, IDomainManagerRepository
{
    public DomainManagerRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "domainManagers";
    }
    
     public async Task<PaginatedResult<User>> List(Guid domainId, PaginatedRequest page)
    {
        const string sqlTemplate = @"SELECT * FROM (
            SELECT u.* FROM users u JOIN domainManagers dm ON u.id = dm.userId
            WHERE dm.domainId = @_pDomainId) AS domainManagers";
        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("_pDomainId", domainId);
        var result = await QueryDynamic(sqlTemplate, "domainManagers", page, dynamicParameters);
        return result;
    }

    public async Task Add(Guid domainId, Guid userId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"INSERT INTO domainManagers(domainId, userId) 
                                    VALUES (@domainId, @userId) ON CONFLICT(domainId, userId) DO NOTHING;",
            new
            {
                domainId,
                userId
            });
    }

    public async Task Add(Guid domainId, IEnumerable<Guid> userIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"INSERT INTO domainManagers(domainId, userId) 
                                    VALUES (@domainId, @userId) ON CONFLICT(domainId, userId) DO NOTHING;",
            userIds.Select(x => new { domainId, userId = x }).ToList());
    }

    public async Task Delete(Guid domainId, Guid userId)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM domainManagers WHERE domainId = @domainId AND userId = @userIds",
            new
            {
                domainId,
                userId
            });
    }

    public async Task Delete(Guid domainId, IEnumerable<Guid> userIds)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM domainManagers WHERE domainId = @domainId AND userId = @userId",
            userIds.Select(x => new { domainId, userId = x }).ToList());
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