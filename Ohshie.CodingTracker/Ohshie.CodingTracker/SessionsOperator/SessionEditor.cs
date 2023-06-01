namespace Ohshie.CodingTracker;

public class SessionEditor
{
    private readonly DbOperations _dbOperations = new();

    private Session FetchSession(int sessionId)
    {
        Session session = _dbOperations.FetchSession(sessionId);
        return session;
    }

    public bool RemoveSession(int id)
    {
        Session session = _dbOperations.FetchSession(id);
        if (session is null) return false;
        
        _dbOperations.RemoveSession(session);
        return true;
    }

    public void WipeData()
    {
        _dbOperations.WipeTable();
    }
}