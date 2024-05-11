using System.Data.SQLite;
using CodingTracker.kalsson.Models;
using Dapper;

namespace CodingTracker.kalsson.Data;

public class CodingSessionRepository
{
    private readonly string _connectionString;

    /// <summary>
    /// Represents a repository for handling coding session data.
    /// </summary>
    public CodingSessionRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Inserts a coding session into the database.
    /// </summary>
    /// <param name="session">The coding session to be inserted.</param>
    public void InsertCodingSession(CodingSession session)
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var sql = @"
            INSERT INTO CodingSessions (StartTime, EndTime)
            VALUES (@StartTime, @EndTime);";
                connection.Open();
                connection.Execute(sql, session);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to insert coding session. Error: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves all coding sessions from the database.
    /// </summary>
    /// <returns>
    /// A collection of all coding sessions stored in the database as instances of <see cref="CodingSession"/>.
    /// </returns>
    public IEnumerable<CodingSession> GetAllCodingSessions()
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                var result = connection.Query<CodingSession>("SELECT * FROM CodingSessions;");
                Console.WriteLine($"Retrieved {result.Count()} sessions."); // Debug output
                return result;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to retrieve coding sessions. Error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Updates a coding session in the database.
    /// </summary>
    /// <param name="session">The coding session to be updated.</param>
    public void UpdateCodingSession(CodingSession session)
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var sql = @"
                UPDATE CodingSessions
                SET StartTime = @StartTime, EndTime = @EndTime
                WHERE Id = @Id;";
            
                connection.Open();
                connection.Execute(sql, session);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to update coding session. Error: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Deletes a coding session from the database.
    /// </summary>
    /// <param name="sessionId">The id of the coding session to be deleted.</param>
    public void DeleteCodingSession(int sessionId)
    {
        try
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                var sql = @"
                DELETE FROM CodingSessions
                WHERE Id = @Id;";
            
                connection.Open();
                connection.Execute(sql, new { Id = sessionId });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to delete coding session. Error: {ex.Message}");
            throw;
        }
    }
}