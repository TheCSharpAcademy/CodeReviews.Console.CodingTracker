using CodingTracker.Arashi256.Classes;
using CodingTracker.Arashi256.Models;
using Dapper;

namespace CodingTracker.Arashi256.Controllers
{
    internal class SessionController
    {
        private Database _database;

        public SessionController() 
        {
            _database = new Database();
        }

        public string AddCodingSession(DateTime startDT, DateTime endDT)
        {
            string duration = Utility.CalculateDuration(startDT, endDT);
            var codingSession = new CodingSession() { Id = null, StartDateTime = startDT, EndDateTime = endDT, Duration = duration };
            int rows = _database.AddNewCodingSession(codingSession);
            return $"Added {rows} coding session of {duration} time";
        }

        public string UpdateCodingSession(int id, DateTime startDT, DateTime endDT) 
        {
            string duration = Utility.CalculateDuration(startDT, endDT);
            var codingSession = new CodingSession() { Id = id, StartDateTime = startDT, EndDateTime = endDT, Duration = duration };
            int rows = _database.UpdateCodingSession(codingSession);
            return $"Updated {rows} coding session with id of {codingSession.Id} with new duration of {codingSession.Duration}";
        }

        public string DeleteCodingSession(int id)
        {
            var codingSession = GetCodingSession(id);
            int rows = _database.DeleteCodingSession(codingSession);
            return $"Deleted {rows} coding session with id of {codingSession.Id}";
        }

        public CodingSession? GetCodingSession(int id)
        {
            var param = new DynamicParameters();
            param.Add("@Id", id);
            string query = @"SELECT Id, StartDateTime, EndDateTime, Duration 
                    FROM coding_session 
                    WHERE Id = @Id;";
            List<CodingSession> codingSessions = _database.GetCodingSessionResults(query, param);
            return codingSessions.Count != 1 ? null : codingSessions[0];
        }

        public List<CodingSession> GetCodingSessionsBetweenDates(DateTime startDT, DateTime endDT, Utility.SortOrder sortOrder)
        {
            var param = new DynamicParameters();
            param.Add("@StartDateTime", startDT.ToString("yyyy-MM-dd HH:mm:ss"));
            param.Add("@EndDateTime", endDT.ToString("yyyy-MM-dd HH:mm:ss"));
            string query = @$"SELECT Id, StartDateTime, EndDateTime, Duration 
                    FROM coding_session 
                    WHERE DATETIME(StartDateTime) >= @StartDateTime AND DATETIME(StartDateTime) <= @EndDateTime
                    ORDER BY DATETIME(StartDateTime) {Utility.TranslateSortOrderToString(sortOrder)};";
            return _database.GetCodingSessionResults(query, param);
        }

        public List<CodingSession> GetDaysCodingSessions(int days, Utility.SortOrder sortOrder) 
        {
            var param = new DynamicParameters();
            param.Add("@Days", $"-{days} days");
            string query = @$"SELECT Id, StartDateTime, EndDateTime, Duration 
                    FROM coding_session 
                    WHERE DATE(StartDateTime) >= DATETIME('now', @Days) AND DATE(EndDateTime) <= DATETIME('now') 
                    ORDER BY DATETIME(StartDateTime) {Utility.TranslateSortOrderToString(sortOrder)};";
            return _database.GetCodingSessionResults(query, param);
        }

        public int[] GetArrayOfValidSessionIDs(List<CodingSession> codingSessions) 
        {
            int[] validIds = new int[codingSessions.Count];
            for (int i = 0; i < codingSessions.Count; i++)
            {
                validIds[i] = i + 1;
            }
            return validIds;
        }

        public bool ResetDatabase()
        {
            return _database.CheckDatabase(true);
        }
    }
}