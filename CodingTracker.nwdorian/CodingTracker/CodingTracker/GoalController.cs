using CodingTracker.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

namespace CodingTracker;
internal class GoalController
{
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    private readonly string? _connectionString;

    public GoalController()
    {
        _connectionString = configuration.GetConnectionString("Default");
    }
    internal void Post(Goal goal)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var insert = "INSERT INTO goals (StartDate, EndDate, Amount) VALUES (@StartDate, @EndDate, @Amount)";

                connection.Execute(insert, goal);
            }
            AnsiConsole.Write("New goal was succesfully added! Press any key to continue... ");
            Console.ReadKey();
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
    }

    internal List<Goal>? GetAll()
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();

                var getAll = "SELECT * FROM goals";

                return connection.Query<Goal>(getAll).ToList();
            }
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
        return null;
    }

    internal Goal? GetById(int id)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var get = $"SELECT * FROM goals WHERE Id = @Id";
                return connection.QuerySingleOrDefault<Goal>(get, new { Id = id });
            }
        }
        catch (Exception e)
        {
            AnsiConsole.Write($"\nError! Details: {e.Message}\nPress any key to continue... ");
            Console.ReadKey();
        }
        return null;
    }
}
