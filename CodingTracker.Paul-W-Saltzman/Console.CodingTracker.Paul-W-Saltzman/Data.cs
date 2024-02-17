using Microsoft.Data.Sqlite;
using System.Configuration;
using System.Globalization;

namespace CodingTracker.Paul_W_Saltzman
{
    internal static class Data
    {
        internal static string ConnectionString()
        {
            string dbName;
            dbName = ConfigurationManager.AppSettings.Get("db");
            string dbPath;
            dbPath = ConfigurationManager.AppSettings.Get("path");
            string projectPath = Directory.GetCurrentDirectory();
            bool exists = false;
            do
            {
                if (Directory.Exists(projectPath + dbPath))
                {
                    //Console.WriteLine("The directory exists.");
                    exists = true;
                }
                else
                {
                    Directory.CreateDirectory(projectPath + dbPath);
                    Console.WriteLine("The directory has been created.");
                    exists = true;
                }
            } while (!exists);

            string connectionString = $@"Data Source={projectPath}{dbPath}{dbName}";

            return connectionString;
        }
        internal static void Init()
        {
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding_session (
                session_id INTEGER PRIMARY KEY AUTOINCREMENT,
                start_time TEXT,
                end_time TEXT,
                time_span TEXT)";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                tableCmd.CommandText =
               @"CREATE TABLE IF NOT EXISTS goals (
                goal_id INTEGER PRIMARY KEY AUTOINCREMENT,
                goal_type TEXT,
                goal TEXT)";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS settings (
                setting_id INTEGER PRIMARY KEY AUTOINCREMENT,
                version INTEGER,
                test_mode INTEGER)";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                tableCmd.CommandText =
               @"CREATE TABLE IF NOT EXISTS daily_totals (
                daily_id INTEGER PRIMARY KEY AUTOINCREMENT,
                date TEXT,
                total_time TEXT,
                goal_met INTEGER,
                trophy_presented INTEGER)";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS weekly_totals (
                weekly_id INTEGER PRIMARY KEY AUTOINCREMENT,
                year_week INTEGER,
                total_time TEXT,
                goal_met INTEGER,
                trophy_presented INTEGER)";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows affected.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
                connection.Close();

            }
        }
        internal static Settings LoadData()


        {
            Settings settings = GetSettings();
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {

                if (settings.Version < 1)//So I can switch version and add more data if needed just add more if statements as needed.
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    try
                    {

                        ExecuteCommand(connection, "INSERT INTO goals (goal_id,goal_type,goal) VALUES ('1','Daily Goal','03:00:00')");
                        ExecuteCommand(connection, "INSERT INTO goals (goal_id,goal_type,goal) VALUES ('2','Weekly Goal','20:00:00')");
                        ExecuteCommand(connection, "UPDATE settings SET version = 1 WHERE setting_id = 1");


                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception);
                    }
                }


            }
            return settings;
        }
        internal static void ExecuteCommand(SqliteConnection connection, string commandText)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = commandText;
                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    Console.WriteLine($"{rowsAffected} rows affected.");
                    Console.ReadLine();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error executing query: " + ex.Message);
                    Console.ReadLine();
                }
            }
        }
        internal static CodingSession AddSession(CodingSession session)
        {
            int insertedId = -1;

            string connectionString = ConnectionString();
            string formattedTimeSpan = $"{(int)session.TimeSpan.TotalHours:D2}:{session.TimeSpan.Minutes:D2}:{session.TimeSpan.Seconds:D2}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"INSERT INTO coding_session (start_time,end_time,time_span) VALUES('{session.StartTime}','{session.EndTime}','{formattedTimeSpan}');
                    SELECT last_insert_rowid(); ";
                try
                {
                    insertedId = Convert.ToInt32(tableCmd.ExecuteScalar());

                    if (insertedId > 0)
                    {
                        session.Id = insertedId;
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

                return session;
            }

        }
        internal static void UpdateSession(CodingSession session)
        {
            string connectionString = ConnectionString();
            string formattedTimeSpan = $"{(int)session.TimeSpan.TotalHours:D2}:{session.TimeSpan.Minutes:D2}:{session.TimeSpan.Seconds:D2}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"UPDATE coding_session SET start_time = '{session.StartTime}',
                                                 end_time = '{session.EndTime}',
                                                 time_span = '{formattedTimeSpan}' 
                                                 WHERE session_id = '{session.Id}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows added.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        internal static void DeleteSession(CodingSession session)
        {
            string connectionString = ConnectionString();
            string formattedTimeSpan = $"{(int)session.TimeSpan.TotalHours:D2}:{session.TimeSpan.Minutes:D2}:{session.TimeSpan.Seconds:D2}";
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $@"DELETE from coding_session WHERE session_id = '{session.Id}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine($@"{rowsAffected} rows deleted.");
                    }
                    else
                    {
                        // The insert did not affect any rows (may indicate an issue).
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }
        internal static List<CodingSession> LoadSessions()
        {
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM coding_session";
                List<CodingSession> sessions = new List<CodingSession>();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        sessions.Add(
                        new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartTime = DateTime.ParseExact(reader.GetString(1), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EndTime = DateTime.ParseExact(reader.GetString(2), "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            TimeSpan = TimeSpan.Parse(reader.GetString(3), CultureInfo.InvariantCulture)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return sessions;
            }



        }
        internal static List<DailyTotals> LoadDailyTotals()
        {
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM daily_totals";
                List<DailyTotals> dailys = new List<DailyTotals>();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        dailys.Add(
                        new DailyTotals
                        {
                            DailyId = reader.GetInt32(0),
                            Date = DateOnly.ParseExact(reader.GetString(1), "MM-dd-yyyy"),
                            TotalTime = TimeSpan.Parse(reader.GetString(2), CultureInfo.InvariantCulture),
                            GoalMet = (reader.GetInt32(3) == 1),
                            TrophyPresented = (reader.GetInt32(4) == 1)
                        });
                    }

                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return dailys;
            }


        }
        internal static List<WeeklyTotals> LoadWeeklyTotals()
        {
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM weekly_totals";
                List<WeeklyTotals> weeklys = new List<WeeklyTotals>();
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        weeklys.Add(
                        new WeeklyTotals
                        {
                            WeeklyId = reader.GetInt32(0),
                            YearWeek = reader.GetInt32(1),
                            TotalTime = TimeSpan.Parse(reader.GetString(2), CultureInfo.InvariantCulture),
                            GoalMet = (reader.GetInt32(3) == 1),
                            TrophyPresented = (reader.GetInt32(4) == 1)
                        });
                    }

                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return weeklys;
            }
        }
        internal static DailyTotals AddUpdateDailyTotals(DailyTotals daily)
        {
            DailyTotals dailyFromDatabase = new DailyTotals();
            dailyFromDatabase.DailyId = 0;
            while (dailyFromDatabase.DailyId == 0)
            {
                string dateString = daily.Date.ToString("MM-dd-yyyy");
                int goalMet = daily.GoalMet ? 1 : 0;
                string connectionString = ConnectionString();
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"SELECT * FROM daily_totals WHERE date = '{dateString}'";
                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            dailyFromDatabase = new DailyTotals
                            {
                                DailyId = reader.GetInt32(0),
                                Date = DateOnly.ParseExact(reader.GetString(1), "MM-dd-yyyy"),
                                TotalTime = daily.TotalTime,
                                GoalMet = (reader.GetInt32(3) == 1),
                                TrophyPresented = (reader.GetInt32(4) == 1)
                            };

                        }
                        connection.Close();
                        if (daily.GoalMet == false && dailyFromDatabase.TrophyPresented == true)
                        {
                            dailyFromDatabase.TrophyPresented = false;
                        }
                        int trophyPresented = dailyFromDatabase.TrophyPresented ? 1 : 0;
                        dailyFromDatabase.TotalTime = daily.TotalTime;
                        dailyFromDatabase.GoalMet = daily.GoalMet;
                        connection.Open();
                        tableCmd.CommandText =
                        $"UPDATE daily_totals set total_time = '{daily.TotalTime}', goal_met = '{goalMet}', trophy_presented = '{trophyPresented}' WHERE daily_id = '{dailyFromDatabase.DailyId}'";
                        try
                        {
                            int rowsAffected = tableCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine($@"{rowsAffected} rows affected.");
                            }
                            else
                            {
                                // The insert did not affect any rows (may indicate an issue).
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            Console.ReadKey();
                        }
                        connection.Close();

                    }

                    else
                    {
                        connection.Close();
                        connection.Open();
                        tableCmd.CommandText = $@"INSERT INTO daily_totals (date, total_time, goal_met, trophy_presented) VALUES('{dateString}', '{daily.TotalTime}', '{goalMet}', '0')";
                        try
                        {
                            int rowsAffected = tableCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine($@"{rowsAffected} rows added.");
                            }
                            else
                            {
                                // The insert did not affect any rows (may indicate an issue).
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            Console.ReadKey();
                        }
                    }
                    connection.Close();
                }

            }
            return dailyFromDatabase;
        }
        internal static WeeklyTotals AddUpdateWeeklyTotals(WeeklyTotals weekly)
        {
            WeeklyTotals weeklyFromDatabase = new WeeklyTotals();
            weeklyFromDatabase.WeeklyId = 0;
            while (weeklyFromDatabase.WeeklyId == 0)
            {
                int yearWeek = weekly.YearWeek;
                int goalMet = weekly.GoalMet ? 1 : 0;
                string connectionString = ConnectionString();
                using (var connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"SELECT * FROM weekly_totals WHERE year_week = '{yearWeek}'";
                    SqliteDataReader reader = tableCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            weeklyFromDatabase = new WeeklyTotals
                            {
                                WeeklyId = reader.GetInt32(0),
                                YearWeek = reader.GetInt32(1),
                                TotalTime = weekly.TotalTime,
                                GoalMet = (reader.GetInt32(3) == 1),
                                TrophyPresented = (reader.GetInt32(4) == 1)
                            };

                        }
                        connection.Close();
                        if (weekly.GoalMet == false && weeklyFromDatabase.TrophyPresented == true)
                        {
                            weeklyFromDatabase.TrophyPresented = false;
                        }
                        int trophyPresented = weeklyFromDatabase.TrophyPresented ? 1 : 0;
                        weeklyFromDatabase.TotalTime = weekly.TotalTime;
                        weeklyFromDatabase.GoalMet = weekly.GoalMet;
                        connection.Open();
                        tableCmd.CommandText =
                        $"UPDATE weekly_totals set total_time = '{weekly.TotalTime}',goal_met = '{goalMet}', trophy_presented = '{trophyPresented}' WHERE weekly_id = '{weeklyFromDatabase.WeeklyId}'";
                        try
                        {
                            int rowsAffected = tableCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine($@"{rowsAffected} rows affected.");
                            }
                            else
                            {
                                // The insert did not affect any rows (may indicate an issue).
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            Console.ReadKey();
                        }
                        connection.Close();

                    }

                    else
                    {
                        connection.Close();
                        connection.Open();
                        tableCmd.CommandText = $@"INSERT INTO weekly_totals (year_week, total_time, goal_met, trophy_presented) VALUES('{yearWeek}', '{weekly.TotalTime}', '{goalMet}', '0')";
                        try
                        {
                            int rowsAffected = tableCmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                Console.WriteLine($@"{rowsAffected} rows added.");
                            }
                            else
                            {
                                
                            }
                        }
                        catch (Exception exception)
                        {
                            Console.WriteLine(exception);
                            Console.ReadKey();
                        }
                    }
                    connection.Close();
                }

            }
            return weeklyFromDatabase;
        }

        internal static void MarkPresented(DailyTotals daily)
        {
            int trophyPresented = daily.TrophyPresented ? 1 : 0;
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE daily_totals SET trophy_presented = '{trophyPresented}' WHERE daily_id = '{daily.DailyId}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        internal static void MarkPresented(WeeklyTotals weekly)
        {
            int trophPresented = weekly.TrophyPresented ? 1 : 0;
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"UPDATE weekly_totals SET trophy_presented = '{trophPresented}' WHERE weekly_id = '{weekly.WeeklyId}'";
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            }
        }

        internal static Settings GetSettings()
        {
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                Settings settings = new Settings();
                bool loaded = false;
                while (!loaded)
                {
                    var tableCmd = connection.CreateCommand();
                    tableCmd.CommandText =
                        $"SELECT * FROM settings WHERE setting_id = '1'";

                    SqliteDataReader reader = tableCmd.ExecuteReader();
                    if (reader.Read())
                    {
                        settings.ID = reader.GetInt32(0);
                        settings.Version = reader.GetInt32(1);
                        settings.TestMode = (reader.GetInt32(2) == 1);
                        loaded = true;
                    }

                    else
                    {
                        tableCmd = connection.CreateCommand();
                        tableCmd.CommandText =
                        $"INSERT INTO settings (setting_id,version,test_mode) VALUES ('1','0','1')";
                        tableCmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
                return settings;

            }
        }

        internal static void ToggleTest(Settings settings)
        {
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))

            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                if (settings.TestMode)
                {
                    settings.TestMode = false;
                    tableCmd.CommandText = $@"Update settings SET test_mode = '0' WHERE setting_id = '1'";

                }
                else if (!settings.TestMode)
                {
                    settings.TestMode = true;
                    tableCmd.CommandText = $@"Update settings SET test_mode = '1' WHERE setting_id = '1'";
                }
                else
                { 
                    Console.WriteLine("didn't get value.");
                    
                }
                try
                {
                    int rowsAffected = tableCmd.ExecuteNonQuery();
                        
                     Console.WriteLine($@"{rowsAffected} rows affected.");
                     
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    Console.ReadLine() ;
                }
                connection.Close();
            }
        }

        internal static Goals LoadSingleGoal(int goalType)
        {

            Goals goal = null;
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM goals";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetInt32(0) == goalType)
                        {
                            goal = new Goals
                            {
                                GoalId = reader.GetInt32(0),
                                GoalType = reader.GetString(1),
                                Goal = TimeSpan.Parse(reader.GetString(2), CultureInfo.InvariantCulture)
                            };
                        }
                        else { }
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();
                return goal;
            }


        }

        internal static List<Goals> LoadGoals()
        {
            List<Goals> goals = new List<Goals>();
            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM goals";

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        goals.Add(
                            new Goals
                            {
                                GoalId = reader.GetInt32(0),
                                GoalType = reader.GetString(1),
                                Goal = TimeSpan.Parse(reader.GetString(2), CultureInfo.InvariantCulture)
                            });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }
                connection.Close();

                return goals;
            }

        }

        internal static void UpdateGoal(Goals goal, TimeSpan goalTime)
        {

            string connectionString = ConnectionString();
            using (var connection = new SqliteConnection(connectionString))
            {

                connection.Open();
                var tableCmd = connection.CreateCommand();
                try
                {
                    ExecuteCommand(connection, $@"UPDATE goals SET goal = '{goalTime}' WHERE goal_id = '{goal.GoalId}'");
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }

            }

        }    
    }
}

