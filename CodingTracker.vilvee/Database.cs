
using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using System.Data;


namespace CodingTracker.vilvee;

static class Database
{
    private static string databaseConnection = System.Configuration.ConfigurationManager.ConnectionStrings["connectionDb"].ConnectionString;

    public static void AddNewSessionToDatabase(CodingSession codingSession)
    {
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            CreateSessionTable(connection);

            string totalDuration = codingSession.EndTime == null ? "Ongoing" : codingSession.Duration;
            using (var tableCmd = connection.CreateCommand())
            {
                tableCmd.CommandText = @"
                        INSERT INTO CodingSession (StartTime, EndTime, TotalDuration)
                        VALUES (@StartTime, @EndTime, @TotalDuration)";

                tableCmd.Parameters.AddWithValue("@StartTime", codingSession.StartTime.ToString("o"));
                tableCmd.Parameters.AddWithValue("@EndTime", codingSession.EndTime?.ToString("o") ?? Convert.DBNull);
                tableCmd.Parameters.AddWithValue("@TotalDuration", totalDuration);

                tableCmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    private static void CreateSessionTable(SqliteConnection connection)
    {
        using (var createTable = connection.CreateCommand())
        {
            createTable.CommandText = @"
                CREATE TABLE IF NOT EXISTS CodingSession(
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT NOT NULL,
                    EndTime TEXT,
                    TotalDuration TEXT 
                )";
            createTable.ExecuteNonQuery();
        }
    }

    public static void EndSessionInDatabase(int id, DateTime endTime)
    {
        var codingSession = RetrieveSession(id);
        if (codingSession == null)
        {
            Console.WriteLine($"Session with ID {id} not found.");
            return;
        }

        codingSession.EndTime = endTime;

        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = @"
                        UPDATE CodingSession
                        SET EndTime = @EndTime, TotalDuration = @TotalDuration
                        WHERE Id = @Id";

                cmd.Parameters.AddWithValue("@EndTime", codingSession.EndTime?.ToString("o"));
                cmd.Parameters.AddWithValue("@TotalDuration", codingSession.Duration);
                cmd.Parameters.AddWithValue("@Id", id);

                cmd.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public static CodingSession RetrieveSession(int id)
    {
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"SELECT Id, StartTime, EndTime FROM CodingSession WHERE Id = @Id";
            tableCmd.Parameters.AddWithValue("@Id", id);

            using (var reader = tableCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return new CodingSession(
                        reader.GetDateTime(reader.GetOrdinal("StartTime")),
                        reader.IsDBNull(reader.GetOrdinal("EndTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndTime"))
                    )
                    { Id = reader.GetInt32(reader.GetOrdinal("Id")) };
                }
            }

            connection.Close();
        }

        return null;
    }


    /// <summary>
    /// OPTION 4
    /// </summary>
    public static List<CodingSession> RetrieveAllSessions()
    {
        List<CodingSession> codingSessionList = new();
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = "SELECT * FROM CodingSession";

            using (var reader = tableCmd.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    var cs = new CodingSession(
                            reader.GetDateTime(reader.GetOrdinal("StartTime")),
                            reader.IsDBNull(reader.GetOrdinal("EndTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("EndTime"))
                        )
                        { Id = reader.GetInt32(reader.GetOrdinal("Id")) };

                    codingSessionList.Add(cs); 
                }

                if (!codingSessionList.Any())
                {
                    Console.WriteLine("No rows found");
                }
            }
            connection.Close();
        }

        PrintTable(codingSessionList); 

        return codingSessionList;
    }



    public static void PrintTable(List<CodingSession> codingSessionList)
    {
        // Create a DataTable and add columns
        DataTable table = new DataTable();
        table.Columns.Add("ID", typeof(int));
        table.Columns.Add("Start Time", typeof(string));
        table.Columns.Add("End Time", typeof(string));
        table.Columns.Add("Total Duration", typeof(string));

        // Add rows from codingSessionList
        foreach (var session in codingSessionList)
        {
            table.Rows.Add(session.Id, session.FormattedStartTime, session.FormattedEndTime, session.TotalSessionDuration);
        }

        // Use ConsoleTableBuilder with DataTable
        ConsoleTableBuilder
            .From(table)
            .WithFormat(ConsoleTableBuilderFormat.Default)
            .ExportAndWriteLine();
    }


    internal static DateTime GetStartTimeFromDb(int id)
    {
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT StartTime FROM CodingSession WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            var result = cmd.ExecuteScalar();

            if (result == null || result == DBNull.Value)
            {
                throw new InvalidOperationException($"Start time not found for session with ID {id}.");
            }

            return Convert.ToDateTime(result);
        }
    }


    internal static DateTime? GetEndTimeFromDb(int id)
    {
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT EndTime FROM CodingSession WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Id", id);

            var result = cmd.ExecuteScalar();

            if (result != null && result != DBNull.Value)
            {
                return Convert.ToDateTime(result);
            }

            connection.Close();
            return null;
        }
    }

    public static bool UpdateSession(int id, DateTime startTime, DateTime? endTime)
    {
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM CodingSession WHERE Id = @Id)";
            checkCmd.Parameters.AddWithValue("@Id", id);
            var exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {id} does not exist.\n\n");
                connection.Close();
                return false;
            }

            var tempSession = new CodingSession(startTime, endTime);

            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = @"
            UPDATE CodingSession
            SET StartTime = @StartTime, EndTime = @EndTime, TotalDuration = @TotalDuration
            WHERE Id = @Id";

            updateCmd.Parameters.AddWithValue("@Id", id);
            updateCmd.Parameters.AddWithValue("@StartTime",  (object)startTime.ToString("o"));
            updateCmd.Parameters.AddWithValue("@EndTime", endTime.HasValue ? (object)endTime.Value.ToString("o") : DBNull.Value);
            updateCmd.Parameters.AddWithValue("@TotalDuration", tempSession.Duration);

            updateCmd.ExecuteNonQuery();
            Console.WriteLine($"\n\nRecord {id} was successfully updated.\n\n");

            connection.Close();
        }
        return true;
    }

    public static bool DeleteRecordInDb(int id)
    {
        using (var connection = new SqliteConnection(databaseConnection))
        {
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT EXISTS(SELECT 1 FROM CodingSession WHERE Id = @Id)";
            checkCmd.Parameters.AddWithValue("@Id", id);
            var exists = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (exists == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {id} does not exist.\n\n");
                connection.Close();
                return false;
            }

            var updateCmd = connection.CreateCommand();
            updateCmd.CommandText = @"
            delete from CodingSession    
            WHERE Id = @Id";
            updateCmd.Parameters.AddWithValue("@Id", id);
            updateCmd.ExecuteNonQuery();
            

            Console.WriteLine($"\n\nRecord {id} was successfully deleted.\n\n");

            connection.Close();
        }
        return true;
    }
}