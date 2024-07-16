using Dapper;
using System.Data.SQLite;

namespace CodingTracker.Data.Managers;

/// <summary>
/// Partial class for non entity specific data manager methods.
/// </summary>
public partial class SqliteDataManager
{
    #region Constants

    private static readonly string CreateTableCodingSessionQuery =
        @"
        CREATE TABLE IF NOT EXISTS CodingSession
        (
             Id INTEGER PRIMARY KEY AUTOINCREMENT
            ,StartTime TEXT NOT NULL
            ,EndTime TEXT NOT NULL
            ,Duration REAL NOT NULL
        )
        ;";

    private static readonly string CreateTableCodingGoalQuery =
        @"
        CREATE TABLE IF NOT EXISTS CodingGoal
        (
             Id INTEGER PRIMARY KEY AUTOINCREMENT
            ,WeeklyDurationInHours REAL NOT NULL
        )
        ;";

    #endregion
    #region Properties

    public string ConnectionString { get; init; }

    #endregion
    #region Constructors

    public SqliteDataManager(string connectionString)
    {
        ConnectionString = connectionString;

        Initialise();
    }

    #endregion
    #region Methods

    private void CreateTableCodingSession()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        connection.Execute(CreateTableCodingSessionQuery);
    }

    private void CreateTableCodingGoal()
    {
        using var connection = new SQLiteConnection(ConnectionString);
        connection.Open();
        connection.Execute(CreateTableCodingGoalQuery);
    }

    private void Initialise()
    {
        // Put all table creation methods here, in dependency order.
        CreateTableCodingSession();
        CreateTableCodingGoal();
        AddCodingGoal();
    }

    #endregion
}
