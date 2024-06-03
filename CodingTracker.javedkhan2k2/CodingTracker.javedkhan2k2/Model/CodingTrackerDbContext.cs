using Dapper;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace CodingTracker.Models;

internal class CodingTrackerDbContext
{
    private string ConnectionString { get; init; }

    public CodingTrackerDbContext(string connectionString)
    {
        ConnectionString = connectionString;
        InitDatabase();
        //SeedData();
    }

    private void InitDatabase()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"Create Table If Not Exists CodingSessions (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StartTime TEXT NOT NULL,
                EndTime TEXT NOT NULL,
                Duration INTEGER NOT NULL
            )";
            connection.Execute(sql);
        }
        //SeedData();
    }

    private void SeedData()
    {
        var random = new Random();

        int habitId;
        int habitQuantity;
        string? sDate;
        string? eDate;
        DateTime startDate = new DateTime(2022, 1, 1);
        int range = (DateTime.Today - startDate).Days;
        for (int i = 0; i < 1000; i++)
        {
            habitId = random.Next(1, 4);
            habitQuantity = random.Next(1, 13);
            // https://stackoverflow.com/questions/194863/random-date-in-c-sharp
            var tempDate = startDate
                .AddDays(random.Next(range))
                .AddHours(random.Next(1, 12))
                .AddMinutes(random.Next(1, 40));
            sDate = tempDate.ToString("yyyy-MM-dd HH:mm:ss");
            var tempDate2 = tempDate
                    .AddHours(random.Next(1, 5))
                    .AddMinutes(random.Next(1, 30));
            eDate = tempDate2.ToString("yyyy-MM-dd HH:mm:ss");
            AddSessionCode(new CodingSessionDto { StartTime = sDate, EndTime = eDate, Duration = (long)tempDate2.Subtract(tempDate).TotalSeconds });

        }
    }

    internal bool AddSessionCode(CodingSessionDto codingSession)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"insert into CodingSessions (StartTime, EndTime, Duration)
                        Values 
                            (@StartTime, @EndTime, @Duration)
                    ";
            int result = connection.Execute(sql, codingSession);
            return result == 0 ? false : true;
        }
    }

    internal bool UpdateSessionCode(int id, CodingSessionDto codingSession)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"Update CodingSessions
                    Set 
                        StartTime = @StartTime,
                        EndTime = @EndTime,
                        Duration = @Duration
                    where
                        Id = @Id;
                  ";
            var parameters = new { Id = id, StartTime = codingSession.StartTime, EndTime = codingSession.EndTime, Duration = codingSession.Duration };
            int result = connection.Execute(sql, parameters);
            return result == 0 ? false : true;
        }
    }

    internal bool DeleteSessionCode(int id)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"Delete from CodingSessions
                            Where Id = @Id
                       ";
            var parameters = new { Id = id };
            int result = connection.Execute(sql, parameters);
            return result == 0 ? false : true;
        }
    }

    internal IEnumerable<CodingSession>? GetAllCodingSessions()
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var sql = "SELECT Id, StartTime, EndTime, Duration AS DurationInSeconds FROM CodingSessions";
            IEnumerable<CodingSession>? codingSessions = connection.Query<CodingSession>(sql);
            return codingSessions;
        }
    }

    internal CodingSession? GetCodingSessionById(int id)
    {
        CodingSession? codingSession;
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @"SELECT Id, StartTime, EndTime, Duration AS DurationInSeconds FROM CodingSessions Where Id = @Id";
            var parameters = new { Id = id };
            codingSession = connection.QueryFirstOrDefault<CodingSession>(sql, parameters);
        }

        return codingSession;
    }

    #region Reports

    internal IEnumerable<YearlyReport>? GetYearlyReport(ReportDto dailyReportDto)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"
                select 
                    strftime('%Y', StartTime) Year,  
                    ROUND(sum(Duration/3600.0),4) as CodingTime,
                    ROUND(avg(Duration/3600.0),4) as AverageTime,
	                COUNT(Duration) as TotalSessions
                from CodingSessions 
                where 
                    strftime('%Y',StartTime) >= @StartDate 
                    and 
                    strftime('%Y',StartTime) <= @EndDate
                group by 
                    strftime('%Y',StartTime)
                order by
                    strftime('%Y',StartTime) {dailyReportDto.Sort}
                ";
            IEnumerable<YearlyReport>? yearlyReport = connection
                                        .Query<YearlyReport>(sql, dailyReportDto);
            return yearlyReport;
        }
    }

    internal IEnumerable<MonthlyReport>? GetMonthlyReport(ReportDto dailyReportDto)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"
                select 
                    strftime('%Y', StartTime) Year,
                    strftime('%m', StartTime) Month, 
                    ROUND(sum(Duration/3600.0),4) as CodingTime,
                    ROUND(avg(Duration/3600.0),4) as AverageTime,
	                COUNT(Duration) as TotalSessions
                from CodingSessions 
                where 
                    date(StartTime) >= @StartDate 
                    and 
                    date(StartTime) <= @EndDate
                group by 
                    strftime('%Y',StartTime),  
                    strftime('%m', StartTime)
                order by
                    strftime('%Y',StartTime) {dailyReportDto.Sort},
                    strftime('%m',StartTime) {dailyReportDto.Sort}
                ";
            IEnumerable<MonthlyReport>? monthlyReport = connection
                                        .Query<MonthlyReport>(sql, dailyReportDto);
            return monthlyReport;
        }
    }

    internal IEnumerable<WeeklyReport>? GetWeeklyReport(ReportDto dailyReportDto)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"
                select 
                    strftime('%Y', StartTime) Year,
                    strftime('%W', StartTime) Week, 
                    ROUND(sum(Duration/3600.0),4) as CodingTime,
                    ROUND(avg(Duration/3600.0),4) as AverageTime,
	                COUNT(Duration) as TotalSessions
                from CodingSessions 
                where 
                    date(StartTime) >= @StartDate 
                    and 
                    date(StartTime) <= @EndDate
                group by 
                    strftime('%Y',StartTime),  
                    strftime('%W', StartTime)
                order by
                    strftime('%Y',StartTime) {dailyReportDto.Sort},
                    strftime('%W',StartTime) {dailyReportDto.Sort}
                ";
            IEnumerable<WeeklyReport>? weeklyReport = connection
                                        .Query<WeeklyReport>(sql, dailyReportDto);
            return weeklyReport;
        }
    }

    internal IEnumerable<DailyReport>? GetDailyReport(ReportDto dailyReportDto)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();
            var sql = @$"
                select 
                    date(StartTime) as Date, 
                    ROUND(sum(Duration/3600.0),4) as CodingTime,
                    ROUND(avg(Duration/3600.0),4) as AverageTime,
	                COUNT(Duration) as TotalSessions
                from CodingSessions 
                where 
                    date(StartTime) >= @StartDate 
                    and 
                    date(StartTime) <= @EndDate
                group by 
                    date(StartTime)
                order by
                    StartTime {dailyReportDto.Sort}
                ";
            IEnumerable<DailyReport>? dailyReport = connection
                                        .Query<DailyReport>(sql, dailyReportDto);
            return dailyReport;
        }
    }

    #endregion // Reports

}