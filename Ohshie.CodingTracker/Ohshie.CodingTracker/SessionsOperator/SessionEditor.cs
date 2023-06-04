namespace Ohshie.CodingTracker;

public class SessionEditor
{
    private readonly DbOperations _dbOperations = new();
    private readonly DbEditSessionOperations _sessionDbOperations = new();

    private Session FetchSession(int sessionId)
    {
        Session session = _dbOperations.FetchSession(sessionId);
        return session;
    }

    public void EditSessionName(string newName, int id)
    {
        Session session = _dbOperations.FetchSession(id);
        if (session.Id == 0) return;
        
        _sessionDbOperations.EditSessionName(newName, session);
    }
    
    public void EditSessionNote(string newNote, int id)
    {
        Session session = _dbOperations.FetchSession(id);
        if (session.Id == 0) return;
        
        _sessionDbOperations.EditSessionNote(newNote, session);
    }

    public void RemoveSession(int id)
    {
        Session session = _dbOperations.FetchSession(id);
        if (session.Id == 0) return;
        
        _dbOperations.RemoveSession(session);
    }

    public void WipeData()
    {
        _dbOperations.WipeTable();
    }
}