using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace CodingTracker;

internal class DatabaseManager
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();
    private readonly string? _connectionString;
    public DatabaseManager()
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    internal void CreateTable()
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var createTable =
                    @"CREATE TABLE IF NOT EXISTS coding(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT
                        )";

                connection.Execute(createTable);

                createTable =
                    @"CREATE TABLE IF NOT EXISTS goals(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDate TEXT,
                        EndDate TEXT,
                        Amount INT
                        )";

                connection.Execute(createTable);
            }
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }

    internal void SeedDatabase()
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var checkTable = "SELECT * FROM Coding";

                var count = connection.QueryFirstOrDefault<Coding>(checkTable);

                if (count == default)
                {
                    var codingData = DataGenerator.GenerateCodingData();

                    var bulkInsert = "INSERT INTO Coding (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";

                    connection.Execute(bulkInsert, codingData);
                }
            }
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }
}
