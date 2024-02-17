using Microsoft.Data.Sqlite;

namespace CodeTracker.csm_stough
{
    public class SessionDao
    {
        private static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

        public static void Init()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_records (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Start TEXT,
                        End TEXT,
                        Duration TEXT
                        )";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static int GetCount(string where = "")
        {
            int count = 0;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                $@"SELECT COUNT(*) FROM coding_records ";

                if (!string.IsNullOrEmpty(where))
                {
                    command.CommandText += "WHERE " + where;
                }

                count = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }

            return count;
        }

        public static CodingSession Insert(DateTime start, DateTime end)
        {
            CodingSession session = null;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                $@"INSERT INTO coding_records(Start, End, Duration) VALUES('{start.ToString("yyyy-MM-dd hh:mm:ss")}', '{end.ToString("yyyy-MM-dd hh:mm:ss")}', '{end - start}'); SELECT last_insert_rowid();";
                int id = Convert.ToInt32(command.ExecuteScalar());
                session = new CodingSession(id, start, end, end - start);
                connection.Close();
            }

            return session;
        }

        public static List<CodingSession> GetAll(int limit = int.MaxValue, int offset = 0, string where = "", bool ascending = true)
        {
            List<CodingSession> records = new List<CodingSession>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText = $"SELECT * FROM coding_records ";
                if(!string.IsNullOrEmpty(where))
                {
                    command.CommandText += "WHERE " + where;
                }
                command.CommandText += "ORDER BY Start ";
                command.CommandText += ascending ? "ASC " : "DESC ";
                command.CommandText += $"LIMIT {limit} OFFSET {offset} ";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        records.Add(new CodingSession(reader.GetInt32(0), reader.GetDateTime(1), reader.GetDateTime(2), reader.GetTimeSpan(3)));
                    }
                }

                connection.Close();
            }

            return records;
        }

        public static void Update(CodingSession session)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"UPDATE coding_records SET Start='{session.startTime}', End='{session.endTime}', Duration='{session.endTime - session.startTime}' WHERE Id={session.id}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void Delete(CodingSession session)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"DELETE FROM coding_records WHERE Id={session.id}";

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static List<ReportRecord> GetAllGroupedByTime(string dateFormat = "%Y-%m-%d", int limit = int.MaxValue, int offset = 0, bool ascending = true)
        {
            List<ReportRecord> records = new List<ReportRecord>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    $@"SELECT 
                     STRFTIME('{dateFormat}', Start) date,
                     COUNT(*) records_number,
                     TIME(SUM(STRFTIME('%s', Duration) - STRFTIME('%s', '00:00:00')), 'unixepoch') total_duration
                    FROM coding_records
                    GROUP BY date
                    ORDER BY date ";
                command.CommandText += ascending ? "ASC " : "DESC ";
                command.CommandText += $"LIMIT {limit} OFFSET {offset}";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        records.Add(new ReportRecord(reader.GetString(0), reader.GetInt32(1), reader.GetTimeSpan(2)));
                    }
                }

                connection.Close();
            }

            return records;
        }

        public static int GetCountGroupedByTime(string dateFormat = "%Y-%m-%d")
        {
            int count;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText = 
                    $@"SELECT COUNT(*) FROM (SELECT 
                     STRFTIME('{dateFormat}', Start) date,
                     COUNT(*) records_number,
                     TIME(SUM(STRFTIME('%s', Duration) - STRFTIME('%s', '00:00:00')), 'unixepoch') total_duration
                    FROM coding_records
                    GROUP BY date
                    ORDER BY date ASC)";

                count = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
            return count;
        }

        public static TimeSpan GetTotalDuration(DateTime date)
        {
            TimeSpan duration = TimeSpan.Zero;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText =
                    $@"SELECT STRFTIME('%Y-%m-%d', Start) date, TIME(SUM(STRFTIME('%s', Duration) - STRFTIME('%s', '00:00:00')), 'unixepoch') total_duration
                        FROM coding_records
                        ASC
                        WHERE Start BETWEEN DATE('now', 'start of month') AND DATE('now', 'start of month', '+1 month', '-1 day')
                        AND date = '{date.ToString("yyyy-MM-dd")}'
                        GROUP BY date";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        duration = reader.GetTimeSpan(1);
                    }
                }

                connection.Close();
            }

            return duration;
        }
    }
}
