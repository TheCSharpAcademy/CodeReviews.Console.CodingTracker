using Microsoft.Data.Sqlite;

namespace ThePortugueseMan.CodingTracker;

public class DbCommands
{
    string? connectionString, mainTableName, goalsTableName, dateTimeFormat, timeSpanFormat;
    AppSettings appSettings = new();
    Format format = new();

    public DbCommands() 
    {
        connectionString = appSettings.GetConnectionString();
        mainTableName = appSettings.GetMainTableName();
        goalsTableName= appSettings.GetGoalsTableName();
        dateTimeFormat = appSettings.GetDateTimeDisplayFormat();
        timeSpanFormat = appSettings.GetTimeSpanFormatOfDB();
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
                    "StartDate STRING, EndDate STRING, TargetHours STRING, HoursSpent STRING)";

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
                $"VALUES ('{format.DateToMainDbString(sessionToInsert.StartDateTime)}'," +
                $"'{format.DateToMainDbString(sessionToInsert.EndDateTime)}'," +
                $"'{format.TimeSpanToString(sessionToInsert.Duration)}')";
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
                $"INSERT INTO {this.goalsTableName}(StartDate, EndDate, TargetHours, HoursSpent) " +
                $"VALUES ('{format.DateToGoalsDbString(goalToInsert.StartDate)}'," +
                $"'{format.DateToGoalsDbString(goalToInsert.EndDate)}'," +
                $"'{format.TimeSpanToString(goalToInsert.TargetHours)}'," +
                $"'{format.TimeSpanToString(goalToInsert.HoursSpent)}')";

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

    public bool CheckIfIndexExistsInTable(int index, string table)
    {
        string tableName;
        if (table == "Main") tableName = this.mainTableName;
        else if (table == "Goals") tableName = this.goalsTableName;
        else throw new Exception("Wrong table name");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();

            checkCmd.CommandText =
                $"SELECT EXISTS(SELECT 1 FROM {tableName} WHERE Id = {index})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());
            connection.Close();

            if (checkQuery == 0) return false;
            else return true;
        }
    }

    public bool DeleteByIndex(int index, string table)
    {
        string tableName;
        if (table == "Main") tableName = this.mainTableName;
        else if (table == "Goals") tableName = this.goalsTableName;
        else throw new Exception("Wrong table name");

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
                    $"StartDate = '{format.DateToMainDbString(newSessionInfo.StartDateTime)}', " +
                    $"EndDate = '{format.DateToMainDbString(newSessionInfo.EndDateTime)}'," +
                    $"Diff = '{format.TimeSpanToString(newSessionInfo.Duration)}' " +
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
                    $"StartDate = '{format.DateToGoalsDbString(newGoalInfo.StartDate)}', " +
                    $"EndDate = '{format.DateToGoalsDbString(newGoalInfo.EndDate)}'," +
                    $"HoursSpent = '{format.TimeSpanToString(newGoalInfo.HoursSpent)}'" +
                    $"WHERE Id = {index}";

                tableCmd.ExecuteNonQuery();
                connection.Close();
                return true;
            }
        }
    }

    public List<CodingSession> GetAllLogsInTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {this.mainTableName}";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                var tableData = new List<CodingSession>();
                while (reader.Read())
                {                    
                    tableData.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        StartDateTime = format.StringToDate(reader.GetString(1)),
                        EndDateTime = format.StringToDate(reader.GetString(2)),
                        Duration = format.StringToTimeSpan(reader.GetString(3)),
                    });
                }
                connection.Close();
                return tableData;
            }
            else 
            {
                connection.Close();
                return null; 
            }
        }
    }

    public Goal GetGoalInTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
                $"SELECT * FROM {this.goalsTableName}";

            SqliteDataReader reader = tableCmd.ExecuteReader();
            if (reader.HasRows)
            {
                var currGoal = new Goal();
                while (reader.Read())
                {
                    currGoal =
                    new Goal
                    {
                        Id = reader.GetInt32(0),
                        StartDate = format.StringToDate(reader.GetString(1)),
                        EndDate = format.StringToDate(reader.GetString(2)),
                        TargetHours = format.StringToTimeSpan(reader.GetString(3)),
                        HoursSpent = format.StringToTimeSpan(reader.GetString(4)),
                    };
                }

                connection.Close();
                return currGoal;
            }
            else
            {
                connection.Close();
                return null;
            }
        }
    }

    public bool DeleteGoalsTableContents()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"DELETE FROM {this.goalsTableName}";

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
}
