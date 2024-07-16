using CodingTracker.Data.Entities;
using CodingTracker.Data.Extensions;
using Dapper;
using System.Data.SQLite;

namespace CodingTracker.Data.Managers;

/// <summary>
/// Partial class for CodingSession entity specific data manager methods.
/// </summary>
public partial class SqliteDataManager
{
    #region Constants

    private static readonly string AddCodingSessionQuery =
        @"
        INSERT INTO CodingSession
        (
             StartTime
            ,EndTime
            ,Duration
        )
        VALUES
        (
             $StartTime
            ,$EndTime
            ,$Duration
        )
        ;";

    private static readonly string DeleteCodingSessionQuery =
        @"
        DELETE FROM
            CodingSession
        WHERE
            Id = $Id
        ;";

    private static readonly string GetCodingSessionQuery =
        @"
        SELECT
            *
        FROM
            CodingSession
        WHERE
            Id = $Id
        ;";

    private static readonly string GetCodingSessionsQuery =
        @"
        SELECT
            *
        FROM
            CodingSession
        ;";

    private static readonly string SetCodingSessionQuery =
        @"
        UPDATE
            CodingSession
        SET
                StartTime = $StartTime
            ,EndTime = $EndTime
            ,Duration = $Duration
        WHERE
            Id = $Id
        ;";

    #endregion
    #region Methods

    public void AddCodingSession(DateTime start, DateTime end, double duration)
    {
        var parameters = new
        { 
            StartTime = start.ToSqliteDateTimeString(),
            EndTime = end.ToSqliteDateTimeString(),
            Duration = duration
        };

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        connection.Execute(AddCodingSessionQuery, parameters);
    }

    public void DeleteCodingSession(int id)
    {
        var parameters = new
        {
            Id = id
        };

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        connection.Execute(DeleteCodingSessionQuery, parameters);
    }

    public CodingSessionEntity GetCodingSession(int id)
    {
        var parameters = new
        {
            Id = id
        };

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        return connection.QuerySingle<CodingSessionEntity>(GetCodingSessionQuery, parameters);
    }

    public IReadOnlyList<CodingSessionEntity> GetCodingSessions()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        return connection.Query<CodingSessionEntity>(GetCodingSessionsQuery).ToList();
    }

    public void SetCodingSession(int id, DateTime start, DateTime end, double duration)
    {
        var parameters = new
        {
            Id = id,
            StartTime = start.ToSqliteDateTimeString(),
            EndTime = end.ToSqliteDateTimeString(),
            Duration = duration
        };

        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        connection.Execute(SetCodingSessionQuery, parameters);
    }

    #endregion
}
