using Microsoft.Data.Sqlite;
using Spectre.Console;
using SQLitePCL;
using System.Configuration;

namespace CodingTracker;

public static class DataBase
{
    public static void Connect()
    {
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @"DROP TABLE IF EXISTS coding_session;
                  CREATE TABLE IF NOT EXISTS coding_session (
                  Id INTEGER PRIMARY KEY AUTOINCREMENT,
                  Date TEXT,
                  StartAt TEXT,
                  EndAt TEXT
                  )";

            tableCmd.ExecuteNonQuery();
            connection.Close();

            var controller = new CodingSessionController();
            controller.Seed();
        }
    }

    public static bool IsExist(int id)
    {
        var connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        using (var connection = new SqliteConnection(connectionString))
        {

            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_session WHERE Id = {id})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
      
            connection.Close();
            return checkQuery > 0;
        }
    }
}
