using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Data.Sqlite;

namespace CodeTracker.csm_stough
{
    public class Database
    {
        private static string connectionString;

        public static void Init()
        {
            connectionString = ConfigurationManager.AppSettings.Get("connectionString");

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

        public static int GetCount()
        {
            int count = 0;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                $@"SELECT COUNT(*) FROM coding_records";
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
                $@"INSERT INTO coding_records(Start, End, Duration) VALUES('{start}', '{end}', '{end - start}'); SELECT last_insert_rowid();";
                int id = Convert.ToInt32(command.ExecuteScalar());
                session = new CodingSession(id, start, end, end - start);
                connection.Close();
            }

            return session;
        }

        public static List<CodingSession> GetAll(int limit = int.MaxValue, int offset = 0)
        {
            List<CodingSession> records = new List<CodingSession>();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();
                command.CommandText =
                    $@"SELECT * FROM coding_records LIMIT {limit} OFFSET {offset}";

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
    }
}
