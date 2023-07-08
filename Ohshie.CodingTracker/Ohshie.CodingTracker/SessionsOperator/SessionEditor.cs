using Ohshie.CodingTracker.DbOperations;

namespace Ohshie.CodingTracker.SessionsOperator;

public class SessionEditor
{
    private readonly DbOperations.DbOperations _dbOperations = new();
    private readonly DbEditSessionOperations _sessionDbOperations = new();
    
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

    public void EditSessionDate(string newDate, int id)
    {
        Session session = _dbOperations.FetchSession(id);
        if (session.Id == 0) return;
        
        _sessionDbOperations.EditSessionDate(newDate, session);
    }

    public void EditSessionLength(string newLength, int id)
    {
        Session session = _dbOperations.FetchSession(id);
        if (session.Id == 0) return;
        
        _sessionDbOperations.EditSessionLength(newLength, session);
    }

    public void WipeData()
    {
        _dbOperations.WipeTable();
    }
}