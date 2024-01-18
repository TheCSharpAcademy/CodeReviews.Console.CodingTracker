using frockett.CodingTracker.Library;
using Microsoft.Data.Sqlite;

namespace Library
{
    public class SqliteDbMethods : IDbMethods
    {
        private string? connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connString");

        public void InitDatabase()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @"CREATE TABLE IF NOT EXISTS coding_time(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Start_Time TEXT,
                    End_Time TEXT,
                    Duration TEXT)";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public void AddCodingSession(CodingSession session)
        {
            throw new NotImplementedException();
        }

        public List<CodingSession> GetCodingSessions()
        {
            throw new NotImplementedException();
        }
    }
}
