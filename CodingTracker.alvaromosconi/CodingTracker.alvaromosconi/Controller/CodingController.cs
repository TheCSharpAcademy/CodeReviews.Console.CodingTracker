using CodingTracker.alvaromosconi.Data;
using CodingTracker.alvaromosconi.Model;

namespace CodingTracker.alvaromosconi.Controller
{
    internal class CodingController
    {
        private CodeSessionLocalStorage localStorage = new CodeSessionLocalStorage();

        public CodingController(CodeSessionLocalStorage sessionStorage)
        {
            this.localStorage = sessionStorage;
        }

        internal void SaveSession(DateTime start, DateTime end)
        {
            CodeSessionModel session = new CodeSessionModel
            {
                StartDateTime = start,
                EndDateTime = end
            };

            localStorage.SaveSession(session);
        }

        internal List<CodeSessionModel> GetAllSessions()
        {
            return localStorage.GetAllSesions();
        }

        internal List<CodeSessionModel> GetSessionsInRange(DateTime start, DateTime end)
        {
            return localStorage.GetAllSessionsBetween(start, end);
        }

        internal void DeleteSession(int id)
        {
            localStorage.DeleteSession(id);
        }

    }

}
