using CodingTracker.alvaromosconi.Data;
using CodingTracker.alvaromosconi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.alvaromosconi
{
    internal class CodingController
    {
        private CodeSessionLocalStorage cs = new CodeSessionLocalStorage();
        private CodeSessionLocalStorage sessionStorage;

        public CodingController(CodeSessionLocalStorage sessionStorage)
        {
            this.sessionStorage = sessionStorage;
        }

        internal void SaveSession(DateTime start, DateTime end)
        {
            CodeSessionModel session = new CodeSessionModel
            {
                StartDateTime = start,
                EndDateTime = end
            };

            cs.SaveSession(session);
        }

        internal List<CodeSessionModel> GetAllSessions()
        {
            return cs.GetAllSesions();
        }

        internal List<CodeSessionModel> GetSessionsInRange(DateTime start, DateTime end)
        {
            return cs.GetAllSessionsBetween(start, end);
        }

        internal void DeleteSession(CodeSessionModel session)
        {
            cs.DeleteSession(session);
        }
    }

}
