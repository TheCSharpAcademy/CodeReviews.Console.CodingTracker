using CodingTracker.Dejmenek.DataAccess.Interfaces;
using CodingTracker.Dejmenek.Models;
using Dapper;
using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker.Dejmenek.DataAccess.Repositories
{
    public class CodingSessionRepository : ICodingSessionRepository
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["CodingTracker"].ConnectionString;

        public void AddCodingSession(string startDateTime, string endDateTime, int duration)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    INSERT INTO CodingSessions (StartDateTime, EndDateTime, Duration)
                    VALUES (@StartDate, @EndDate, @Duration);
                ";

                connection.Execute(sql, new
                {
                    StartDate = startDateTime,
                    EndDate = endDateTime,
                    Duration = duration
                });
            }
        }

        public void DeleteCodingSession(int id)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    DELETE FROM CodingSessions
                    WHERE Id = @Id;
                ";

                connection.Execute(sql, new
                {
                    Id = id
                });
            }
        }

        public List<CodingSession> GetAllCodingSessions()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    SELECT * FROM CodingSessions;
                ";

                return connection.Query<CodingSession>(sql).ToList();
            }
        }

        public IEnumerable<(string year, int durationSum)> GetYearlyCodingSessionReport()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    SELECT strftime('%Y', StartDateTime) AS Year, SUM(
	                    CASE
	                        WHEN strftime('%Y', StartDateTime) = strftime('%Y', EndDateTime) THEN Duration
	                        ELSE Duration - ROUND((JULIANDAY(EndDateTime, 'start of month', 'start of day') - JULIANDAY(StartDateTime)) * 24 * 60)
	                    END
                    )
                    FROM CodingSessions
                    GROUP BY Year;
                ";

                return connection.Query<(string year, int durationSum)>(sql);
            }
        }

        public IEnumerable<(string year, string month, int durationSum)> GetMonthlyCodingSessionReport()
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                string sql = @"
                    SELECT strftime('%Y', StartDateTime) AS Year ,strftime('%m', StartDateTime) AS Month, SUM(
	                    CASE
	                        WHEN strftime('%m', StartDateTime) = strftime('%m', EndDateTime) THEN Duration
	                        ELSE Duration - ROUND(JULIANDAY(EndDateTime, 'start of month', 'start of day') - JULIANDAY(StartDateTime) * 24 * 60)
	                    END
                    )
                    FROM CodingSessions
                    GROUP BY Year, Month;
                ";

                return connection.Query<(string year, string month, int durationSum)>(sql);
            }
        }
    }
}
