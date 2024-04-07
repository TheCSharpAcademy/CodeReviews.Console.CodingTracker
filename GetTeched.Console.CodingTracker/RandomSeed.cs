using Dapper;
using System.Configuration;
using System.Data.SQLite;
using System.Globalization;

namespace coding_tracker;

internal class RandomSeed
{
    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    CodingSession session = new();
    CodingGoal goal = new();

    internal void GenerateRandomData()
    {
        Random random = new Random();

        int loggedDays; int day; int month; int year = 23; int hours; int minutes; int seconds; string startDate; string endDate; string duration;

        loggedDays = random.Next(30, 260);


        for (int i = 0; i < loggedDays; i++)
        {
            day = random.Next(1, 28);
            month = random.Next(1, 12);
            hours = random.Next(0, 23);
            minutes = random.Next(0, 59);
            seconds = random.Next(0, 59);

            startDate = String.Format("{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", day, month, year, hours, minutes, seconds);
            DateTime parseStartDate = DateTime.Parse(startDate);

            if (hours >= 20)
            {
                day = day + 1;
                hours = random.Next(0, 23);
                minutes = random.Next(0, 59);
                seconds = random.Next(0, 59);
            }
            else
            {
                hours = hours + random.Next(1, 3);
                minutes = random.Next(0, 59);
                seconds = random.Next(0, 59);
            }
            endDate = String.Format("{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", day, month, year, hours, minutes, seconds);
            DateTime parseEndDate = DateTime.Parse(endDate);
            duration = Duration(startDate, endDate);

            RandomSeedAdd(parseStartDate.ToString("yyyy-MM-dd"), parseStartDate.ToString("yyyy-MM-dd HH:mm:ss"), parseEndDate.ToString("yyyy-MM-dd HH:mm:ss"), duration);

        }

        for (int i = 0; i < 30; i++)
        {
            day = random.Next(1, 28);
            month = random.Next(1, 12);
            hours = random.Next(30, 300);
            int completed = random.Next(0, 1);
            startDate = String.Format("{0:00}-{1:00}-{2:00}", day, month, year);
            DateTime parseStartDate = DateTime.Parse(startDate);

            RandomSeedGoalAdd(parseStartDate.ToString("yyyy-MM-dd"), hours, completed);
        }
    }

    internal string Duration(string sessionStart, string sessionEnd)
    {
        DateTime startTime = DateTime.ParseExact(sessionStart, "dd-MM-yy HH:mm:ss", new CultureInfo("en-GB"));
        DateTime endTime = DateTime.ParseExact(sessionEnd, "dd-MM-yy HH:mm:ss", new CultureInfo("en-GB"));

        TimeSpan duration = endTime.Subtract(startTime);
        return duration.TotalSeconds.ToString();
    }

    internal void RandomSeedAdd(string date, string startDate, string endDate, string duration)
    {
        session.Date = date;
        session.StartTime = startDate;
        session.EndTime = endDate;
        session.Duration = duration;

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO Coding_Session (Date, StartTime, EndTime, Duration) VALUES (@Date, @StartTime, @EndTime, @Duration)";
            connection.Execute(sqlQuery, new { session.Date, session.StartTime, session.EndTime, session.Duration });
        }
    }

    internal void RandomSeedGoalAdd(string date, int hours, int completed)
    {
        goal.Date = date;
        goal.Hours = hours;
        goal.Completed = completed;

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO ALL_Coding_Goals (Date, Hours, Completed) VALUES (@Date, @Hours, @Completed)";
            connection.Execute(sqlQuery, new { goal.Date, goal.Hours, goal.Completed });
        }

    }
}
