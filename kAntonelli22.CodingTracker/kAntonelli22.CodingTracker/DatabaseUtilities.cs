using System.Data.SQLite;
using Dapper;
using System.Data;
using Spectre.Console;

namespace CodingTracker.DatabaseUtilities;
internal class DatabaseManager
{
    private static void DatabaseConnection(out IDbConnection? dbConnection)
    {
        try
        {
            IDbConnection connection = new SQLiteConnection(Program.ConnectionString);
            dbConnection = connection;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLineInterpolated($"[red]An error has occured: {ex.Message} while connecting to the database[/]");
            dbConnection = null;
        }
    }
    public static void GetSessions()
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = "SELECT Start, End, Duration FROM Sessions";
        List<CodingSession> sessions = connection.Query<CodingSession>(query).ToList();
        CodingSession.Sessions = sessions;
        connection.Close();
    } // end of GetSessions method

    public static void InsertSession(CodingSession session)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;
        connection.Open();

        string query = @"
        INSERT INTO Sessions (Start, End, Duration)
        VALUES (@Start, @End, @Duration)";
        connection.Execute(query, session);
        connection.Close();
    } // end of InsertSession method

    public static void RunQuery(string query)
    {
        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return;

        connection.Open();
        connection.Execute(query);
        connection.Close();
    } // end of InsertSession method

    public static int GetID(int offset)
    {
        int id = -1;
        string query = $"SELECT Id FROM Sessions LIMIT 1 OFFSET {offset}";

        DatabaseConnection(out IDbConnection? connection);
        if (connection == null)
            return id;
        connection.Open();

        id = connection.QuerySingle<int>(query);
        connection.Close();
        return id;
    } // end of GetID method
} // end of DatabaseManager Class