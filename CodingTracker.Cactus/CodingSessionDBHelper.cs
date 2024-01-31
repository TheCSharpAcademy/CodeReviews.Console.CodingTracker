using Microsoft.Data.Sqlite;

namespace CodingTracker.Cactus
{
    public class CodingSessionDBHelper
    {
        private static string GetConnectionStr()
        {
            string dataSource = System.Configuration.ConfigurationManager.AppSettings.Get("DataSource");
            return $"Data Source={dataSource}";
        }

        public static void CreateCodingTrackerTableIfNotExist()
        {
            using (var connection = new SqliteConnection(GetConnectionStr()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                @"
                CREATE TABLE IF NOT EXISTS codingTracker (
                    id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    startTime STRING NOT NULL,
                    endTime STRING NOT NULL
                );
                ";
                command.ExecuteNonQuery();
            }
        }

        public static int Insert(CodingSession codingSession)
        {
            int id = -1;
            using (var connection = new SqliteConnection(GetConnectionStr()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $@"INSERT INTO codingTracker(startTime, endTime) 
                                     VALUES('{codingSession.StartTime}', '{codingSession.EndTime}');
                                     SELECT last_insert_rowid();";
                id = Convert.ToInt32(command.ExecuteScalar());
            }
            return id;
        }

        public static List<CodingSession> SeleteAll()
        {
            List<CodingSession> codingSessions = new();
            using (var connection = new SqliteConnection(GetConnectionStr()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = @"SELECT * FROM codingTracker";
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = int.Parse(reader.GetString(0));
                        DateTime startTime = DateTime.Parse(reader.GetString(1));
                        DateTime endTime = DateTime.Parse(reader.GetString(2));
                        codingSessions.Add(new CodingSession(id, startTime, endTime));
                    }
                }
            }
            return codingSessions;
        }

        public static void Update(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(GetConnectionStr()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $@"UPDATE codingTracker SET startTime='{codingSession.StartTime}', 
                                         endTime='{codingSession.EndTime}'
                                         WHERE id='{codingSession.Id}'";
                command.ExecuteNonQuery();
            }
        }

        public static void Delete(int id)
        {
            using (var connection = new SqliteConnection(GetConnectionStr()))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = $@"DELETE FROM codingTracker WHERE id='{id}'";
                command.ExecuteNonQuery();
            }
        }
    }
}
