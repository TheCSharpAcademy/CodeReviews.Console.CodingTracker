using CodingTracker.Data;
using CodingTracker.Models;

namespace CodingTracker.Controllers;

public class CodingController
{
    private readonly DbManager _dbManager;

    public CodingController(DbManager dbManager)
    {
        _dbManager = dbManager;
        _dbManager.CreateDb();
    }

    public List<CodingSession> GetAllSessions()
    {
        return _dbManager.GetAll();
    }

    public CodingSession? GetSessionById(int id)
    {
        var session = _dbManager.Get(id);
        if (session == null) return null;

        return session;
    }

    public CodingSession? CreateSession(CodingSession newSession)
    {
        var session = _dbManager.Add(newSession);
        if (session == null) return null;

        return session;
    }

    public CodingSession? UpdateSession(int id, CodingSession updatedSession)
    {
        if (id != updatedSession.Id) return null;

        var session = _dbManager.Update(id, updatedSession);
        if (session == null) return null;

        return session;
    }

    public CodingSession? DeleteSession(int id)
    {
        var session = _dbManager.Delete(id);
        if (session == null) return null;

        return session;
    }
}

