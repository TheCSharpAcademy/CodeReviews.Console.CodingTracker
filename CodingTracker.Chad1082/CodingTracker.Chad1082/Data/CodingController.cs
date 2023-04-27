using CodingTracker.Chad1082.Models;
using Microsoft.Data.Sqlite;

namespace CodingTracker.Chad1082.Data
{
    internal class CodingController
    {
        public static List<CodingSession> GetSessions()
        {
            List<CodingSession> codingSessions = new();

            using (SqliteConnection conn = new SqliteConnection(Database.connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM ""main"".""CodingSessions""";

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        codingSessions.Add(new CodingSession
                        {
                            Id = int.Parse(reader["ID"].ToString()),
                            StartTime = DateTime.Parse(reader["StartDateTime"].ToString()),
                            EndTime = DateTime.Parse(reader["EndDateTime"].ToString())
                        });
                    }

                }
                conn.Close();
            }

            return codingSessions;
        }
        public static CodingSession GetSingleSession(int ID)
        {
            CodingSession codingSession = new();

            using (SqliteConnection conn = new SqliteConnection(Database.connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"SELECT * FROM ""main"".""CodingSessions"" WHERE ""ID""=$Id";
                command.Parameters.AddWithValue("$Id", ID);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        codingSession.Id = int.Parse(reader["ID"].ToString());
                        codingSession.StartTime = DateTime.Parse(reader["StartDateTime"].ToString());
                        codingSession.EndTime = DateTime.Parse(reader["EndDateTime"].ToString());
                        conn.Close();

                        return codingSession;
                    }
                }
            }
            return null;
        }
        public static bool DeleteSession(int Id)
        {
            bool result = false;

            using (SqliteConnection conn = new SqliteConnection(Database.connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"DELETE FROM ""main"".""CodingSessions"" WHERE ""ID"" = $sessiontodelete";
                command.Parameters.AddWithValue("$sessiontodelete", Id);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.RecordsAffected > 0)
                    {
                        result = true;
                    }

                }
                conn.Close();
            }
            return result;
        }
        public static bool UpdateSession(int Id, string newStartDate, string newEndDate)
        {
            bool result = false;

            using (SqliteConnection conn = new SqliteConnection(Database.connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"UPDATE ""main"".""CodingSessions"" SET ""StartDateTime""=$newStartDate, ""EndDateTime""=$newEndDate WHERE ""ID""=$ID";
                command.Parameters.AddWithValue("$ID", Id);
                command.Parameters.AddWithValue("$newStartDate", newStartDate);
                command.Parameters.AddWithValue("$newEndDate", newEndDate);

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.RecordsAffected > 0)
                    {
                        result = true;
                    }
                }
                conn.Close();
            }
            return result;
        }
        public static void AddSession(string startDate, string endDate)
        {
            using (SqliteConnection conn = new SqliteConnection(Database.connString))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = @"INSERT INTO ""main"".""CodingSessions""(""StartDateTime"",""EndDateTime"") VALUES ($StartDateTime,$EndDateTime);";

                command.Parameters.AddWithValue("$StartDateTime", startDate);
                command.Parameters.AddWithValue("$EndDateTime", endDate);
                command.ExecuteNonQuery();
                conn.Close();


            }
        }
    }
}
