using CodingTracker.Database;
using CodingTracker.Models;
using CodingTracker.Services;
using Dapper;
using System.Text;

namespace CodingTracker.DAO;

public class CodingSessionDAO
{
    private readonly DatabaseContext _dbContext;
    private CodingGoalDAO _codingGoalDAO;

    public CodingSessionDAO(DatabaseContext context, CodingGoalDAO codingGoalDAO)
    {
        _dbContext = context;
        _codingGoalDAO = codingGoalDAO;
    }

    public bool InsertSessionAndUpdateGoals(CodingSessionModel session)
    {
        using (var connection = _dbContext.GetNewDatabaseConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Insert the new session
                    string sessionSql = @"
                        INSERT INTO tb_CodingSessions (DateCreated, DateUpdated, SessionDate, Duration, StartTime, EndTime)
                        VALUES (@DateCreated, @DateUpdated, @SessionDate, @Duration, @StartTime, @EndTime);
                        SELECT last_insert_rowid();";

                    int newRecordID = connection.ExecuteScalar<int>(sessionSql, session, transaction: transaction);
                    if (newRecordID <= 0)
                    {
                        transaction.Rollback();
                        return false;
                    }

                    // Update the goals based on the new session's duration
                    var goals = _codingGoalDAO.GetInProgressCodingGoals();
                    foreach (var goal in goals)
                    {
                        
                        if (DateTime.Parse(goal.DateCreated) > DateTime.Parse(session.SessionDate))
                            continue;

                        goal.UpdateProgress(TimeSpan.Parse(session.Duration));
                        if (!_codingGoalDAO.UpdateCodingGoal(goal, connection, transaction))
                        {
                            transaction.Rollback();
                            return false;
                        }
                    }

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();  // Roll back on any error
                    Utilities.DisplayExceptionErrorMessage("Transaction failed: ", ex.Message);
                    return false;
                }
            }
        }
    }


    public List<CodingSessionModel> GetAllSessionRecords()
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "SELECT * FROM tb_CodingSessions";
                var sessions = connection.Query<CodingSessionModel>(sql).ToList();
                return sessions;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving sessions", ex.Message);
            return new List<CodingSessionModel>();  // Return an empty list in case of error
        }
    }

    public List<CodingSessionModel> ExecutetGetSessionsQuery(string sqlstring)
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = sqlstring;
                var sessions = connection.Query<CodingSessionModel>(sql).ToList();
                return sessions;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving sessions", ex.Message);
            return new List<CodingSessionModel>();  // Return an empty list in case of error
        }
    }

    public List<CodingSessionModel> GetSessionsRecords(TimePeriod? periodFilter, int? numOfPeriods, List<(CodingSessionModel.EditableProperties, SortDirection, int)> orderByOptions)
    {
        var sbSql = new StringBuilder("SELECT * FROM tb_CodingSessions");

        if (periodFilter.HasValue && numOfPeriods.HasValue)
        {
            string timePeriodValue = $"{numOfPeriods.Value * Utilities.GetDaysMultiplier(periodFilter.Value)} days";
            sbSql.Append($" WHERE SessionDate >= date('now', '-{timePeriodValue}')");
        }

        if (orderByOptions.Any())
        {
            sbSql.Append(" ORDER BY ");
            sbSql.Append(string.Join(", ", orderByOptions.OrderBy(o => o.Item3)
                .Select(o => $"{o.Item1} {(o.Item2 == SortDirection.ASC ? "ASC" : "DESC")}")));
        }

        return ExecutetGetSessionsQuery(sbSql.ToString());
    }

    public bool DeleteAllSessions()
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "DELETE FROM tb_CodingSessions";
                int result = connection.Execute(sql);
                return result > 0;  // Returns true if any rows were affected
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error deleting sessions", ex.Message);
            return false;
        }
    }

    public bool DeleteSessionRecord(int sessionId)
    {
        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                string sql = "DELETE FROM tb_CodingSessions WHERE Id = @SessionId";
                int result = connection.Execute(sql, new { SessionId = sessionId });
                return result > 0;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage($"Error deleting session with ID {sessionId}", ex.Message);
            return false;
        }
    }

    public SessionStatistics GetSessionStatistics(TimePeriod period, int numberOfPeriods)
    {
        string sql = $@"
        SELECT 
            COUNT(*) AS TotalSessions, 
            AVG(Duration) AS AverageDuration 
        FROM tb_CodingSessions
        WHERE SessionDate >= date('now', '-{Utilities.GetDaysMultiplier(period) * numberOfPeriods} days')";

        try
        {
            using (var connection = _dbContext.GetNewDatabaseConnection())
            {
                var result = connection.QuerySingleOrDefault<SessionStatistics>(sql);
                if (result == null)
                {
                    return new SessionStatistics { TotalSessions = 0, AverageDuration = 0.0 };
                }

                return result;
            }
        }
        catch (Exception ex)
        {
            Utilities.DisplayExceptionErrorMessage("Error retrieving session statistics", ex.Message);
            return new SessionStatistics();  // Return an empty/default SessionStatistics in case of error
        }
    }
}
