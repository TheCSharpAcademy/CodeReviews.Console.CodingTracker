using Microsoft.Data.Sqlite;
using System.Configuration;
using MapDataReader;

namespace CodingTracker
{
    public class DataAccessor
    {
        private string connectionString;

        public DataAccessor() => SetConnectionToAppSettingsReference();

        public void InitializeDatabase()
        {
            SetConnectionToAppSettingsReference();
            CreateMainTableIfMissing();
        }

        private void SetConnectionToAppSettingsReference()
        {
            connectionString = ConfigurationManager.AppSettings.Get("dbConnectionString");
            if (connectionString == null)
            {
                throw new NullReferenceException("Could not retrieve connection string from app settings.");
            }
        }

        private void CreateMainTableIfMissing()
        {
            using SqliteConnection conn = new(connectionString);
            conn.Open();
            string sql = @"CREATE TABLE IF NOT EXISTS tracker (
                         id INTEGER NOT NULL UNIQUE, 
                         start_time TEXT NOT NULL, 
                         end_time TEXT NOT NULL, 
                         duration TEXT NOT NULL, 
                         PRIMARY KEY(id AUTOINCREMENT));";
            SqliteCommand cmd = new(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public List<CodingSession> GetCodingSessions()
        {
            using SqliteConnection conn = new(connectionString);
            conn.Open();
            string sql = "SELECT * FROM tracker;";
            SqliteCommand cmd = new(sql, conn);
            return cmd.ExecuteReader().ToCodingSession();
        }

        public void AddEntry(string startTime, string endTime, string duration)
        {
            using SqliteConnection conn = new(connectionString);
            conn.Open();
            string sql = @"INSERT INTO tracker (start_time, end_time, duration) 
                           VALUES (@start_time, @end_time, @duration);";
            SqliteCommand cmd = new(sql, conn);
            AddParameter("@start_time", startTime, cmd);
            AddParameter("@end_time", endTime, cmd);
            AddParameter("@duration", duration, cmd);
            cmd.ExecuteNonQuery();
        }

        public void DeleteEntry(int id)
        {
            using SqliteConnection conn = new(connectionString);
            conn.Open();
            string sql = "DELETE FROM tracker WHERE id = @id;";
            SqliteCommand cmd = new(sql, conn);
            AddParameter("@id", id, cmd);
            cmd.ExecuteNonQuery();
        }

        public void UpdateEntry(int id, string startTime, string endTime, string duration)
        {
            using SqliteConnection conn = new(connectionString);
            conn.Open();
            string sql = "UPDATE tracker SET start_time = @start_time, end_time = @end_time, duration = @duration WHERE id = @id;";
            SqliteCommand cmd = new(sql, conn);
            AddParameter("@id", id, cmd);
            AddParameter("@start_time", startTime, cmd);
            AddParameter("@end_time", endTime, cmd);
            AddParameter("@duration", duration, cmd);
            cmd.ExecuteNonQuery();
        }

        protected static void AddParameter<T>(string name, T value, SqliteCommand cmd)
        {
            SqliteParameter param = new(name, SqliteTypeConverter.GetDbType(value.GetType()))
            {
                Value = value
            };
            cmd.Parameters.Add(param);
        }
    }
}
