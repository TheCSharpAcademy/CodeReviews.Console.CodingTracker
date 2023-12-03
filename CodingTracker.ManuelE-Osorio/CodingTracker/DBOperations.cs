using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker;

class DBOperations
{
    private static readonly string connectionString = ConfigurationManager.ConnectionStrings["CodingTrackerConnectionString"].ConnectionString;
    private static readonly string? tableName = ConfigurationManager.AppSettings.Get("TableName");
    
    public static void CreateTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"CREATE TABLE IF NOT EXISTS {tableName} (
                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                StartDate STRING,
                StartTime STRING,
                EndDate STRING,
                EndTime STRING,
                SessionTime STRING)";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void DeleteTable()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"DROP TABLE IF EXISTS {tableName}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static bool IsTableEmpty()
    {
        bool isDBEmpty = true;
        if(TableExists())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT EXISTS (SELECT 1 FROM {tableName})";
            
                SqliteDataReader reader = tableCmd.ExecuteReader();

                reader.Read();
                isDBEmpty = !Convert.ToBoolean(reader.GetInt32(0));
                connection.Close();
            }
        }
        return isDBEmpty;
    }

    public static bool TableExists()
    {
        bool tableExists;
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $@"SELECT EXISTS (SELECT 1 FROM sqlite_schema WHERE type='table' AND name='{tableName}')";
            
            SqliteDataReader reader = tableCmd.ExecuteReader();

            reader.Read();
            tableExists = Convert.ToBoolean(reader.GetInt32(0));
            connection.Close();
        }
        return tableExists;
    }

    public static bool InsertValue(string[] codingSessionString)
    {
        bool insertSuccess = false;
        int rowsUpdated;

        
        if(TableExists() && !IsTableEmpty())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"INSERT INTO {tableName} (StartDate, StartTime, EndDate, EndTime, SessionTime)
                    VALUES ( '{codingSessionString[0]}', '{codingSessionString[1]}', '{codingSessionString[2]}', 
                    '{codingSessionString[3]}','{codingSessionString[4]}');";

                rowsUpdated = tableCmd.ExecuteNonQuery();
                if (rowsUpdated>0)
                {
                    insertSuccess = true;
                }

                connection.Close(); 
            }
        }
        return insertSuccess;
    }

    public static bool InsertValue(string startDate, string startTime)
    {
        bool insertSuccess = false;
        int rowsUpdated;
        if(TableExists() && !IsTableEmpty())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"INSERT INTO {tableName} (StartDate, StartTime)
                    VALUES ( '{startDate}', '{startTime}');";

                rowsUpdated = tableCmd.ExecuteNonQuery();
                if (rowsUpdated>0)
                {
                    insertSuccess = true;
                }

                connection.Close(); 
            }
        }
        return insertSuccess;
    }

    public static bool DeleteValue(string? ID)
    {
        bool deleteValueSuccess = false;
       
        if (TableExists() && !IsTableEmpty())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"DELETE FROM {tableName}
                    WHERE ID = '{ID}'";

                deleteValueSuccess = Convert.ToBoolean(tableCmd.ExecuteNonQuery());
                connection.Close();
            }
        }    
        return deleteValueSuccess;
    }

    public static bool UpdateValue(string[] codingSessionString)
    {
        bool updateValueSuccess = false; 
        if (TableExists() && !IsTableEmpty())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"UPDATE {tableName} 
                    SET
                    StartDate = '{codingSessionString[0]}',
                    StartTime = '{codingSessionString[1]}',
                    EndDate = '{codingSessionString[2]}',
                    EndTime = '{codingSessionString[3]}'
                    WHERE
                    ID = '{codingSessionString[5]}'";

                updateValueSuccess = Convert.ToBoolean(tableCmd.ExecuteNonQuery());
                connection.Close();
            }
        }
        return updateValueSuccess;
    }

    public static List<CodingSession> SelectValue()
    {
        List<CodingSession> codingSessions = new();    
     
        if(TableExists() && !IsTableEmpty())
        {        
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT ID, StartDate, StartTime, EndDate, EndTime                
                FROM {tableName};";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while(reader.Read())
                {
                    codingSessions.Add(new (
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)));
                }
                connection.Close(); 
            }
        }
        return codingSessions;
    }

    public static List<CodingSession> SelectValue(string? startDate, string? endDate)
    {
        List<CodingSession> codingSessions = new();    
    
        if(TableExists() && !IsTableEmpty())
        {        
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT ID, StartDate, StartTime, EndDate, EndTime                
                FROM {tableName}
                WHERE StartDate BETWEEN '{startDate}' AND '{endDate}';";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while(reader.Read())
                {
                    codingSessions.Add(new (
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)         
                    ));
                }
                connection.Close(); 
            }
        }
        return codingSessions;
    }

    public static List<CodingSession> SelectValue(string? startDate, string? endDate, string? operation)
    {
        List<CodingSession> codingSessions = new();
        
        string operationString = operation switch
        {
            ("1") => " ORDER BY SessionTime DESC;",
            ("2") => " ORDER BY SessionTime ASC;",
            _ => ";",
        };
        
        string filterString;
        if (startDate == null || endDate ==null)
        {
            filterString = "";
        }
        else
        {
            filterString = $"WHERE StartDate BETWEEN '{startDate}' AND '{endDate}'";
        }

        if (TableExists() && !IsTableEmpty())
        {        
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT ID, StartDate, StartTime, EndDate, EndTime                
                FROM {tableName}
                {filterString}
                {operationString}";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while(reader.Read())
                {
                    codingSessions.Add(new (
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)));
                }
                connection.Close(); 
            }
        }
        return codingSessions;
    }

    public static List<CodingSession> SelectValue(string? operation)
    {
        List<CodingSession> codingSessions = new();
        
        string operationString = operation switch
        {
            ("1") => " ORDER BY SessionTime DESC;",
            ("2") => " ORDER BY SessionTime ASC;",
            _ => ";",
        };

        if (TableExists() && !IsTableEmpty())
        {        
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT ID, StartDate, StartTime, EndDate, EndTime                
                FROM {tableName}
                {operationString};";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                while(reader.Read())
                {
                    codingSessions.Add(new (
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4)));
                }
                connection.Close(); 
            }
        }
        return codingSessions;
    }

    public static string[] GetTotalAndAverageValue(string? startDate, string? endDate)
    {
        List<TimeSpan> sessionLenghtList = new(); 
        string[] totalAndAverage = new string[2];
        
        string filterString;
        if (startDate == null || endDate ==null)
        {
            filterString = "";
        }
        else
        {
            filterString = $"WHERE StartDate BETWEEN '{startDate}' AND '{endDate}'";
        }

        if(TableExists() && !IsTableEmpty())
        {       
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT SessionTime
                    FROM {tableName}
                    {filterString}";
            
                SqliteDataReader reader = tableCmd.ExecuteReader();
                while(reader.Read())
                {
                    sessionLenghtList.Add(TimeSpan.Parse(reader.GetString(0)));         
                }
                connection.Close();
            }

            TimeSpan averageLenght = new();
            TimeSpan totalLenght = new();
            double averageSeconds = sessionLenghtList.Average(timeSpan => timeSpan.TotalSeconds);
            double totalSeconds = sessionLenghtList.Sum(timeSpan => timeSpan.TotalSeconds);
            averageLenght = TimeSpan.FromSeconds(averageSeconds);
            totalLenght = TimeSpan.FromSeconds(totalSeconds);
            totalAndAverage[0] = averageLenght.ToString(("d\\.hh\\:mm"));
            totalAndAverage[1] = totalLenght.ToString(("d\\.hh\\:mm"));     
        }
        return totalAndAverage;
    }
}