using CodingTracker.kraven88.Models;

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
		// TODO
		throw new NotImplementedException();
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

    public void SaveSession()
	{
        // TODO
        throw new NotImplementedException();
    }

    public void DeleteSession()
    {
        // TODO
        throw new NotImplementedException();
    }
}
