using System.Globalization;
using CodingTracker.rthring.Models;
using Microsoft.Data.Sqlite;

namespace CodingTracker.rthring
{
    public class DatabaseController
    {
        private string ConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");
        public DatabaseController()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_session (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime Text,
                        Duration INTEGER
                        )";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        // insert into coding_session (StartTime, EndTime, Duration) VALUES ('2023/09/25; 01:17', '2023/09/25; 02:18', 61)

        // SELECT * FROM coding_session

        internal List<CodingSession> GetRecords()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM coding_session";

                List<CodingSession> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                StartTime = DateTime.ParseExact(reader.GetString(1), "yyyy/MM/dd HH:mm", new CultureInfo("en-US")),
                                EndTime = DateTime.ParseExact(reader.GetString(2), "yyyy/MM/dd HH:mm", new CultureInfo("en-US")),
                                Duration = reader.GetInt32(3)
                            });
                    }
                }

                connection.Close();

                return tableData;
            }
        }

        internal void InsertRecord(CodingSession session)
        {
            string startTime = session.StartTime.ToString("yyyy/MM/dd HH:mm", new CultureInfo("en-US"));
            string endTime = session.EndTime.ToString("yyyy/MM/dd HH:mm", new CultureInfo("en-US"));
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"INSERT INTO coding_session(StartTime, EndTime, Duration) VALUES('{startTime}', '{endTime}', {session.Duration})";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        
        internal bool DeleteRecord(int id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE from coding_session WHERE Id = {id}";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
                return true;
            }

        }

        internal bool RecordExistsById(int id)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_session WHERE Id = {id})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    connection.Close();
                    return false;
                }
                connection.Close();
                return true;
            }
        }

        internal void UpdateRecord(CodingSession session)
        {
            string startTime = session.StartTime.ToString("yyyy/MM/dd HH:mm", new CultureInfo("en-US"));
            string endTime = session.EndTime.ToString("yyyy/MM/dd HH:mm", new CultureInfo("en-US"));
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE coding_session SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = {session.Duration} WHERE Id = {session.Id}";
                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
