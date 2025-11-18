using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    public class Database(string connectionString) : IDisposable
    {
        private readonly SqliteConnection connection = new(connectionString);

        public void Migrate()
        {
            connection.Open();

            using SqliteCommand tableCmd = connection.CreateCommand();

            tableCmd.CommandText =
            @"CREATE TABLE IF NOT EXISTS coding_sessions (
        id INTEGER PRIMARY KEY AUTOINCREMENT,
        start_time TEXT,
        end_time TEXT,
        duration REAL
        )";

            _ = tableCmd.ExecuteNonQuery();

            tableCmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS coding_goals (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        start_time TEXT,
                        end_time TEXT,
                        duration INTEGER,
                        is_finished INTEGER,
                        is_achieved INTEGER
                        );";

            _ = tableCmd.ExecuteNonQuery();

            connection.Close();
        }

        public void Save(CodingSession session)
        {
            connection.Open();
            string query = "insert into coding_sessions (start_time, end_time, duration) values(@Start, @End, @Duration)";

            _ = connection.Execute(query, session);
        }

        public List<CodingSession> Get(Enums.SessionOrder? order = null)
        {
            string query = "select id, start_time as start, end_time as end, duration from coding_sessions";
            switch (order)
            {
                case Enums.SessionOrder.Ascending:
                    query += " order by id asc";
                    break;
                case Enums.SessionOrder.Descending:
                    query += " order by id desc";
                    break;
                default:
                    break;
            }

            connection.Open();
            List<CodingSession> records = [.. connection.Query<CodingSession>(query)];
            return records;
        }

        public void Update(CodingSession session)
        {
            connection.Open();
            string query = "update coding_sessions set start_time = @Start, end_time = @End, duration = @Duration where id = @Id";
            _ = connection.Execute(query, session);
        }

        public void Delete(CodingSession session)
        {
            connection.Open();
            string query = "delete from coding_sessions where id = @Id";
            _ = connection.Execute(query, session);

        }

        public List<FilteredCodingSession> GetGroupByDay(Enums.SessionOrder? order = null)
        {
            string query = "select strftime('%Y-%j', start_time) as filterid, sum(duration) as duration from coding_sessions group by filterid";
            switch (order)
            {
                case Enums.SessionOrder.Ascending:
                    query += " order by filterid asc";
                    break;
                case Enums.SessionOrder.Descending:
                    query += " order by filterid desc";
                    break;
                default:
                    break;
            }
            connection.Open();
            List<FilteredCodingSession> records = [.. connection.Query<FilteredCodingSession>(query)];
            return records;
        }

        public List<FilteredCodingSession> GetGroupByWeek(Enums.SessionOrder? order = null)
        {
            string query = "select printf('%s-%02d', strftime('%Y', start_time), cast(strftime('%W', start_time) as integer) + 1) as filterid, sum(duration) as duration from coding_sessions group by filterid";
            switch (order)
            {
                case Enums.SessionOrder.Ascending:
                    query += " order by filterid asc";
                    break;
                case Enums.SessionOrder.Descending:
                    query += " order by filterid desc";
                    break;
                default:
                    break;
            }
            connection.Open();
            List<FilteredCodingSession> records = [.. connection.Query<FilteredCodingSession>(query)];
            return records;
        }

        public List<FilteredCodingSession> GetGroupByYear(Enums.SessionOrder? order = null)
        {
            string query = "select strftime('%Y', start_time) as filterid, sum(duration) as duration from coding_sessions group by filterid";
            switch (order)
            {
                case Enums.SessionOrder.Ascending:
                    query += " order by filterid asc";
                    break;
                case Enums.SessionOrder.Descending:
                    query += " order by filterid desc";
                    break;
                default:
                    break;
            }
            connection.Open();
            List<FilteredCodingSession> records = [.. connection.Query<FilteredCodingSession>(query)];
            return records;
        }

        public Report GetReport()
        {
            string query = @"SELECT
  COUNT(CASE WHEN date(start_time) = date('now', 'localtime') THEN 1 END) AS counttoday,
  COALESCE(SUM(CASE WHEN date(start_time) = date('now', 'localtime') THEN duration END), 0) AS totaltoday,

  COUNT(CASE WHEN strftime('%Y-%W', start_time) = strftime('%Y-%W', 'now', 'localtime') THEN 1 END) AS countweek,
  COALESCE(SUM(CASE WHEN strftime('%Y-%W', start_time) = strftime('%Y-%W', 'now', 'localtime') THEN duration END), 0) AS totalweek,

  COUNT(CASE WHEN strftime('%Y', start_time) = strftime('%Y', 'now', 'localtime') THEN 1 END) AS countyear,
  COALESCE(SUM(CASE WHEN strftime('%Y', start_time) = strftime('%Y', 'now', 'localtime') THEN duration END), 0) AS totalyear,

  COUNT(*) AS count,
  COALESCE(SUM(duration), 0) AS total

FROM coding_sessions;
";
            connection.Open();
            Report report = connection.QuerySingle<Report>(query);
            return report;
        }

        public CodingGoal? GetActiveCodingGoal()
        {
            string query = @"select id, start_time as Start, end_time as End, duration as Duration, is_finished as IsFinished from coding_goals where is_finished == 0 limit 1";
            connection.Open();
            CodingGoal? codingGoal = connection.QuerySingleOrDefault<CodingGoal>(query);
            return codingGoal;
        }

        public void SaveCodingGoal(CodingGoal codingGoal)
        {
            connection.Open();
            string query = "insert into coding_goals (start_time, end_time, duration, is_finished, is_achieved) values(@Start, @End, @Duration, @IsFinished, @IsAchieved)";
            Console.WriteLine($"{codingGoal.Start} {codingGoal.End} {codingGoal.Duration}");

            _ = connection.Execute(query, codingGoal);
        }

        public void UpdateCodingGoal(CodingGoal codingGoal)
        {
            connection.Open();
            string query = "update coding_goals set start_time = @Start, end_time = @End, duration = @Duration, is_finished = @IsFinished, is_achieved = @IsAchieved where id = @Id";
            _ = connection.Execute(query, codingGoal);
        }

        public List<CodingSession> GetAllCodingSessionsBetweenDates(DateTime start, DateTime end)
        {
            string query = "select id, start_time as start, end_time as end, duration from coding_sessions where start_time >= @Start and end_time <= @End";
            connection.Open();
            var parameter = new { Start = start, End = end };
            List<CodingSession> records = [.. connection.Query<CodingSession>(query, parameter)];
            return records;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
