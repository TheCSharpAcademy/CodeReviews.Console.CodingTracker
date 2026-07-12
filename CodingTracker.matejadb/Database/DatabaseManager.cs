using CodingTracker.matejadb.Config;
using CodingTracker.matejadb.Models;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.matejadb.Database;

internal class DatabaseManager {

    static SqliteConnection OpenConnection() {
        var connection = new SqliteConnection(AppSettings.ConnectionString);
        connection.Open();
        return connection;
    }

    internal void Init() {
        var sql = "CREATE TABLE IF NOT EXISTS coding_tracker ( " +
           "Id INTEGER PRIMARY KEY AUTOINCREMENT, " +
           "StartTime TEXT NOT NULL, " +
           "EndTime TEXT NOT NULL, " +
           "Duration TEXT NOT NULL)";
        var connection = OpenConnection();

        connection.Execute(sql);
        connection.Close();
    }

    internal void AddSession(string startTime, string endTime, string duration) {
        var sql = @"INSERT INTO coding_tracker(StartTime, EndTime, Duration) VALUES (@startTime, @endTime, @duration)";
        var connection = OpenConnection();

        connection.Execute(sql, new { startTime, endTime, duration });
        connection.Close();
    }

    internal void UpdateSession(int id, string startTime, string endTime, string duration) {
        var sql = @"UPDATE coding_tracker SET StartTime = @startTime, EndTime = @endTime, Duration = @duration WHERE Id = @id";
        var connection = OpenConnection();

        connection.Execute(sql, new { id, startTime, endTime, duration });
        connection.Close();
    }

    internal void DeleteSession(int id) {
        var sql = @"DELETE FROM coding_tracker WHERE Id = @id";
        var connection = OpenConnection();

        connection.Execute(sql, new { id });
        connection.Close();
    }

    internal List<CodingSession> GetAllSessions() {
        var sql = @"SELECT Id, StartTime, EndTime, Duration FROM coding_tracker";
        var connection = OpenConnection();

        var sessions = connection.Query<CodingSession>(sql).ToList();

        return sessions;
    }
}
