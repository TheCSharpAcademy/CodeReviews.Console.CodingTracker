using System.Collections;
using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Collections.Specialized;

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

    public static bool InsertValue(string startDate, string startTime, string endDate, string endTime, string sessionTime)
    {
        bool insertSuccess = false;
        int rowsUpdated;
        if(TableExists())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"INSERT INTO {tableName} (StartDate, EndDate, StartTime, EndTime, SessionTime)
                    VALUES ( '{startDate}', '{endDate}', '{startTime}', '{endTime}','{sessionTime}');";

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
        if(TableExists())
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

    public static bool DeleteValue(string connectionString, string tableName, int ID)
    {
        bool deleteValueSuccess = false;
       
        if (TableExists())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"DELETE FROM {tableName}
                    WHERE ID = {ID}";

                deleteValueSuccess = Convert.ToBoolean(tableCmd.ExecuteNonQuery());
                connection.Close();
            }
        }    
        return deleteValueSuccess;
    }

    public static bool UpdateValue(int ID, string startDate, string startTime, string endDate, string endTime)
    {
        bool updateValueSuccess = false; 
        if (TableExists())
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"UPDATE {tableName} 
                    SET
                    StartDate = '{startDate}',
                    StartTime = '{startTime}',
                    EndDate = '{endDate}',
                    EndTime = '{endTime}'
                    WHERE
                    ID = {ID}";

                updateValueSuccess = Convert.ToBoolean(tableCmd.ExecuteNonQuery());
                connection.Close();
            }
        }
        return updateValueSuccess;
    }

    public static bool SelectValue(List<CodingSession> codingSessions)
    {
        bool selectValueSuccess = false;
    
        if(TableExists())
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
                        reader.GetString(4)         
                    ));
                }
                connection.Close(); 
            }
        }


        return selectValueSuccess;
    }

    public static bool SelectValue(List<CodingSession> codingSessions,string startDate, string endDate)
    {
        bool selectValueSuccess = false;
    
        if(TableExists())
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


        return selectValueSuccess;
    }

    public static bool SelectValue(List<CodingSession> codingSessions,string startDate, string endDate,int operation)
    {
        bool selectValueSuccess = false;
        string operationString = "ASC";


        switch(operation)
        {
            case(1):
                operationString = ";";
                break;
            case(2):
                operationString = " ORDER BY SessionTime DESC;";
                break;
            case(3):
                operationString = " ORDER BY SessionTime ASC;";
                break;        
        }

        if(TableExists())
        {        
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"SELECT ID, StartDate, StartTime, EndDate, EndTime                
                FROM {tableName}
                WHERE StartDate BETWEEN '{startDate}' AND '{endDate}'
                {operationString};";

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


        return selectValueSuccess;
    }

}