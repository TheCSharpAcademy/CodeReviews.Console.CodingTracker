using CodingTracker.kraven88.Models;

namespace CodingTracker.kraven88.Data;

internal class DataAccess
{
    private SqliteDB db;

	public DataAccess()
	{
		db = new SqliteDB();
	}

    public void DeleteSession(CodingSession session)
    {
        var sql = "DELETE FROM Sessions WHERE Id = @id";

        db.DeleteById(sql, session);
    }

	public List<CodingSession> LoadAllSessions()
	{
        var sql = "Select * FROM Sessions " +
            "ORDER BY EndDate";

        return db.LoadData(sql);
	}

	public List<CodingSession> LoadLastSession()
	{
        var sql = "SELECT * FROM Sessions " +
            "ORDER BY EndDate DESC " +
            "LIMIT 1";

        return db.LoadData(sql);
    }

    public List<CodingSession> LoadSelectedSessions(CodingSession session)
    {
        var sql = "SELECT * FROM Sessions " +
            "WHERE StartDate BETWEEN @start AND @end " +
            "ORDER BY EndDate";

        return db.LoadData(sql, session);
    }

    public void SaveSession(CodingSession session)
	{
        var sql = $@"INSERT INTO Sessions (StartDate, EndDate)
            VALUES (@start, @end)";

        db.SaveData(sql, session);
    }
}
