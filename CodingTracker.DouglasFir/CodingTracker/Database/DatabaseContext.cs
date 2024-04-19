using System.Data.SQLite;
using CodingTracker.Services;

namespace CodingTracker.Database;

public class DatabaseContext
{
    private readonly string _dbConnectionString;

    public DatabaseContext()
    {
        _dbConnectionString = ConfigSettings.DbConnectionString;
    }

    public SQLiteConnection GetNewDatabaseConnection()
    {
        var connection = new SQLiteConnection(_dbConnectionString);
        try
        {
            connection.Open();
            return connection;
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error opening database connection.", ex.Message);
            connection.Dispose();
            throw;
        }
    }
}
