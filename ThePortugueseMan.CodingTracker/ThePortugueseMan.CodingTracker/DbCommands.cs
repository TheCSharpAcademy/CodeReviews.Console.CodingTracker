using System;
using System.Globalization;
using Microsoft.Data.Sqlite;

namespace ThePortugueseMan.CodingTracker;

public class DbCommands
{
    string? connectionString, mainTableName, goalsTableName, dateTimeFormat, timeSpanFormat;
    AppSettings appSettings = new();

    public DbCommands() 
    {
        this.connectionString = appSettings.GetConnectionString();
        this.mainTableName = appSettings.GetMainTableName();
        this.goalsTableName= appSettings.GetGoalsTableName();
        this.dateTimeFormat = appSettings.GetDateTimeFormatOfDB();
        this.timeSpanFormat = appSettings.GetTimeSpanFormatOfDB();
    }

    public void InitializeMainTable()
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

    public void InitializeGoalsTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS {this.goalsTableName}" +
                    "(Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                    "StartDate STRING, EndDate STRING, TargetHours STRING, HoursSpent STRING, Status STRING)";

            tableCmd.ExecuteNonQuery();

            connection.Close();
        }
    }

    public bool Insert(CodingSession sessionToInsert)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO {this.mainTableName}(StartDate, EndDate, Diff) " +
                $"VALUES ('{sessionToInsert.StartDateTime.ToString(dateTimeFormat)}'," +
                $"'{sessionToInsert.EndDateTime.ToString(dateTimeFormat)}'," +
                $"'{sessionToInsert.Duration.ToString(timeSpanFormat)}')";

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

    public bool Insert(Goal goalToInsert)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"INSERT INTO {this.goalsTableName}(StartDate, EndDate, TargetHours, HoursSpent, Status) " +
                $"VALUES ('{goalToInsert.StartDate.ToString("dd-MM-yy")}'," +
                $"'{goalToInsert.EndDate.ToString("dd-MM-yy")}'," +
                $"'{goalToInsert.TargetHours.ToString(timeSpanFormat)}'," +
                $"'{goalToInsert.HoursSpent.ToString(timeSpanFormat)}'," +
                $"'Active')";

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

    public bool CheckIfThereIsActiveGoal()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {this.goalsTableName} WHERE Status = 'Active')";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public bool CheckIfIndexExistsInTable(int index)
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

    public bool DeleteByIndex(int index)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"DELETE from {this.mainTableName} WHERE Id = '{index}'";

            int rowCount = tableCmd.ExecuteNonQuery();
            connection.Close();
            if (rowCount == 0) return false;
            else return true;

        }
    }

    public bool Update(int index, CodingSession newSessionInfo)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {this.mainTableName} WHERE Id = {index})";
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
                    $"UPDATE {this.mainTableName} SET " +
                    $"StartDate = '{newSessionInfo.StartDateTime.ToString(dateTimeFormat)}', " +
                    $"EndDate = '{newSessionInfo.EndDateTime.ToString(dateTimeFormat)}'," +
                    $"Diff = '{newSessionInfo.Duration.ToString(timeSpanFormat)}' " +
                    $"WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                return true;
            }
        }
    }

    public bool Update(int index, Goal newGoalInfo)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {this.goalsTableName} WHERE Id = {index})";
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
                    $"UPDATE {this.goalsTableName} SET " +
                    $"StartDate = '{newGoalInfo.StartDate.ToString("dd-MM-yy")}', " +
                    $"EndDate = '{newGoalInfo.EndDate.ToString("dd-MM-yy")}'," +
                    $"HoursSpent = '{newGoalInfo.HoursSpent.ToString(timeSpanFormat)}'," +
                    $"Status = '{newGoalInfo.Status}'" +
                    $"WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();

                connection.Close();

                return true;
            }
        }
    }

    public List<CodingSession> ReturnAllLogsInTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {this.mainTableName}";

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
                }
            }
            else { return null; }

            connection.Close();

            return tableData;
        }
    }

    public List<Goal> ReturnAllGoalsInTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {this.goalsTableName}";

            var tableData = new List<Goal>();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(
                    new Goal
                    {
                        Id = reader.GetInt32(0),
                        StartDate = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        EndDate = DateTime.ParseExact(reader.GetString(2), "dd-MM-yy", new CultureInfo("en-US")),
                        TargetHours = TimeSpan.ParseExact(reader.GetString(3), timeSpanFormat, new CultureInfo("en-US")),
                        HoursSpent = TimeSpan.ParseExact(reader.GetString(4), timeSpanFormat, new CultureInfo("en-US")),
                        Status = reader.GetString(5)
                    });
                }
            }
            else { return null; }

            connection.Close();

            return tableData;
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

    public CodingSession ReturnSessionByIndex(int index)
    {
        CodingSession returnSession = new();
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {this.mainTableName} WHERE Id = {index}";

            SqliteDataReader reader = tableCmd.ExecuteReader();

            while (reader.Read())
            {
                returnSession = new CodingSession
                {
                    Id = reader.GetInt32(0),
                    StartDateTime = DateTime.ParseExact(reader.GetString(1), dateTimeFormat, new CultureInfo("en-US")),
                    EndDateTime = DateTime.ParseExact(reader.GetString(2), dateTimeFormat, new CultureInfo("en-US")),
                    Duration = TimeSpan.ParseExact(reader.GetString(3), timeSpanFormat, new CultureInfo("en-US"))
                };
            }

            connection.Close();
            return returnSession;
        }
    }
}
