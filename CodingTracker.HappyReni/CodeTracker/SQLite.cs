using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodeTracker
{
    internal class SQLite
    {
        public SQLite() 
        {
            TableName = ConfigurationManager.AppSettings.Get("TableName");
            CreateTable();
        }
        public string TableName { get; set; }

        private SqliteConnection GetConnection()
        {
            string connStr = ConfigurationManager.AppSettings.Get("DatabasePath");
            using var conn = new SqliteConnection(connStr);
            return conn;
        }
        
        public void CreateTable()
        {
            var conn = GetConnection();
            conn.Open();

            string createTableQuery = $"CREATE TABLE IF NOT EXISTS {TableName} (Id INTEGER PRIMARY KEY, Start TEXT, End TEXT, Duration TEXT)";
            using var createTableCommand = new SqliteCommand(createTableQuery, conn);
            createTableCommand.ExecuteNonQuery();
        }

        public void Insert(CodingSession code)
        {
            var id = code.Id;
            var start = code.StartTime.ToString();
            var end = code.EndTime.ToString();
            var duration = code.Duration.ToString();

            var conn = GetConnection();
            conn.Open();

            string insertQuery = $"INSERT INTO {TableName} (Id, Start, End, Duration) VALUES (@id, @start, @end, @duration)";
            using var insertCommand = new SqliteCommand(insertQuery, conn);
            insertCommand.Parameters.AddWithValue("@id", id);
            insertCommand.Parameters.AddWithValue("@start", start);
            insertCommand.Parameters.AddWithValue("@end", end);
            insertCommand.Parameters.AddWithValue("@duration", duration);
            var res = insertCommand.ExecuteNonQuery();
            var ret = res == 0 ? "Failed to log." : "Successfully logged.";
            Console.WriteLine(ret);
        }

        public void Delete(int idx)
        {
            var conn = GetConnection();
            conn.Open();

            string deleteQuery = $"DELETE FROM {TableName} WHERE Id = {idx}";
            using var deleteCommand = new SqliteCommand(deleteQuery, conn);
            var res = deleteCommand.ExecuteNonQuery();
            var ret = res == 0 ? "Failed to delete." : "Successfully deleted.";
            Console.WriteLine(ret);
        }

        public void Update(CodingSession code)
        {
            var id = code.Id;
            var start = code.StartTime.ToString();
            var end = code.EndTime.ToString();
            var duration = code.Duration.ToString();

            var conn = GetConnection();
            conn.Open();

            string updateQuery = $"UPDATE {TableName} SET Start = @start, End = @end, Duration= @duration WHERE Id = @idx";
            using var updateCommand = new SqliteCommand(updateQuery, conn);
            updateCommand.Parameters.AddWithValue("@start", start);
            updateCommand.Parameters.AddWithValue("@end", end);
            updateCommand.Parameters.AddWithValue("@duration", duration);
            updateCommand.Parameters.AddWithValue("@idx", id);

            var res = updateCommand.ExecuteNonQuery();
            var ret = res == 0 ? "Failed to update." : "Successfully updated.";
            Console.WriteLine(ret);
        }

        public void DropTable()
        {
            var conn = GetConnection();
            conn.Open();

            try
            {
                string dropTableQuery = $"DROP TABLE {TableName}";
                using var dropTableCommand = new SqliteCommand(dropTableQuery, conn);
                dropTableCommand.ExecuteNonQuery();
                Console.WriteLine("Successfully dropped.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ViewTables()
        {
            var conn = GetConnection();
            conn.Open();

            string viewTableQuery = $"SELECT name FROM sqlite_master WHERE type='table'";
            using var viewTableCommand = new SqliteCommand(viewTableQuery, conn);
            using var tableReader = viewTableCommand.ExecuteReader();

            if ( tableReader.HasRows != true )
            {
                Console.WriteLine($"There is no table!");
                return;
            }
            while (tableReader.Read())
            {
                string selectQuery = $"SELECT * From {TableName}";
                using var selectCommand = new SqliteCommand(selectQuery, conn);
                using var dataReader = selectCommand.ExecuteReader();

                int idx = 0 ;
                while (dataReader.Read())
                {
                    int id = dataReader.GetInt32(0);
                    string date = dataReader.GetString(1);
                    string log = dataReader.GetString(2);
                    Console.WriteLine($"\t>{id}:\t{date}\t{log}");
                    idx++;
                }
            }
        }
        public List<CodingSession> GetSqlData()
        {
            var conn = GetConnection();
            conn.Open();

            string selectQuery = $"SELECT * From \"{TableName}\"";
            using var selectCommand = new SqliteCommand(selectQuery, conn);
            using var dataReader = selectCommand.ExecuteReader();

            List<CodingSession> ret = new();
            while (dataReader.Read())
            {
                int id = dataReader.GetInt32(0);
                var start = DateTime.Parse(dataReader.GetString(1));
                var end = DateTime.Parse(dataReader.GetString(2));
                var duration = dataReader.GetDouble(3);
                ret.Add(new CodingSession(new List<object> { id, start, end, duration }));
            }
            return ret;
        }
    }

}
