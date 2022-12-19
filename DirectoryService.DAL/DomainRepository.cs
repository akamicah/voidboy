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
            @"INSERT INTO domains (name, description, contactInfo, hostNames, thumbnailUrl, images, maturity, visibility, publicKey, apiKey, sponsorUserId, iceServerAddress, version, protocol, networkAddress, networkPort, networkingMode, restricted, numUsers, anonUsers, capacity, restriction, tags, creatorIp, lastHeartBeat, lastSenderKey) 
                VALUES( @name, @description, @contactInfo, @hostNames, @thumbnailUrl, @images, @maturity, @visibility, @publicKey, @apiKey, @sponsorUserId, @iceServerAddress, @version, @protocol, @networkAddress, @networkPort, @networkingMode, @restricted, @numUsers, @anonUsers, @capacity, @restriction, @tags, @creatorIp, @lastHeartBeat, @lastSenderKey)
                RETURNING id;",
            new
            {
                entity.Name,
                entity.Description,
                entity.ContactInfo,
                entity.HostNames,
                entity.ThumbnailUrl,
                entity.Images,
                entity.Maturity,
                entity.Visibility,
                entity.PublicKey,
                entity.ApiKey,
                entity.SponsorUserId,
                entity.IceServerAddress,
                entity.Version,
                entity.Protocol,
                entity.NetworkAddress,
                entity.NetworkPort,
                entity.NetworkingMode,
                entity.Restricted,
                entity.NumUsers,
                entity.AnonUsers,
                entity.Capacity,
                entity.Restriction,
                entity.Tags,
                entity.CreatorIp,
                entity.LastHeartbeat,
                entity.LastSenderKey
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