using frockett.CodingTracker.Library;

namespace Library;

public interface IDbMethods
{
    public void InitDatabase();
    public void InsertCodingSession(CodingSession session);
    public void UpdateCodingSession(CodingSession session);
    public bool ValidateSessionById(int id);
    public void DeleteCodingSession(int id);
    public List<CodingSession> GetAllCodingSessions();
    public bool CheckForTableData();
}
