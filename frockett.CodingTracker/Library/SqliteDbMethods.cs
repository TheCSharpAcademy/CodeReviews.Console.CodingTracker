using frockett.CodingTracker.Library;
using Microsoft.Data.Sqlite;
using System.Globalization;

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
                                      VALUES ('{session.StartTime}','{session.EndTime}', '{session.Duration}')"; // Use TimeSpan FromTicks() to reconstitute the time
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

    public bool CheckForTableData()
    {
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM coding_time";

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
        //string validFormat = "dd-MM-yyyy hh:mm";
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
                            //StartTime = DateTime.ParseExact(reader.GetString(1), validFormat, CultureInfo.InvariantCulture),
                            StartTime = DateTime.Parse(reader.GetString(1)),
                            EndTime = DateTime.Parse(reader.GetString(2)),
                            //EndTime = DateTime.ParseExact(reader.GetString(2), validFormat, CultureInfo.InvariantCulture),
                            Duration = TimeSpan.Parse(reader.GetString(3))
                            //Duration = TimeSpan.FromTicks(reader.GetInt64(3))
                        });
                }
            }

            connection.Close();

            return sessions;
        }
    }
}
