using frockett.CodingTracker.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library;

public interface IDbMethods
{
    public void InitDatabase();
    public void InsertCodingSession(CodingSession session);
    public void UpdateCodingSession(CodingSession session);
    public bool ValidateSessionById(int id);
    public void DeleteCodingSession(int id);
    public List<CodingSession> GetCodingSessions();
}
