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
            @"SELECT * FROM users WHERE id = :id AND deleted = FALSE",
            new
            {
                id
            });
        
        return entity;
    }

    public async Task<User?> Update(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task Delete(IEnumerable<User> entities)
    {
        throw new NotImplementedException();
    }

    public async Task HardDelete(User entity)
    {
        throw new NotImplementedException();
    }

    public async Task HardDelete(IEnumerable<User> entities)
    {
        throw new NotImplementedException();
    }
    
    /// <summary>
    /// Delete all entities where deleted = true
    /// </summary>
    public async Task PurgeDeleted()
    {
        using var con = await _dbContext.CreateConnectionAsync();
        await con.ExecuteAsync(@"DELETE FROM users WHERE deleted = TRUE");
    }

    public async Task<User?> FindByUsername(string username)
    {
        using var con = await _dbContext.CreateConnectionAsync();
        var entity = await con.QueryFirstOrDefaultAsync<User>(
            @"SELECT * FROM users WHERE LOWER(username) = :username AND deleted = FALSE",
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
            @"SELECT * FROM users WHERE email = :emailAddress AND deleted = FALSE",
            new
            {
                emailAddress
            });
        
        return entity;
    }
    
}