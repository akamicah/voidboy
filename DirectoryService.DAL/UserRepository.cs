using Dapper;
using DirectoryService.Core.Entities;
using DirectoryService.Core.RepositoryInterfaces;
using DirectoryService.DAL.Infrastructure;
using DirectoryService.Shared.Attributes;

namespace DirectoryService.DAL;

[ScopedRegistration]
public class UserRepository : IUserRepository
{
    private readonly DbContext _dbContext;

    public UserRepository(DbContext db)
    {
        _dbContext = db;
    }
    
    /// <summary>
    /// Store a new User entity in the database
    /// </summary>
    public async Task<User?> Create(User entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
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

    /// <summary>
    /// Fetch a user by it's id
    /// </summary>
    public async Task<User?> Retrieve(Guid id)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM users WHERE id = :id",
            new
            {
                id
            });
        
        return entity;
    }

    public async Task<User?> Update(User entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
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

    public async Task Delete(User entity)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM users WHERE id = :id",
            new
            {
                entity.Id
            });
    }

    public async Task Delete(IEnumerable<User> entities)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(
            @"DELETE FROM users WHERE id = :id",
            entities.Select(e => e.Id)
                .Select(i => new { Id = i }).ToArray());
    }

    public async Task<User?> FindByUsername(string username)
    {
        using var con = await _dbContext.CreateConnectionAsync();
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
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM users WHERE email = :emailAddress",
            new
            {
                emailAddress
            });
        
        return entity;
    }
    
}