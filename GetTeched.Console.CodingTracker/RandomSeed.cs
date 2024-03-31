using System;
using System.Collections.Generic;
using System.Configuration;
using Dapper;
using System.Data.SQLite;
using Microsoft.Data.Sqlite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using System.Globalization;

namespace coding_tracker;

internal class RandomSeed
{
    readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    static int id = 0;
    static string startTime = "";
    static string endTime = "";
    static string duration = "";

    CodingSession session = new()
    {
        Id = id,
        StartTime = startTime,
        EndTime = endTime,
        Duration = duration
    };

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

            startDate = String.Format("{0:00}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}",day, month, year, hours, minutes, seconds);

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

            duration = Duration(startDate, endDate);

            RandomSeedAdd(startDate, endDate, duration);

        }
    }

    internal string Duration(string sessionStart, string sessionEnd)
    {
        DateTime startTime = DateTime.ParseExact(sessionStart, "dd-MM-yy HH:mm:ss", new CultureInfo("en-GB"));
        DateTime endTime = DateTime.ParseExact(sessionEnd, "dd-MM-yy HH:mm:ss", new CultureInfo("en-GB"));

        TimeSpan duration = endTime.Subtract(startTime);
        return duration.TotalSeconds.ToString();
    }

    internal void RandomSeedAdd(string startDate, string endDate, string duration)
    {
        session.StartTime = startDate;
        session.EndTime = endDate;
        session.Duration = duration;

        using (var connection = new SQLiteConnection(connectionString))
        {
            connection.Open();
            string sqlQuery = "INSERT INTO Coding_Session (StartTime, EndTime, Duration) VALUES (@StartTime, @EndTime, @Duration)";
            connection.Execute(sqlQuery, new { session.StartTime, session.EndTime, session.Duration });
        }
    }
}
