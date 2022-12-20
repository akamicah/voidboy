using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class PlaceRepository : BaseRepository<Place>, IPlaceRepository
{
    public PlaceRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "places";
    }
    
     public async Task<Place> Create(Place entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO places (name, description, visibility, maturity, tags, domainId, path, thumbnailUrl, imageUrls, attendance, sessionToken, creatorIp, lastActivity)
                VALUES( @name, @description, @visibility, @maturity, @tags, @domainId, @path, @thumbnailUrl, @imageUrls, @attendance, @sessionToken, @creatorIp, @lastActivity)
                RETURNING id;",
            new
            {
                entity.Name,
                entity.Description,
                entity.Visibility,
                entity.Maturity,
                entity.Tags,
                entity.DomainId,
                entity.Path,
                entity.ThumbnailUrl,
                entity.ImageUrls,
                entity.Attendance,
                entity.SessionToken,
                entity.CreatorIp,
                entity.LastActivity
            });

        return await Retrieve(id);
    }

     public async Task<Place?> Update(Place entity)
    {
        throw new NotImplementedException();
    }
}