using Dapper;
using Microsoft.Extensions.Logging;

namespace DirectoryService.DAL.Infrastructure;

public static class DatabaseMigrator
{
    /// <summary>
    /// Database Migrator
    /// </summary>
    public static bool MigrateDatabase(ILogger logger)
    {
        using var con = new DbContext().CreateConnection();

        logger.LogInformation("Beginning Migrations");
        
        // Create migrations table if it does not yet exist
        con.Execute(@"CREATE TABLE IF NOT EXISTS migrations (
                            date_added TIMESTAMP NOT NULL, 
                            filename VARCHAR(128) NOT NULL,
                            CONSTRAINT migrations_filename_pk
                            PRIMARY KEY (filename));");

        var appliedMigrations =
            con.Query<string>("SELECT filename FROM migrations ORDER BY date_added;").ToList();

        var migrationDirectory = AppDomain.CurrentDomain.BaseDirectory + "Migrations/";
        var files = Directory.GetFiles(migrationDirectory, "*.sql").ToList();

        files.Sort();
        foreach (var filename in files)
        {
            var file = filename.Replace(migrationDirectory, "");
            if (appliedMigrations.Contains(file))
            {
                logger.LogInformation("Already applied: {file}", file);
                continue;
            }

            logger.LogInformation("Applying {file}...", file);
            try
            {
                var sql = File.ReadAllText(filename);
                con.Execute(sql);

                con.Execute(
                    "INSERT INTO migrations (date_added, filename) SELECT CURRENT_TIMESTAMP, @pFilename ;",
                    new
                    {
                        pFilename = file
                    });
            }
            catch (Exception e)
            {
                logger.LogCritical("Error in migration: {file}", file);
                logger.LogCritical("{error}", e.Message);
                return false;
            }
        }
        return true;
    }
}