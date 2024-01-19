using frockett.CodingTracker.Library;
using Microsoft.Data.Sqlite;

namespace Library;

public class SqliteDbMethods : IDbMethods
{
    private string? connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connString");

    public void InitDatabase()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @"CREATE TABLE IF NOT EXISTS coding_time(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Start_Time TEXT,
                    End_Time TEXT,
                    Duration BIGINT)";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void InsertCodingSession(CodingSession session)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = @$"INSERT INTO coding_time(start_time, end_time, duration) 
                                      VALUES ('{session.StartTime}','{session.EndTime}', '{session.Duration}')";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public void UpdateCodingSession(CodingSession session)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $@"UPDATE coding_time 
                                        SET start_time = '{session.StartTime}', end_time = '{session.EndTime}', duration = {session.Duration.Ticks} 
                                        WHERE id = {session.Id}";

            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public bool ValidateSessionById(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $@"SELECT EXISTS(SELECT 1 FROM coding_time WHERE Id = {id})";
            int checkQuery = Convert.ToInt32(command.ExecuteScalar());
            connection.Close();
            if (checkQuery == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    public void DeleteCodingSession(int id)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM coding_time WHERE id = {id}";
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public bool CheckForTableData(int year = 0, int month = 0)
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();

            if (year != 0)
            {
                command.CommandText = $"SELECT * FROM coding_time WHERE substr(start_time, 7, 4) = '{year}'";
            }
            else
            {
                command.CommandText = $"SELECT * FROM coding_time";
            }
   
            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                connection.Close();
                return true;
            }
            else
            {
                connection.Close();
                return false;
            }
        }
    }
    public List<CodingSession> GetAllCodingSessions()
    {
        List<CodingSession> sessions = new List<CodingSession>();

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM coding_time";

            SqliteDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    sessions.Add(
                        new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartTime = DateTime.Parse(reader.GetString(1)),
                            EndTime = DateTime.Parse(reader.GetString(2)),
                            Duration = TimeSpan.Parse(reader.GetString(3))
                        });
                }
            }
            connection.Close();
            return sessions;
        }
    }

    public List<CodingSession> GetCustomCodingSessions(bool isListOfAverages, DateOnly date)
    {
        List<CodingSession> sessions = new List<CodingSession>();

        if (isListOfAverages)
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"SELECT substr(start_time, 4, 2) || '/' || substr(start_time, 7, 4) AS month,
                                    ROUND(SUM(duration), 1) AS total_coding_time,
                                    ROUND(AVG(duration), 1) AS average_coding_time
                                    FROM coding_time
                                    GROUP BY month
                                    HAVING substr(start_time, 7, 4) = '{date.Year}'
                                    ORDER BY month";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sessions.Add(
                            new CodingSession
                            {
                                Month = reader.GetString(0),
                                TotalTime = double.Parse(reader.GetString(1)),
                                AverageTime = double.Parse(reader.GetString(2)),
                            });
                    }
                }
                connection.Close();
                return sessions;
            }
        }
        else
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = $@"SELECT * FROM coding_time
                                        WHERE substr(start_time, 7, 4) = '{date.Year}'
                                        AND substr(start_time, 4, 2) = '{date.ToString("MM")}'
                                        ORDER BY start_time";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sessions.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                StartTime = DateTime.Parse(reader.GetString(1)),
                                EndTime = DateTime.Parse(reader.GetString(2)),
                                Duration = TimeSpan.Parse(reader.GetString(3))
                            });
                    }
                }
                connection.Close();
                return sessions;
            }
        }
    }

    public void SeedRandomData(int iterations)
    {
        DataSeeding dataSeeding = new DataSeeding();

        for (int i = 0; i < iterations; i++)
        {
            CodingSession session = dataSeeding.GetRandomSession();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand command = connection.CreateCommand();
                command.CommandText = @$"INSERT INTO coding_time(start_time, end_time, duration) 
                                      VALUES ('{session.StartTime}','{session.EndTime}', '{session.Duration}')";
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
