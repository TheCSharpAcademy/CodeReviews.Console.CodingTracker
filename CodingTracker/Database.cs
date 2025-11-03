using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    public class Database(string connectionString) : IDisposable
    {
        private readonly SqliteConnection connection = new(connectionString);

        public void Migrate()
        {
            connection.Open();

            using SqliteCommand tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS coding_sessions (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        start_time TEXT,
        end_time TEXT,
        duration REAL
        )";

            _ = tableCmd.ExecuteNonQuery();

            connection.Close();

        }

        public void Save(CodingSession session)
        {
            connection.Open();
            string query = "insert into coding_sessions (start_time, end_time, duration) values(@Start, @End, @Duration)";

            _ = connection.Execute(query, session);
        }

        public List<CodingSession> Get()
        {
            connection.Open();
            List<CodingSession> records = [.. connection.Query<CodingSession>("select id, start_time as start, end_time as end, duration from coding_sessions")];
            return records;
        }

        public void Update(CodingSession session)
        {
            connection.Open();
            string query = "update coding_sessions set start_time = @Start, end_time = @End, duration = @Duration where id = @Id";
            _ = connection.Execute(query, session);
        }

        public void Delete(CodingSession session)
        {
            connection.Open();
            string query = "delete from coding_sessions where id = @Id";
            _ = connection.Execute(query, session);

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
