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
                 state = @state
                 WHERE id = @id;",
            new
            {
                entity.Id,
                entity.Email,
                entity.AuthVersion,
                entity.AuthHash,
                entity.Activated,
                entity.Role,
                entity.State
            });

        return await Retrieve(entity.Id);
    }


    public async Task<User?> FindByUsername(string username)
    {
        using var con = await DbContext.CreateConnectionAsync();
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

    public async Task<PaginatedResponse<User>> ListRelativeUsers(Guid relativeUser, PaginatedRequest page, bool includeSelf)
    {
        const string sqlTemplate = $@"SELECT * FROM 
             (SELECT u.*, CASE WHEN u.id = @selfId AND @includeSelf = TRUE THEN TRUE 
            ELSE CASE WHEN connection.isConnection = TRUE THEN TRUE ELSE FALSE END END connection
            FROM users u LEFT JOIN (SELECT CASE WHEN COUNT(*) > 0 THEN TRUE ELSE FALSE END AS isConnection,
            uc.userAId FROM userConnections uc WHERE userBId = @selfId GROUP BY userAId)
            AS connection ON connection.userAId = u.id) AS t ";

        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("selfId", relativeUser);
        dynamicParameters.Add("includeSelf", includeSelf);

        return await QueryDynamic(sqlTemplate, "t", page, dynamicParameters);
    }
}