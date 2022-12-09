using System.Data;
using Dapper;
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

    public void RunScript(string filename)
    {
        using var con = CreateConnection();
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var baseDir = Path.Combine(Path.GetDirectoryName(assembly.Location)!, "Scripts/");
        if (!File.Exists(baseDir + "/" + filename))
            throw new FileNotFoundException("File not found", baseDir + "/" + filename);
        var script = File.ReadAllText(baseDir + "/" + filename);
        con.Execute(script);
    }
}