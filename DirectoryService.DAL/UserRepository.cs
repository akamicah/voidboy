using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedRegistration]
public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(DbContext dbContext) : base(dbContext)
    {
        TableName = "users";
    }
    
    /// <summary>
    /// Store a new User entity in the database
    /// </summary>
    public async Task<User?> Create(User entity)
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
    
}