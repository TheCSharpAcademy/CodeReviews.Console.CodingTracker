using codingTracker.Ibrahim.Helpers;
using codingTracker.Ibrahim.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace codingTracker.Ibrahim.data
{
    public class DatabaseManager
    {
        public DatabaseManager()
        {
            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using(var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS coding_tracker(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT)";
                    command.ExecuteNonQuery();
                }               
            }
        }
        public static CodingSession GetOne(int Id)
        {
            Id = helper.SessionExists(Id, true);
            CodingSession session = new CodingSession();

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * FROM coding_tracker WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {                         
                            session.Id = reader.GetInt32(0);
                            session.StartTime = reader.GetString(1);
                            session.EndTime = reader.GetString(2);
                            session.Duration = reader.GetString(3);
                        }
                    }
                }
            }
            return session;
        }
        public static List<CodingSession> GetALLData()
        {
            List<CodingSession> History = new List<CodingSession>();

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * FROM coding_tracker";
                    
                    using(var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CodingSession session = new CodingSession();
                            session.Id = reader.GetInt32(0);
                            session.StartTime = reader.GetString(1);
                            session.EndTime = reader.GetString(2);
                            session.Duration = reader.GetString(3);

                            History.Add(session);
                        }
                    }
                }
            }
            return History;
        }
        public static void UpdateData(int Id, string? StartTime, string? EndTime)
        {
            Id = helper.SessionExists(Id, true);

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    {
                        (StartTime,EndTime) = helper.ValidateDateTimes(StartTime, EndTime);
                        
                        command.CommandText = "Update coding_tracker SET StartTime = @StartTime, EndTime = @EndTime WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@StartTime", StartTime);
                        command.Parameters.AddWithValue("@EndTime", EndTime);
                    }
                    else if (!string.IsNullOrEmpty(StartTime) && string.IsNullOrEmpty(EndTime))
                    {
                        StartTime = helper.ValidateDateTime(Id,StartTime,null);

                        command.CommandText = "Update coding_tracker SET StartTime = @StartTime WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@StartTime", StartTime);
                    }
                    else if (string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    {
                        EndTime = helper.ValidateDateTime(Id,null,EndTime);

                        command.CommandText = "Update coding_tracker SET EndTime = @EndTime WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@EndTime", EndTime);
                    }
                }
            }
        }
        public static void DeleteData(int Id)
        {
            Id = helper.SessionExists(Id, true);

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM coding_tracker WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void InsertData(string StartTime, string EndTime)
        {
            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO coding_tracker (StartTime,EndTime,Duration) VALUES (@StartTime, @EndTime, @Duration)";
                    command.Parameters.AddWithValue("@StartTime", StartTime);
                    command.Parameters.AddWithValue("@EndTime", EndTime);
                    string Duration = helper.CalculateDuration(StartTime, EndTime);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool SessionExists(int Id)
        {
            bool sessionExists;
            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM coding_tracker WHERE Id = @Id)";
                    command.Parameters.AddWithValue("@Id", Id);    
                    sessionExists= Convert.ToBoolean(command.ExecuteScalar());
                }
            }
            return sessionExists;
        }

        internal static void GetReports()
        {
            throw new NotImplementedException();
        }
    }
}