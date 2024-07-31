using System.Configuration;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Globalization;

namespace CodingTracker;

// Handles session data
public class Model
{
    static string connectionString = ConfigurationManager.ConnectionStrings["CodingSessionsDatabase"].ConnectionString;

    internal static void CreateDatabase()
    {
        var sql = @"CREATE TABLE IF NOT EXISTS coding_sessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Date TEXT,
                Language TEXT,
                StartTime TEXT,
                EndTime TEXT,
                Duration TEXT
                )";


        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sql);
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    internal static bool InsertSession(string? date, string? language, string startTime, string endTime, string duration)
    {
        bool sessionAdded = false;

        string? sql = $@"INSERT INTO coding_sessions(Date, Language, StartTime, EndTime, Duration)
                                    VALUES ('{date}', '{language}', '{startTime}', '{endTime}', '{duration}')";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                int rowsAffected = connection.Execute(sql);

                sessionAdded = rowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return sessionAdded;
    }

    internal static bool UpdateSession(string id, string column, string value, string? duration = null)
    {
        bool sessionUpdated = false;
        string? sql = @$"UPDATE coding_sessions
                    SET {column} = '{value}'
                    WHERE Id = {id}";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                var rowsAffected = connection.Execute(sql);

                // Calculates new updated duration
                if (column == "StartTime" || column == "EndTime")
                {
                    sql = @$"UPDATE coding_sessions
                        SET Duration = '{duration}'
                        Where Id = {id}";

                    connection.Execute(sql);
                }

                sessionUpdated = rowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return sessionUpdated;
        }
    }

    internal static bool DeleteSession(string? id)
    {
        bool sessionDeleted = false;
        string? sql;

        if (id == "*")
        {
            sql = $"DELETE FROM coding_sessions";
        }
        else
        {
            sql = $"DELETE FROM coding_sessions WHERE Id = {id}";
        }

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                int rowsAffected = connection.Execute(sql);

                sessionDeleted = rowsAffected == 0 ? false : true;
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return sessionDeleted;
    }

    internal static List<CodingSession> FilterSessions(string? startDate, string? endDate)
    {
        var sessionsList = new List<CodingSession>();
        var sql = $@"SELECT * FROM coding_sessions 
                    WHERE Date 
                    BETWEEN '{startDate}' and '{endDate}'";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    sessionsList.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateOnly.ParseExact(reader.GetString(1), "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None),
                        Language = reader.GetString(2),
                        StartTime = TimeOnly.ParseExact(reader.GetString(3), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        EndTime = TimeOnly.ParseExact(reader.GetString(4), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        Duration = TimeSpan.ParseExact(reader.GetString(5), "h\\:mm\\:ss", new CultureInfo("en-US"), TimeSpanStyles.None)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return sessionsList;
        }
    }

    internal static List<CodingSession> SortSessions(string optionSelected)
    {
        List<CodingSession> sessionsList = new();

        var sql = $@"SELECT * 
                    FROM coding_sessions
                    ORDER BY Date {optionSelected}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    sessionsList.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateOnly.ParseExact(reader.GetString(1), "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None),
                        Language = reader.GetString(2),
                        StartTime = TimeOnly.ParseExact(reader.GetString(3), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        EndTime = TimeOnly.ParseExact(reader.GetString(4), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        Duration = TimeSpan.ParseExact(reader.GetString(5), "h\\:mm\\:ss", new CultureInfo("en-US"), TimeSpanStyles.None)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return sessionsList;
        }
    }

    internal static List<CodingSession> FetchSessions()
    {
        var sql = "SELECT * FROM coding_sessions";
        var sessionsList = new List<CodingSession>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    sessionsList.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateOnly.ParseExact(reader.GetString(1), "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None),
                        Language = reader.GetString(2),
                        StartTime = TimeOnly.ParseExact(reader.GetString(3), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        EndTime = TimeOnly.ParseExact(reader.GetString(4), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        Duration = TimeSpan.ParseExact(reader.GetString(5), "h\\:mm\\:ss", new CultureInfo("en-US"), TimeSpanStyles.None)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return sessionsList;
        }
    }

    internal static string? FetchValue(string? id, string? column, string? database)
    {
        var sql = $"SELECT {column} from {database} WHERE Id = {id}";
        string? result = "";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                result = connection.ExecuteScalar(sql)?.ToString();
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return result;
    }
}

// Handles goal data
public class GoalsModel
{
    static string connectionString = ConfigurationManager.ConnectionStrings["CodingGoalsDatabase"].ConnectionString;
    static string connectionString2 = ConfigurationManager.ConnectionStrings["CodingSessionsDatabase"].ConnectionString;

    internal static void CreateGoalsDatabase()
    {
        var sql = @"CREATE TABLE IF NOT EXISTS coding_goals (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Language TEXT,
                    Percentage TEXT,
                    TotalHours TEXT,
                    HoursLeft TEXT
                    )";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sql);
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }

    internal static bool InsertGoal(string language, string totalHours)
    {
        bool goalAdded = false;
        var sql = $@"INSERT INTO coding_goals(Language, Percentage, TotalHours, HoursLeft)
                    VALUES ('{language}', 0, '{totalHours}', 0)";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                int rowsAffected = connection.Execute(sql);
                goalAdded = rowsAffected == 1 ? true : false;
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return goalAdded;
    }

    internal static bool DeleteGoal(string? id)
    {
        bool goalDeleted = false;
        string? sql;

        try
        {
            if (id == "*")
            {
                sql = $"DELETE FROM coding_goals";
            }
            else
            {
                sql = $"DELETE FROM coding_goals WHERE Id = {id}";
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                int rowsAffected = connection.Execute(sql);

                goalDeleted = rowsAffected == 0 ? false : true;
            }
        }
        catch (Exception ex)
        {
            Views.ShowError(ex.Message);
        }
        return goalDeleted;
    }

    internal static List<CodingGoal> FetchGoals()
    {
        var sql = "SELECT * FROM coding_goals";
        var goalsTable = new List<CodingGoal>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    goalsTable.Add(
                        new CodingGoal
                        {
                            Id = reader.GetInt32(0),
                            Language = reader.GetString(1),
                            Percentage = reader.GetString(2),
                            TotalHours = int.Parse(reader.GetString(3)),
                            HoursLeft = double.Parse(reader.GetString(4)),
                        });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return goalsTable;
    }

    // Retrieves list of sessions for specific language used in a goal 
    internal static List<CodingSession> GetFilteredLanguageSessions(CodingGoal goal)
    {
        List<CodingSession> filteredList = new();

        var sql = $@"SELECT * FROM coding_sessions
                    WHERE Language = '{goal.Language}'";

        using (var connection = new SqliteConnection(connectionString2))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    filteredList.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateOnly.ParseExact(reader.GetString(1), "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None),
                        Language = reader.GetString(2),
                        StartTime = TimeOnly.ParseExact(reader.GetString(3), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        EndTime = TimeOnly.ParseExact(reader.GetString(4), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        Duration = TimeSpan.ParseExact(reader.GetString(5), "h\\:mm\\:ss", new CultureInfo("en-US"), TimeSpanStyles.None)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        return filteredList;
    }

    // Updates the database with new goal status
    internal static void UpdateGoalPercentage(CodingGoal goal, string percentage, string hoursLeft)
    {
        var sql = $@"UPDATE coding_goals
                    SET Percentage = '{percentage}', HoursLeft = '{hoursLeft}'
                    WHERE Id = {goal.Id}";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();
                connection.Execute(sql);
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}

// Handles report data
public class ReportModel
{
    static string connectionString = ConfigurationManager.ConnectionStrings["CodingSessionsDatabase"].ConnectionString;

    internal static List<CodingSession> GetLanguagesUsed()
    {
        var sql = "SELECT DISTINCT Language FROM coding_sessions";
        var languages = new List<CodingSession>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    languages.Add(
                    new CodingSession
                    {
                        Language = reader.GetString(0),
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return languages;
    }

    internal static List<CodingSession> GetFilteredLanguageSessions(CodingSession session)
    {
        List<CodingSession> filteredList = new();

        var sql = $@"SELECT * FROM coding_sessions
                    WHERE Language = '{session.Language}'";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    filteredList.Add(
                    new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateOnly.ParseExact(reader.GetString(1), "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None),
                        Language = reader.GetString(2),
                        StartTime = TimeOnly.ParseExact(reader.GetString(3), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        EndTime = TimeOnly.ParseExact(reader.GetString(4), "h\\:mm\\:ss tt", new CultureInfo("en-US"), DateTimeStyles.None),
                        Duration = TimeSpan.ParseExact(reader.GetString(5), "h\\:mm\\:ss", new CultureInfo("en-US"), TimeSpanStyles.None)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return filteredList;
    }

    internal static List<CodingSession> GetMonthlySessions(string month)
    {
        List<CodingSession> monthlyDates = new();

        var sql = $@"SELECT DISTINCT Date 
                    FROM coding_sessions
                    WHERE Date > {month}";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            try
            {
                var reader = connection.ExecuteReader(sql);

                while (reader.Read())
                {
                    monthlyDates.Add(
                    new CodingSession
                    {
                        Date = DateOnly.ParseExact(reader.GetString(0), "yyyy/MM/dd", new CultureInfo("en-US"), DateTimeStyles.None)
                    });
                }
            }
            catch (Exception ex)
            {
                Views.ShowError(ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        return monthlyDates;
    }
}

public class CodingSession
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string? Language { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public TimeSpan Duration { get; set; }
}

public class CodingGoal
{
    public int Id { get; set; }
    public string? Language { get; set; }
    public string? Percentage { get; set; }
    public int TotalHours { get; set; }
    public double HoursLeft { get; set; }
}