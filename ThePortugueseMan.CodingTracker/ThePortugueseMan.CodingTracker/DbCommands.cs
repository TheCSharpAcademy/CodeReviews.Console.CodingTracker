using System;
using System.Globalization;
using Microsoft.Data.Sqlite;
using System.Text;
using ConsoleTableExt;

namespace ThePortugueseMan.CodingTracker;

public class DbCommands
{
    string? connectionString, mainTableName, dateTimeFormat, timeSpanFormat;
    AppSettings appSettings = new();
    public DbCommands() 
    {
        this.connectionString = appSettings.GetConnectionString();
        this.mainTableName = appSettings.GetMainTableName();
        this.dateTimeFormat = appSettings.GetDateTimeFormatOfDB();
        this.timeSpanFormat = appSettings.GetTimeSpanFormatOfDB();
    }

    //if the main table doesn't exist, it's created
    public void Initialization()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS {this.mainTableName}" +
                    "(Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "StartDate STRING, EndDate STRING, Diff STRING)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }
    public bool Insert(ref CodingSession sessionToInsert)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO {this.mainTableName}(StartDate, EndDate, Diff) " +
                $"VALUES ('{sessionToInsert.StartDateTime.ToString(dateTimeFormat)}'," +
                $"'{sessionToInsert.EndDateTime.ToString(dateTimeFormat)}'," +
                $"'{sessionToInsert.Duration.ToString("h\\:mm")}')";

            try 
            {
                tableCmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch
            {
                connection.Close();
                return false;
            }
            
        }
    }
    //checks if there is an entry at Id = index
    public bool CheckIfIndexExists(int index)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {this.mainTableName} WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }
    public bool DeleteByIndex(int index, string? tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE from {tableName} WHERE Id = '{index}'";

            int rowCount = tableCmd.ExecuteNonQuery();
            connection.Close();
            if (rowCount == 0) return false;
            else return true;

        }
    }
    public bool DeleteTable(string? tableName)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DROP TABLE {tableName}";

            if (tableCmd.ExecuteNonQuery() == 0)
            {
                connection.Close();
                return false;
            }
            else
            {
                connection.Close();
                return true;
            }
        }
    }
    //Updates entry on subTable by index - overload based on datatypes
    public bool Update(string tableName, int index, string date, int quantity)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE {tableName} SET date = '{date}', quantity = {quantity} WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                return true;
            }
        }
    }
    public List<CodingSession> ReturnAll()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {this.mainTableName}";

            var tableDisplayData = new List<List<object>>();
            var tableData = new List<CodingSession>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartDateTime = DateTime.ParseExact(reader.GetString(1),dateTimeFormat, new CultureInfo("en-US")),
                        EndDateTime = DateTime.ParseExact(reader.GetString(2), dateTimeFormat, new CultureInfo("en-US")),
                        Duration = TimeSpan.ParseExact(reader.GetString(3), timeSpanFormat, new CultureInfo("en-US"))
                    });

                    tableDisplayData.Add(
                        new List<object>
                        { reader.GetInt32(0), 
                            reader.GetString(1), reader.GetString(2), reader.GetString(3) });
                }
            }
            else { return null; }

            connection.Close();

            return tableData;
        }
    }
}
