using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class DomainRepository : BaseRepository<Domain>, IDomainRepository
{
    public DomainRepository(DbContext db) : base(db)
    {
        TableName = "domains";
    }

    public async Task<Domain> Create(Domain entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO domains (name, description, contactInfo, thumbnailUrl, imageUrls, maturity, visibility, publicKey, sessionToken, ownerUserId, iceServerAddress, version, protocol, networkAddress, networkPort, networkingMode, restricted, userCount, anonCount, capacity, restriction, tags, creatorIp, lastHeartBeat) 
                VALUES( @name, @description, @contactInfo, @thumbnailUrl, @ImageUrls, @maturity, @visibility, @publicKey, @sessionToken, @ownerUserId, @iceServerAddress, @version, @protocol, @networkAddress, @networkPort, @networkingMode, @restricted, @userCount, @anonCount, @capacity, @restriction, @tags, @creatorIp, @lastHeartBeat)
                RETURNING id;",
            new
            {
                entity.Name,
                entity.Description,
                entity.ContactInfo,
                entity.ThumbnailUrl,
                entity.ImageUrls,
                entity.Maturity,
                entity.Visibility,
                entity.PublicKey,
                entity.SessionToken,
                entity.OwnerUserId,
                entity.IceServerAddress,
                entity.Version,
                entity.Protocol,
                entity.NetworkAddress,
                entity.NetworkPort,
                entity.NetworkingMode,
                entity.Restricted,
                entity.UserCount,
                entity.AnonCount,
                entity.Capacity,
                entity.Restriction,
                entity.Tags,
                entity.CreatorIp,
                entity.LastHeartbeat,
            });

        return await Retrieve(id);
    }

    public async Task<Domain?> FindByName(string name)
    {
        using var con = await DbContext.CreateConnectionAsync();
        name = name.ToLower();
        var entity = await con.QueryFirstOrDefaultAsync<Domain>(
            @"SELECT * FROM domains WHERE LOWER(name) = :name",
            new
            {
                name
            });

        return entity;
    }
    
    public async Task<Domain?> Update(Domain entity)
    {
        throw new NotImplementedException();
    }
}