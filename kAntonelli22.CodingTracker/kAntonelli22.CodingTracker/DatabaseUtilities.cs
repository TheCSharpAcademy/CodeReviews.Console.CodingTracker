﻿using System.Data.SQLite;
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
            IDbConnection connection = new SQLiteConnection(Program.connectionString);
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
        IDbConnection? connection;
        DatabaseConnection(out connection);
        if (connection == null)
            return;
        connection.Open();

        string query = "SELECT start, end, duration FROM Sessions";
        List<CodingSession> sessions = connection.Query<CodingSession>(query).ToList(); // extra step here for project requirments
        CodingSession.sessions = sessions;
        connection.Close();
    } // end of GetSessions method

    public static void InsertSession(CodingSession session)
    {
        IDbConnection? connection;
        DatabaseConnection(out connection);
        if (connection == null)
            return;
        connection.Open();

        string query = @"
        INSERT INTO Sessions (start, end, duration)
        VALUES (@start, @end, @duration)";
        connection.Execute(query, session);
        connection.Close();
    } // end of InsertSession method

    public static void RunQuery(string query)
    {
        IDbConnection? connection;
        DatabaseConnection(out connection);
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
        IDbConnection? connection;
        DatabaseConnection(out connection);
        if (connection == null)
            return id;
        connection.Open();

        id = connection.QuerySingle<int>(query);
        Console.WriteLine("id: " + id);
        connection.Close();
        return id;
    } // end of GetID method
} // end of DatabaseManager Class