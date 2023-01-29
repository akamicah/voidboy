using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserPresenceRepository : BaseRepository<UserPresence>, IUserPresenceRepository
{
    public UserPresenceRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "userPresence";
    }

    public async Task<UserPresence> Create(UserPresence entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"INSERT INTO userPresence (id,
                          connected,
                          domainId,
                          placeId,
                          networkAddress,
                          nodeId, 
                          availability,
                          publicKey,
                          path,
                          lastHeartbeat)
                VALUES( @id, @connected, @domainId, @placeId, @networkAddress, @nodeId, @availability, @publicKey, @path, @lastHeartbeat)
                ON CONFLICT(id) DO UPDATE 
                    SET connected = excluded.connected,
                        domainId = excluded.domainId,
                        placeId = excluded.placeId,
                        networkaddress = excluded.networkAddress,
                        nodeId = excluded.nodeId,
                        availability = excluded.availability,
                        publicKey = excluded.publicKey,
                        path = excluded.path,
                        lastHeartbeat = excluded.lastHeartbeat",
            new
            {
                entity.Id,
                entity.Connected,
                entity.DomainId,
                entity.PlaceId,
                entity.NetworkAddress,
                entity.NodeId,
                entity.Availability,
                entity.PublicKey,
                entity.Path,
                entity.LastHeartbeat
            });
        
        return await Retrieve(entity.Id);
    }

    public override async Task<UserPresence?> Retrieve(Guid id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<UserPresence>(
            @"SELECT * FROM userPresence WHERE id = @id",
            new
            {
                id
            });
        return entity;
    }
    
    public async Task<UserPresence?> Update(UserPresence entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"UPDATE userPresence SET 
                        connected = @connected,
                        domainId = @domainId,
                        placeId = @placeId,
                        networkAddress = @networkAddress,
                        nodeId = @nodeId,
                        availability = @availability,                        
                        publicKey = @publicKey,
                        path = @path,                        
                        lastHeartbeat = @lastHeartbeat 
                    WHERE id = @userId",
            new
            {
                entity.Id,
                entity.Connected,
                entity.DomainId,
                entity.PlaceId,
                entity.NetworkAddress,
                entity.NodeId,
                entity.Availability,
                entity.PublicKey,
                entity.Path,
                entity.LastHeartbeat
            });
        
        return await Retrieve(entity.Id);
    }
}