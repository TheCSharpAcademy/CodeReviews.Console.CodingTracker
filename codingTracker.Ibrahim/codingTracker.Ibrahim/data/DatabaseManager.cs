using codingTracker.Ibrahim.Helpers;
using codingTracker.Ibrahim.Models;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace codingTracker.Ibrahim.data
{
    public class DatabaseManager
    {
        public DatabaseManager()
        {
            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        CREATE TABLE IF NOT EXISTS coding_tracker(
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartTime TEXT,
                        EndTime TEXT,
                        Duration TEXT)";
                    command.ExecuteNonQuery();
                }
            }
        }
        public static CodingSession GetOne(int Id)
        {
            Id = helper.SessionExists(Id, true);
            CodingSession session = new CodingSession();

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * FROM coding_tracker WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            session.Id = reader.GetInt32(0);
                            session.StartTime = reader.GetString(1);
                            session.EndTime = reader.GetString(2);
                            session.Duration = reader.GetString(3);
                        }
                    }
                }
            }
            return session;
        }
        public static List<CodingSession> GetALLData()
        {
            List<CodingSession> History = new List<CodingSession>();

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "Select * FROM coding_tracker";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CodingSession session = new CodingSession();
                            session.Id = reader.GetInt32(0);
                            session.StartTime = reader.GetString(1);
                            session.EndTime = reader.GetString(2);
                            session.Duration = reader.GetString(3);

                            History.Add(session);
                        }
                    }
                }
            }
            return History;
        }
        public static void UpdateData(int Id, string? StartTime, string? EndTime)
        {
            Id = helper.SessionExists(Id, true);

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    {
                        (StartTime, EndTime) = helper.ValidateDateTimes(StartTime, EndTime);

                        command.CommandText = "Update coding_tracker SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@StartTime", StartTime);
                        command.Parameters.AddWithValue("@EndTime", EndTime);
                        String Duration = helper.CalculateDuration(StartTime, EndTime);
                        command.Parameters.AddWithValue("@Duration", Duration);
                        command.ExecuteNonQuery();

                    }
                    else if (!string.IsNullOrEmpty(StartTime) && string.IsNullOrEmpty(EndTime))
                    {
                        StartTime = helper.ValidateDateTime(Id, StartTime, null).startTime;

                        command.CommandText = "Update coding_tracker SET StartTime = @StartTime, Duration = @Duration WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@StartTime", StartTime);
                        String Duration = helper.CalculateDuration(StartTime, DatabaseManager.GetOne(Id).EndTime);
                        command.Parameters.AddWithValue("@Duration", Duration);
                        command.ExecuteNonQuery();

                    }
                    else if (string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
                    {
                        EndTime = helper.ValidateDateTime(Id, null, EndTime).endTime;

                        command.CommandText = "Update coding_tracker SET EndTime = @EndTime, Duration = @Duration WHERE Id = @Id";
                        command.Parameters.AddWithValue("@Id", Id);
                        command.Parameters.AddWithValue("@EndTime", EndTime);
                        String Duration = helper.CalculateDuration(DatabaseManager.GetOne(Id).StartTime, EndTime);
                        command.Parameters.AddWithValue("@Duration", Duration);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
        public static void DeleteData(int Id)
        {
            Id = helper.SessionExists(Id, true);

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM coding_tracker WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static void InsertData(string StartTime, string EndTime)
        {
            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO coding_tracker (StartTime,EndTime,Duration) VALUES (@StartTime, @EndTime, @Duration)";
                    command.Parameters.AddWithValue("@StartTime", StartTime);
                    command.Parameters.AddWithValue("@EndTime", EndTime);
                    string Duration = helper.CalculateDuration(StartTime, EndTime);
                    command.Parameters.AddWithValue("@Duration", Duration);
                    command.ExecuteNonQuery();
                }
            }
        }
        public static bool SessionExists(int Id)
        {
            bool sessionExists;
            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM coding_tracker WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", Id);
                    sessionExists = Convert.ToBoolean(command.ExecuteScalar());
                }
            }
            return sessionExists;
        }

        public static List<CodingSessionReport> GetReports(string A_or_T,string W_or_M_or_Y )
        {
            A_or_T = A_or_T == "A" ? "AVG" : "SUM";
            Console.WriteLine(A_or_T + W_or_M_or_Y);
            
            List<CodingSessionReport> weeklyHistory = new List<CodingSessionReport>();

            using (var connection = new SqliteConnection(ConfigurationManager.AppSettings.Get("connectionString")))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @$"
            SELECT
                strftime('%{W_or_M_or_Y}', 
                    SUBSTR('0' || SUBSTR(StartTime, 7, 4), -4) || '-' ||
                    SUBSTR('0' || SUBSTR(StartTime, 4, 2), -2) || '-' ||
                    SUBSTR('0' || SUBSTR(StartTime, 1, 2), -2) || ' ' ||
                    CASE 
                        WHEN UPPER(TRIM(SUBSTR(StartTime, -2))) = 'PM' AND TRIM(SUBSTR(StartTime, 12, 2)) != '12' THEN 
                            SUBSTR('0' || (CAST(TRIM(SUBSTR(StartTime, 12, 2)) AS INTEGER) + 12), -2)
                        WHEN UPPER(TRIM(SUBSTR(StartTime, -2))) = 'AM' AND TRIM(SUBSTR(StartTime, 12, 2)) = '12' THEN 
                            '00'
                        ELSE 
                            SUBSTR('0' || TRIM(SUBSTR(StartTime, 12, 2)), -2)
                    END || ':' || 
                    SUBSTR('0' || TRIM(SUBSTR(StartTime, 15, 2)), -2) || ':00'
                ) AS WeekNumber,
                {A_or_T}(
                    (CAST(SUBSTR(Duration, 1, INSTR(Duration, ' hours') - 1) AS INTEGER) * 60) +
                    CAST(SUBSTR(Duration, INSTR(Duration, 'and ') + 4, INSTR(Duration, ' minutes') - INSTR(Duration, 'and ') - 4) AS INTEGER)
                ) AS TotalDurationMinutes
            FROM
                coding_tracker
            GROUP BY
                WeekNumber
            ORDER BY
                WeekNumber;";

                    switch (W_or_M_or_Y)
                    {
                        case "W":
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    //Console.WriteLine($"Debug: Weeks = {reader.Get(0)}, Duration = {reader.GetDataTypeName(1)}");

                                    var weeklySummary = new WeeklyCodingSessionReport
                                    {
                                        Weeks = reader.GetString(0),
                                        Duration = TimeSpan.FromMinutes(reader.GetInt32(1))
                                    };

                                    weeklyHistory.Add(weeklySummary);
                                }
                            }
                            break;
                        case "M":
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var weeklySummary = new MonthlyCodingSessionReport
                                    {
                                        Months = reader.GetString(0),
                                        Duration = TimeSpan.FromMinutes(reader.GetInt32(1))
                                    };

                                    weeklyHistory.Add(weeklySummary);
                                }
                            }
                            break;
                        case "Y":
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var weeklySummary = new YearlyCodingSessionReport
                                    {
                                        Years = reader.GetString(0),
                                        Duration = TimeSpan.FromMinutes(reader.GetInt32(1))
                                    };

                                    weeklyHistory.Add(weeklySummary);
                                }
                            }
                            break;
                    }      
                }
            }
            return weeklyHistory;
        }
    }
}