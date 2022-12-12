using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
{
    public UserProfileRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "userProfiles";
    }

    public async Task<UserProfile> Create(UserProfile entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO userProfiles (userId, heroImageUrl, thumbnailImageUrl, tinyImageUrl)
                VALUES( @userId, @heroImageUrl, @thumbnailImageUrl, @tinyImageUrl)
                RETURNING id;",
            new
            {
                entity.UserId,
                entity.HeroImageUrl,
                entity.ThumbnailImageUrl,
                entity.TinyImageUrl
            });
        
        return await Retrieve(id);
    }

    public override async Task<UserProfile?> Retrieve(Guid id)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<UserProfile>(
            @"SELECT * FROM userProfiles WHERE userId = @id",
            new
            {
                id
            });
        return entity;
    }
    
    public async Task<UserProfile?> Update(UserProfile entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"UPDATE userProfiles SET heroImageUrl = @heroImageUrl, thumbnailImageUrl = @thumbnailImageUrl, tinyImageUrl = @tinyImageUrl WHERE userId = @userId",
            new
            {
                entity.UserId,
                entity.HeroImageUrl,
                entity.ThumbnailImageUrl,
                entity.TinyImageUrl
            });
        
        return await Retrieve(id);
    }
}