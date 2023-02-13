using CodingTracker.kraven88.Models;
using System.Data.SQLite;

namespace CodingTracker.kraven88.Data;

internal class DataAccess
{
    private SqliteDB db;

	public DataAccess()
	{
		db = new SqliteDB();
	}

	public List<CodingSession> LoadAllSessions()
	{
        var sql = "Select * FROM Sessions ORDER BY EndDate";
        return db.LoadData(sql);
	}

	public List<CodingSession> LoadLastSession()
	{
        // TODO
        throw new NotImplementedException();
    }

    public List<CodingSession> LoadSelectedSessions()
    {
        // TODO
        throw new NotImplementedException();
    }

    public void SaveSession(CodingSession session)
	{
        var sql = $@"INSERT INTO Sessions (StartDate, EndDate)
            VALUES (@start, @end)";

        db.SaveData(sql, session);
    }

    public void DeleteSession()
    {
        // TODO
        throw new NotImplementedException();
    }
}
