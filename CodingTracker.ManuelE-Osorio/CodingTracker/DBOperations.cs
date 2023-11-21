using Microsoft.Data.Sqlite;

class DBOperations
{
    public static void CreateTable(string connectionString, string tableName)
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
                EndTime STRING )";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }

    public static void DeleteTable(string connectionString, string tableName)
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

    public static bool IsTableEmpty(string connectionString, string tableName)
    {
        bool isDBEmpty = true;
        if(TableExists(connectionString,tableName))
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

    public static bool TableExists(string connectionString, string tableName)
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

    public static bool InsertValue(string connectionString, string tableName, string startDate, string startTime,
    string endDate, string endTime)
    {
        bool insertSuccess = false;
        int rowsUpdated;
        if(TableExists(connectionString,tableName))
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $@"INSERT INTO {tableName} (StartDate, EndDate, StartTime, EndTime)
                    VALUES ( '{startDate}', '{endDate}', '{startTime}', '{endTime}');";

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

    public static bool InsertValue(string connectionString, string tableName, string startDate, string startTime)
    {
        bool insertSuccess = false;
        int rowsUpdated;
        if(TableExists(connectionString,tableName))
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
       
        if (TableExists(connectionString, tableName))
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

    public static bool UpdateValue(string connectionString, string tableName, int ID, string startDate, string startTime,
    string endDate, string endTime)
    {
        bool updateValueSuccess = false; 
        if (TableExists(connectionString,tableName))
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
}