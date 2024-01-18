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
    public void AddCodingSession(CodingSession session);
    public List<CodingSession> GetCodingSessions();
}
