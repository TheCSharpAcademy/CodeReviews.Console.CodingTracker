using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker.CSharpAcademy_Learner
{
    public static class DatabaseManager
    {
        public static void InitializeDatabase()
        {
            var createTableSQL = @"CREATE TABLE IF NOT EXISTS coding_sessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER
                )";

            using (var connection = new SqliteConnection(Configuration.GetConnectionString()))
            {
                connection.Execute(createTableSQL);
            }
        }
    }
}
