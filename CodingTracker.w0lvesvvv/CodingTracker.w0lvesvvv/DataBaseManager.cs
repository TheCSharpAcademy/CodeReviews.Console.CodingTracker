using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.w0lvesvvv
{
    public static class DataBaseManager
    {
        private static readonly string connectionString = ConfigurationManager.AppSettings["connectionString"] ?? "";

        public static void createDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var query = connection.CreateCommand();
                query.CommandText = @"CREATE TABLE IF NOT EXISTS coding_session (
                                          coding_session_id_i INTEGER PRIMARY KEY AUTOINCREMENT,
                                          coding_session_start_date_time_nv NVARCHAR,
                                          coding_session_end_date_time_nv NVARCHAR,
                                          coding_session_duration_i INTEGER
                                      );";

                query.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void insertCodingTime(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = $@" INSERT INTO coding_session (coding_session_start_date_time_nv, coding_session_end_date_time_nv, coding_session_duration_i)
                                        SELECT '{codingSession.coding_session_start_date_time_nv}', '{codingSession.coding_session_end_date_time_nv}', {codingSession.getDuration()};";

                int rows = query.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static List<CodingSession> getCodingRecords()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = $@"SELECT * FROM coding_session;";

                List<CodingSession> codingRecords = new();

                SqliteDataReader result = query.ExecuteReader();

                if (result.HasRows)
                {
                    while (result.Read())
                    {
                        CodingSession record = new CodingSession
                        {
                            coding_session_id_i = result.GetInt32(0),
                            coding_session_start_date_time_nv = result.GetString(1),
                            coding_session_end_date_time_nv = result.GetString(2)
                        };

                        codingRecords.Add(record);
                    }

                }

                connection.Close();

                return codingRecords;
            }
        }
    }
}

