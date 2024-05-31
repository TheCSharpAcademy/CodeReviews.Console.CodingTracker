using Dapper;
using Patryk_MM.Console.CodingTracker.Config;
using Patryk_MM.Console.CodingTracker.Models;
using Patryk_MM.Console.CodingTracker.Queries.Session;
using Patryk_MM.Console.CodingTracker.Utilities;

namespace Patryk_MM.Console.CodingTracker.Services {
    /// <summary>
    /// Provides services for managing coding sessions and goals.
    /// </summary>
    public class TrackerService {
        /// <summary>
        /// Retrieves all coding sessions from the database.
        /// </summary>
        /// <returns>A list of <see cref="CodingSession"/> objects.</returns>
        public List<CodingSession> GetSessions() {
            using (var connection = Database.GetConnection()) {
                string selectQuery = "SELECT * FROM CodingSessions";
                return connection.Query<CodingSession>(selectQuery).ToList();
            }
        }

        /// <summary>
        /// Retrieves a coding session from the list of sessions.
        /// </summary>
        /// <returns>The selected <see cref="CodingSession"/> or null if cancelled.</returns>
        public CodingSession? GetSessionFromList() {
            var getSessionsHandler = new GetSessionsHandler(this);
            var sessions = getSessionsHandler.Handle();
            DataVisualization.PrintSessions(sessions);
            int sessionId = UserInput.GetSessionId(sessions);
            if (sessionId == 0) { return null; }

            CodingSession session = sessions[sessionId - 1];
            return session;
        }

        /// <summary>
        /// Creates a new coding session in the database.
        /// </summary>
        /// <param name="session">The <see cref="CodingSession"/> to create.</param>
        public void CreateSession(CodingSession session) {
            using (var connection = Database.GetConnection()) {
                string insertQuery = "INSERT INTO CodingSessions (StartDate, EndDate, Duration) VALUES (@StartDate, @EndDate, @Duration);";
                connection.Execute(insertQuery, session);
            }
        }

        /// <summary>
        /// Updates an existing coding session in the database.
        /// </summary>
        /// <param name="session">The <see cref="CodingSession"/> to update.</param>
        public void UpdateSession(CodingSession session) {
            using (var connection = Database.GetConnection()) {
                string updateQuery = "UPDATE CodingSessions SET StartDate = @StartDate, EndDate = @EndDate, Duration = @Duration WHERE Id = @Id;";
                connection.Execute(updateQuery, session);
            }
        }

        /// <summary>
        /// Deletes a coding session from the database.
        /// </summary>
        /// <param name="session">The <see cref="CodingSession"/> to delete.</param>
        public void DeleteSession(CodingSession session) {
            using (var connection = Database.GetConnection()) {
                string deleteQuery = "DELETE FROM CodingSessions WHERE Id = @Id;";
                connection.Execute(deleteQuery, session);
            }
        }

        /// <summary>
        /// Retrieves all coding goals from the database.
        /// </summary>
        /// <returns>A list of <see cref="CodingGoal"/> objects.</returns>
        public List<CodingGoal> GetGoals() {
            using (var connection = Database.GetConnection()) {
                string selectQuery = "SELECT * FROM CodingGoals;";
                return connection.Query<CodingGoal>(selectQuery).ToList();
            }
        }

        /// <summary>
        /// Creates a new coding goal in the database.
        /// </summary>
        /// <param name="goal">The <see cref="CodingGoal"/> to create.</param>
        public void CreateGoal(CodingGoal goal) {
            using (var connection = Database.GetConnection()) {
                string insertQuery = "INSERT INTO CodingGoals (YearAndMonth, Hours, HourGoal) VALUES (@YearAndMonth, @Hours, @HourGoal);";
                connection.Execute(insertQuery, goal);
            }
        }

        /// <summary>
        /// Updates an existing coding goal in the database.
        /// </summary>
        /// <param name="goal">The <see cref="CodingGoal"/> to update.</param>
        public void UpdateGoal(CodingGoal goal) {
            using (var connection = Database.GetConnection()) {
                string updateQuery = "UPDATE CodingGoals SET YearAndMonth = @YearAndMonth, Hours = @Hours, HourGoal = @HourGoal WHERE Id = @Id;";
                connection.Execute(updateQuery, goal);
            }
        }
    }
}
