using System.Collections.Specialized;
using System.Configuration;
using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CodingTracker
{
    internal class Database
    {
        private string dbPath = ConfigurationManager.AppSettings["dbPath"];
        private SqliteConnection connection = new SqliteConnection(ConfigurationManager.AppSettings["connectionString"]);

        internal Database()
        {
            SqlMapper.AddTypeHandler(new TimeSpanHandler());
            CreateTableIfNotExists();
        }

        private void CreateTableIfNotExists()
        {
            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS coding_session (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT,
                EndTime TEXT,
                Duration TEXT
            );";

            connection.Execute(createTableQuery);
        }

        internal List<CodingSession> GetAllSession()
        {
            string sql = "SELECT * FROM coding_session ORDER BY StartTime ASC;";
            List<CodingSession> sessions = connection.Query<CodingSession>(sql).ToList();
            return sessions;
        }

        internal void Insert(CodingSession session)
        {
            string sql = $"INSERT INTO coding_session(StartTime, EndTime, Duration) VALUES(@StartTime, @EndTime, @Duration)";
            connection.Execute(sql, session);
        }

        internal void Delete(CodingSession session)
        {
            string sql = $"DELETE from coding_session WHERE Id = @Id";
            connection.Execute(sql, session);
        }

        internal CodingSession GetSessionById(int sessionId)
        {
            string sql = "SELECT * FROM coding_session WHERE Id = @Id;";
            CodingSession session = connection.QuerySingleOrDefault<CodingSession>(sql, new { Id = sessionId });
            return session;
        }

        internal void Update(CodingSession session)
        {
            string sql = "UPDATE coding_session SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
            connection.Execute(sql, session);
        }

        internal CodingSession GetOverlappedSession(DateTime StartTime, DateTime EndTime)
        {
            string sql = "SELECT * FROM coding_session WHERE StartTime < @EndTime  AND EndTime > @StartTime";
            return connection.QueryFirstOrDefault<CodingSession>(sql, new { StartTime, EndTime });
        } 
    }
    internal class TimeSpanHandler : SqlMapper.TypeHandler<TimeSpan>
    {
        public override void SetValue(System.Data.IDbDataParameter parameter, TimeSpan value)
        {
            parameter.Value = value.ToString();
        }

        public override TimeSpan Parse(object value)
        {
            return TimeSpan.Parse(value.ToString());
        }
    }
}
