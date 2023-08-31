using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.w0lvesvvv
{
    public static class DataBaseManager
    {
        private static readonly string ConnectionString = ConfigurationManager.AppSettings["connectionString"] ?? "";

        public static void CreateDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
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

        public static void InsertCodingTime(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = $@" INSERT INTO coding_session (coding_session_start_date_time_nv, coding_session_end_date_time_nv, coding_session_duration_i)
                                        SELECT '{codingSession.Coding_session_start_date_time_nv}', '{codingSession.Coding_session_end_date_time_nv}', {codingSession.GetDuration()};";

                query.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static List<CodingSession> GetCodingRecords()
        {
            using (var connection = new SqliteConnection(ConnectionString))
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
                            Coding_session_id_i = result.GetInt32(0),
                            Coding_session_start_date_time_nv = result.GetString(1),
                            Coding_session_end_date_time_nv = result.GetString(2)
                        };

                        codingRecords.Add(record);
                    }

                }

                connection.Close();

                return codingRecords;
            }
        }

        public static void UpdateCodingTime(CodingSession codingSession)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = $@" UPDATE coding_session SET 
                                            coding_session_start_date_time_nv = '{codingSession.Coding_session_start_date_time_nv}'
                                          , coding_session_end_date_time_nv = '{codingSession.Coding_session_end_date_time_nv}'
                                          , coding_session_duration_i = {codingSession.GetDuration()}
                                        WHERE coding_session_id_i = {codingSession.Coding_session_id_i};";

                query.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static void DeleteCodingTime(int id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var query = connection.CreateCommand();
                query.CommandText = $@" DELETE FROM coding_session WHERE coding_session_id_i = {id};";

                query.ExecuteNonQuery();

                connection.Close();
            }
        }
        

    }
}

