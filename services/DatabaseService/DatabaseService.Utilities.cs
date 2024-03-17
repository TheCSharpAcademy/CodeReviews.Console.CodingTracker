using System.Text;
using CodingTracker.enums;
using CodingTracker.models;

namespace CodingTracker.services;

/// <summary>
/// Provides methods for interacting with a database and performing CRUD (Create, Read,
/// Update, Delete) operations on coding sessions.
/// </summary>
internal partial class DatabaseService
{
    /// <summary>
    /// Chooses the appropriate SQL query based on the provided action.
    /// </summary>
    /// <param name="action">The action to perform in the database (Insert, Update, Delete).</param>
    /// <returns>The SQL query as a string.</returns>
    private string ChooseUpdateQuery(DatabaseUpdateActions action) => action switch
    {
        DatabaseUpdateActions.Insert => "INSERT INTO records (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)",
        DatabaseUpdateActions.Update => "UPDATE records SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id",
        DatabaseUpdateActions.Delete => "DELETE FROM records WHERE Id = @Id",
        _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
    };

    /// <summary>
    /// Retrieves the SQL query for retrieving coding sessions from the database based on the specified start and end dates.
    /// </summary>
    /// <param name="start">The start date of the coding sessions to retrieve. Can be null to retrieve all sessions before the end date.</param>
    /// <param name="end">The end date of the coding sessions to retrieve. Can be null to retrieve all sessions after the start date.</param>
    /// <returns>
    /// A string representing the SQL query for retrieving coding sessions.
    /// </returns>
    private string CodingSessionsRetrieveQuery(DateTime? start, DateTime? end)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append("SELECT * FROM records");
        
        if (start is null && end is null)
        {
            return stringBuilder.ToString();
        }

        stringBuilder
            .Append(" WHERE ")
            .Append(start is not null ? "StartTime >= @Start " : "");

        stringBuilder
            .Append(start is not null && end is not null ? "AND EndTime <= @End" : "EndTime <= @End");

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Chooses the update query based on the given action.
    /// </summary>
    /// <param name="action">The action to perform in the database (Insert, Update, Delete).</param>
    /// <param name="session">Session with changes</param>
    /// <returns>The update query as a string.</returns>
    private object ChooseUpdateParameters(DatabaseUpdateActions action, CodingSession session) => action switch
    {
        DatabaseUpdateActions.Insert => new { session.StartTime, session.EndTime, session.Duration },
        DatabaseUpdateActions.Update => new { session.StartTime, session.EndTime, session.Duration, session.Id },
        _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
    };

    /// <summary>
    /// Chooses the appropriate update parameters based on the action and session.
    /// </summary>
    /// <param name="action">The action to perform in the database (Insert, Update, Delete).</param>
    /// <param name="id">Record ID to be used as a parameter.</param>
    /// <returns>The update parameters for the given action.</returns>
    private object ChooseUpdateParameters(DatabaseUpdateActions action, int id)
    {
        return new { Id = action == DatabaseUpdateActions.Delete ? id : -1 };
    }
}