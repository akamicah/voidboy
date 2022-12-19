using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedDependency]
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "users";
    }

    /// <summary>
    /// Store a new User entity in the database
    /// </summary>
    public async Task<User> Create(User entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var id = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO users (identityProvider, username, email, authVersion, authHash, activated, role, state, creatorIp)
                VALUES( @identityProvider, @username, @email, @authVersion, @authHash, @activated, @role, @state, @creatorIp )
                RETURNING id;",
            new
            {
                entity.IdentityProvider,
                entity.Username,
                entity.Email,
                entity.AuthVersion,
                entity.AuthHash,
                entity.Activated,
                entity.Role,
                entity.State,
                entity.CreatorIp
            });

        var connectionGroup = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO userGroups (ownerUserId, internal, name, description, rating)
                VALUES( @ownerUserId, @internal, @name, @description, @rating )
                RETURNING id;",
            new
            {
                OwnerUserId = id,
                Internal = true,
                Name = entity.Username + " Connections",
                Description = entity.Username + " Connections",
                Rating = MaturityRating.Everyone
            });
        
        var friendsGroup = await con.QuerySingleAsync<Guid>(
            @"INSERT INTO userGroups (ownerUserId, internal, name, description, rating)
                VALUES( @ownerUserId, @internal, @name, @description, @rating )
                RETURNING id;",
            new
            {
                OwnerUserId = id,
                Internal = true,
                Name = entity.Username + " Friends",
                Description = entity.Username + " Friends",
                Rating = MaturityRating.Everyone
            });

        entity.ConnectionGroup = connectionGroup;
        entity.FriendsGroup = friendsGroup;
        
        await Update(entity);
        
        return await Retrieve(id);
    }

    public async Task<User?> Update(User entity)
    {
        using var con = await DbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"UPDATE users SET email = @email,
                 authVersion = @authVersion,
                 authHash = @authHash,
                 activated = @activated,
                 role = @role,
                 state = @state,
                 connectionGroup = @connectionGroup,
                 friendGroup = @friendsGroup
                 WHERE id = @id;",
            new
            {
                entity.Id,
                entity.Email,
                entity.AuthVersion,
                entity.AuthHash,
                entity.Activated,
                entity.Role,
                entity.State,
                entity.ConnectionGroup,
                entity.FriendsGroup
            });

        return await Retrieve(entity.Id);
    }


    public async Task<User?> FindByUsername(string username)
    {
        using var con = await DbContext.CreateConnectionAsync();
        username = username.ToLower();
        var entity = await con.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM users WHERE LOWER(username) = :username",
            new
            {
                username
            });

        return entity;
    }

    public async Task<User?> FindByEmail(string emailAddress)
    {
        using var con = await DbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM users WHERE email = :emailAddress",
            new
            {
                emailAddress
            });

        return entity;
    }

    public async Task<PaginatedResult<User>> ListRelativeUsers(Guid relativeUser, PaginatedRequest page, bool includeSelf)
    {
        const string sqlTemplate = $@"SELECT * FROM (SELECT u.*, CASE WHEN u.id = @selfId THEN TRUE ELSE FALSE END AS self,
                      COALESCE(connections.isConnection, FALSE) AS connection,
                      COALESCE(friends.isFriend, FALSE) AS friend FROM users u
LEFT JOIN (SELECT CASE WHEN ugm.userid IS NULL THEN FALSE ELSE TRUE END AS isConnection, ugm.userId AS userId
           FROM userGroupMembers ugm
         JOIN users u ON u.id = @selfId
           WHERE ugm.userGroupId = u.connectionGroup GROUP BY ugm.userId) AS connections ON connections.userId = u.id
LEFT JOIN (SELECT CASE WHEN ugm.userid IS NULL THEN FALSE ELSE TRUE END AS isFriend, ugm.userId As userId
           FROM userGroupMembers ugm
                    JOIN users u ON u.id = @selfId
           WHERE ugm.userGroupId = u.friendGroup GROUP BY ugm.userId) AS friends ON friends.userId = u.id) AS relativeUsers ";

        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("selfId", relativeUser);

        if(!includeSelf)
            page.Where.Add("self", false);

            return await QueryDynamic(sqlTemplate, "relativeUsers", page, dynamicParameters);
    }
}