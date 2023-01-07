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
            @"INSERT INTO domains (name, description, contactInfo, thumbnailUrl, imageUrls, maturity, visibility, publicKey, sessionToken, ownerUserId, iceServerAddress, version, protocolVersion, networkAddress, networkPort, networkingMode, restricted, userCount, anonCount, capacity, restriction, tags, creatorIp, lastHeartBeat) 
                VALUES( @name, @description, @contactInfo, @thumbnailUrl, @ImageUrls, @maturity, @visibility, @publicKey, @sessionToken, @ownerUserId, @iceServerAddress, @version, @protocolVersion, @networkAddress, @networkPort, @networkingMode, @restricted, @userCount, @anonCount, @capacity, @restriction, @tags, @creatorIp, @lastHeartBeat)
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
                entity.ProtocolVersion,
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
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.ExecuteAsync(
            @"UPDATE domains SET name = @name,
                   description = @description,
                   contactInfo = @contactInfo,
                   thumbnailUrl = @thumbnailUrl,
                   imageUrls = @imageUrls,
                   maturity = @maturity,
                   visibility = @visibility,
                   publicKey = @publicKey,
                   sessionToken = @sessionToken,
                   ownerUserId = @ownerUserId,
                   iceServerAddress = @iceServerAddress,
                   version = @version,
                   protocolVersion = @protocolVersion,
                   networkAddress = @networkAddress,
                   networkPort = @networkPort,
                   networkingMode = @networkingMode,
                   restricted = @restricted,
                   capacity = @capacity,
                   restriction = @restriction,
                   tags = @tags,
                   creatorIp = @creatorIp,
                   lastHeartbeat = @lastHeartbeat,
                   active = @active,
                   anonCount = @anonCount,
                   userCount = @userCount
               WHERE id = @id",
            new
            {
                entity.Id,
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
                entity.ProtocolVersion,
                entity.NetworkAddress,
                entity.NetworkPort,
                entity.NetworkingMode,
                entity.Restricted,
                entity.Capacity,
                entity.Restriction,
                entity.Tags,
                entity.CreatorIp,
                entity.LastHeartbeat,
                entity.Active,
                entity.AnonCount,
                entity.UserCount
            });

        return await Retrieve(entity.Id);
    }
}