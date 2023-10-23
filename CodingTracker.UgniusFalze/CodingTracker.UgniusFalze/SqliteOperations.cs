using Microsoft.Data.Sqlite;
using System.Globalization;

namespace CodingTracker
{
    internal class SqliteOperations
    {
        private SqliteConnection connection;

        public SqliteOperations(string dbFile = "codingTrackingLog.db")
        {
            connection = new SqliteConnection($"Data Source={dbFile}");
            connection.Open();
            CreateHabbitTrackerTable();
        }

        private void CreateHabbitTrackerTable()
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                CREATE TABLE IF NOT EXISTS CoddingTracker(
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL UNIQUE,
                    EndTime TEXT NOT NULL UNIQUE
                  );
            ";
            command.ExecuteNonQuery();
        }

        public CoddingSession GetSessionWithId(long id)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"
                SELECT Id, StartTime, EndTime
                FROM CoddingTracker 
                WHERE Id = $id";
            command.Parameters.AddWithValue("id", id);
            CultureInfo provider = CultureInfo.InvariantCulture;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read() == false)
                {
                    throw new ArgumentException();
                }
                else
                {
                    DateTime startDate = DateTime.ParseExact(reader.GetString(1), GetDateFormat(), provider);
                    DateTime endDate = DateTime.ParseExact(reader.GetString(2), GetDateFormat(), provider);
                    CoddingSession coddingSession = new(id, startDate, endDate);
                    return coddingSession;
                }
            }
        }

        public void InsertSession(DateTime startDate, DateTime endDate)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                INSERT INTO CoddingTracker(StartTime, EndTime) VALUES($startDate, $endDate);
            ";

            command.Parameters.AddWithValue("$startDate", startDate.ToString(GetDateFormat()));
            command.Parameters.AddWithValue("$endDate", endDate.ToString(GetDateFormat()));
            command.ExecuteNonQuery();
        }

        public void UpdateDate(long id, DateTime date, bool start = true)
        {
            CoddingSession session = GetSessionWithId(id);
            SqliteCommand command = connection.CreateCommand();
            string commandText;
            if(start)
            {
                Verifier.VerifyDate(date, session.EndTime);
                commandText = GetStartDateQuery();
            }
            else
            {
                Verifier.VerifyDate(session.StartTime, date);
                commandText = GetEndDateQuery();
            }

            command.CommandText = commandText;
            command.Parameters.AddWithValue("$date", date.ToString(GetDateFormat()));
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        private static string GetStartDateQuery()
        {
            return
            @"
                UPDATE CoddingTracker
                SET StartTime = $date
                WHERE Id = $id
                ;
            ";
        }
        private static string GetEndDateQuery()
        {
            return
            @"
                UPDATE CoddingTracker
                SET EndTime = $date
                WHERE Id = $id
                ;
            ";
        }

        public List<CoddingSession> GetSessions()
        {
            List<CoddingSession> sessions = new();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                SELECT Id, StartTime, EndTime
                FROM CoddingTracker
                ;
            ";
            CultureInfo provider = CultureInfo.InvariantCulture;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    long id = reader.GetInt64(0);
                    DateTime startDate = DateTime.ParseExact(reader.GetString(1), GetDateFormat(), provider);
                    DateTime endDate = DateTime.ParseExact(reader.GetString(2), GetDateFormat(), provider);
                    CoddingSession session = new(id, startDate, endDate);
                    sessions.Add(session);
                }
            }

            return sessions;
        }

        public void DeleteSession(long id)
        {
            GetSessionWithId(id);
            SqliteCommand command = connection.CreateCommand();
            command.CommandText =
            @"
                DELETE FROM CoddingTracker
                WHERE Id = $id
                ;
            ";
            command.Parameters.AddWithValue("$id", id);
            command.ExecuteNonQuery();
        }

        public static string GetDateFormat()
        {
            return "dd/MM/yyyy HH:mm:ss";
        }
    }
}
