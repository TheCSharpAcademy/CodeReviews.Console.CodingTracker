#if DEBUG
    using CodingTracker.models;
    using Dapper;

    namespace CodingTracker.services;

    /// <summary>
    /// The DatabaseService class provides methods for interacting with a database.
    /// </summary>
    internal partial class DatabaseService
    {
        /// <summary>
        /// Inserts a bulk of coding sessions into the database.
        /// </summary>
        /// <param name="sessions">The list of <see cref="CodingSession"/> objects to insert.</param>
        internal void BulkInsertSessions(List<CodingSession> sessions)
        {
            using var connection = GetConnection();
            
            string query = """
                                INSERT INTO records (StartTime, EndTime, Duration)
                                VALUES (@StartTime, @EndTime, @Duration)
                           """;

            connection.Execute(query, sessions.Select(session => new
            {
                session.StartTime,
                session.EndTime,
                session.Duration
            }));
        }
    }
#endif