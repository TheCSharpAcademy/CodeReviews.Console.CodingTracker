using CodingTracker.enums;
using CodingTracker.models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.services;

/// <summary>
/// This class provides database services for handling CRUD operations related to coding sessions.
/// </summary>
internal partial class DatabaseService
{
    /// <summary>
    /// Retrieves coding sessions from the database based on the specified start and end dates.
    /// </summary>
    /// <param name="start">The start date of the coding sessions to retrieve. Can be null to retrieve all sessions before the end date.</param>
    /// <param name="end">The end date of the coding sessions to retrieve. Can be null to retrieve all sessions after the start date.</param>
    /// <returns>
    /// An IEnumerable of CodingSession objects representing the retrieved coding sessions.
    /// Returns null if an error occurs while retrieving the sessions.
    /// </returns>
    internal IEnumerable<CodingSession>? RetrieveCodingSessions(DateTime? start, DateTime? end)
    {
        var query = CodingSessionsRetrieveQuery(start, end);
        
        try
        {
            var connection = GetConnection();

            return connection.Query<CodingSession>(query, param: new { Start = start, End = end }).ToList();
        }
        catch (SqliteException e)
        {
            AnsiConsole.WriteLine("Error getting coding sessions." + e.Message);
            return null;
        }
    }

    /// <summary>
    /// Updates data in the database based on the given action and parameters.
    /// </summary>
    /// <param name="action">The action to perform in the database (Insert, Update, Delete).</param>
    /// <param name="session">Optional coding session to be used as a parameter.</param>
    /// <param name="recordId">Optional record ID to be used as a parameter.</param>
    /// <returns>The number of rows affected in the database.</returns>
    internal int UpdateData(DatabaseUpdateActions action, CodingSession? session = null, int recordId = 0)
    {
        using var connection = GetConnection();
        
        var query = ChooseUpdateQuery(action);
        
        var parameters = session is null 
            ? ChooseUpdateParameters(action, recordId) 
            : ChooseUpdateParameters(action, session);
        
        return connection.Execute(query, parameters);
    }

    /// <summary>
    /// Retrieves the last record from the database.
    /// </summary>
    /// <returns>The last <see cref="CodingSession"/> record from the database.</returns>
    internal CodingSession? GetLastRecord()
    {
        using var connection = GetConnection();
        
        return connection.QueryFirstOrDefault<CodingSession>("SELECT * FROM records ORDER BY Id DESC LIMIT 1");
    }
}