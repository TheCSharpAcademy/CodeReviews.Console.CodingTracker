using Dapper;
using Patryk_MM.Console.CodingTracker.Config;
using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Queries;
using Patryk_MM.Console.CodingTracker.Utilities;

namespace Patryk_MM.Console.CodingTracker.Services {
    public class TrackerService {
        public List<CodingSession> GetSessions() {
            using (var connection = Database.GetConnection()) {
                string selectQuery = "SELECT * FROM CodingSessions";

                return connection.Query<CodingSession>(selectQuery).ToList();
            }
        }

        public CodingSession? GetSessionFromList() {
            var getSessionsHandler = new GetSessionsHandler(this);
            var sessions = getSessionsHandler.Handle();
            DataVisualization.PrintSessions(sessions);
            int sessionId = UserInput.GetSessionId(sessions);
            if (sessionId == 0) { return null; }

            CodingSession session = sessions[sessionId - 1];

            return session;
        }

        public void CreateSession(CodingSession session) {
            using (var connection = Database.GetConnection()) {
                string insertQuery = "INSERT INTO CodingSessions (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration);";

                connection.Execute(insertQuery, session);
            }
        }

        public void UpdateSession(CodingSession session) {
            using (var connection = Database.GetConnection()) {
                string insertQuery = "UPDATE CodingSessions SET StartDate = @StartDate, EndDate = @EndDate, Duration = @Duration WHERE Id = @Id;";

                connection.Execute(insertQuery, session);
            }
        }

        public void DeleteSession(CodingSession session) {
            using (var connection = Database.GetConnection()) {
                string insertQuery = "DELETE FROM CodingSessions WHERE Id = @Id;";

                connection.Execute(insertQuery, session);
            }
        }
    }
}
