using Microsoft.Data.Sqlite;
using CodingTracker.Models;
using System.Globalization;
using static CodingTracker.Helpers;

namespace CodingTracker;

public static class DataAccess
{
    private static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

    public static void InitializeDatabase()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand tableCommand = connection.CreateCommand();

            tableCommand.CommandText =
                $@"CREATE TABLE IF NOT EXISTS CodingSessions (Id INTEGER PRIMARY KEY AUTOINCREMENT, Date TEXT, TimeSpentCoding TEXT)";

            tableCommand.ExecuteNonQuery();

            connection.Close();
        }
    }

    public static string InsertSession()
    {
        string operationComplete = "0";

        string date = InputSessionDate();

        if (date == "0") return operationComplete;

        string timeSpentCoding = InputSessionTime();

        if (timeSpentCoding == "0") return timeSpentCoding;

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand tableCommand = connection.CreateCommand();

            tableCommand.CommandText =
                $@"INSERT INTO CodingSessions(date, TimeSpentCoding) VALUES('{date}', '{timeSpentCoding}')";

            tableCommand.ExecuteNonQuery();

            operationComplete = "1";

            connection.Close();
        }

        return operationComplete;
    }

    public static List<CodingSessions> GetSessionsHistory()
    {
        // Send back a list of CodingSessions
        List<CodingSessions> codingSessions = new List<CodingSessions>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand tableCommand = connection.CreateCommand();

            tableCommand.CommandText = "SELECT * FROM CodingSessions";

            SqliteDataReader reader = tableCommand.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    codingSessions.Add(new CodingSessions
                    {
                        Id = reader.GetInt32(0),
                        ShortDate = (DateTime.ParseExact(reader.GetString(1), "d-M-yy", new CultureInfo("en-US"))).ToShortDateString(),
                        TimeSpentCoding = reader.GetString(2)
                    }); 
                }
            }
            else
            {
                Console.WriteLine("No session found.");
            }
            connection.Close();
        }

        return codingSessions;
    }

    public static int DeleteSession(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            SqliteCommand tableCommand = connection.CreateCommand();

            tableCommand.CommandText =
                $@"DELETE FROM CodingSessions WHERE Id = {id}";

            int rowCount = tableCommand.ExecuteNonQuery();

            connection.Close();

            return rowCount;
        }
    }

    public static int UpdateSession(int id)
    {
        int checkQuery = 0;
        if (id != 0)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCommand = connection.CreateCommand();
                checkCommand.CommandText = $"SELECT EXISTS(SELECT 1 FROM CodingSessions WHERE Id = {id})";

                SqliteCommand tableCommand = connection.CreateCommand();
                checkQuery = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (checkQuery == 0) return checkQuery;

                string date = InputSessionDate();
                if (date == "0") return 2;
                string timeSpentCoding = InputSessionTime();
                if (timeSpentCoding == "0") return 2;

                tableCommand.CommandText =
                    $@"UPDATE CodingSessions SET Date = '{date}', TimeSpentCoding = '{timeSpentCoding}' WHERE Id = {id}";

                tableCommand.ExecuteNonQuery();

                connection.Close();
            }
        }
        return checkQuery;
    }
}

