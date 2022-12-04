using System.Data;
using DirectoryService.Shared.Attributes;
using DirectoryService.Shared.Config;
using Npgsql;

namespace DirectoryService.DAL.Infrastructure;

[SingletonRegistration]
// ReSharper disable once ClassNeverInstantiated.Global
public class DbContext
{
    private readonly string _dbConnectionString;

    public DbContext()
    {
        _dbConnectionString = ServicesConfigContainer.Config.Db.ConnectionString;
    }

    public async Task<IDbConnection> CreateConnectionAsync()
    {
        var connection = new NpgsqlConnection(_dbConnectionString);
        await connection.OpenAsync();
        return connection;
    }
    
    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_dbConnectionString);
        connection.Open();
        return connection;
    }
}