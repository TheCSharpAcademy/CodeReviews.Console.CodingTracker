using CodingTrackerProgram.Model;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTrackerProgram.Database
{
    public class CodingSessionRepository
    {
        public static string TableName
        {
            get
            {
                return "CodingSessions";
            }
        }

        public static void CreateTable()
        {
            try
            {
                using (SqliteConnection connection = Connection.GetConnection())
                {
                    connection.Open();

                    var command = connection.CreateCommand();

                    command.CommandText = $@"
                    CREATE TABLE IF NOT EXISTS {TableName} (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        StartDateTime DATETIME NOT NULL,
                        EndDateTime DATETIME NOT NULL
                )";

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Could not initialise application. {ex.Message}.\nExiting...");
                Environment.Exit(1);
            }
        }

        public static CodingSession? FindById(Int64 codingSessionId)
        {
            CodingSession? session = null;

            try
            {
                using (SqliteConnection connection = Connection.GetConnection())
                {
                    connection.Open();

                    string sql = $@"SELECT * FROM {TableName}
                        WHERE Id = @Id";

                    var sessions = connection.Query<CodingSession>(
                        sql,
                        new { Id = codingSessionId }
                    ).ToList() ?? new List<CodingSession>();

                    if (sessions.Count == 0)
                    {
                        Console.WriteLine("Could not find.");
                        return null;
                    }

                    return sessions[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\tERROR: Could not find. {ex.Message}");
            }

            return session;
        }

        public static List<CodingSession> FindAll()
        {
            List<CodingSession> sessions = new();

            try
            {
                using (SqliteConnection connection = Connection.GetConnection())
                {
                    connection.Open();
                    sessions = connection.Query<Model.CodingSession>($@"SELECT * FROM {TableName}").ToList() ?? sessions;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\tERROR: Could not display. {ex.Message}");
            }

            return sessions;
        }

        public static void Create(DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                using (SqliteConnection connection = Connection.GetConnection())
                {
                    connection.Open();

                    string sql =
                        $@"INSERT INTO {TableName} (StartDateTime, EndDateTime) VALUES (@StartDateTime, @EndDateTime)";


                    int rowsAffected = connection.Execute(sql, new { StartDateTime = startDateTime, EndDateTime = endDateTime });

                    Console.WriteLine(
                        rowsAffected == 0 ? "\nCould not be created." : "\nCreated. Good job!"
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Could not create. {ex.Message}");
            }
        }

        public static void Update(Int64 codingSessionId, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                using (SqliteConnection connection = Connection.GetConnection())
                {
                    connection.Open();

                    string sql =
                        $@"UPDATE {TableName}
                            SET StartDateTime = @StartDateTime, EndDateTime = @EndDateTime
                            WHERE Id = @Id";


                    int rowsAffected = connection.Execute(
                        sql,
                        new
                        {
                            Id = codingSessionId,
                            StartDateTime = startDateTime,
                            EndDateTime = endDateTime
                        }
                    );

                    Console.WriteLine(
                        rowsAffected == 0 ? "\nCould not be updated." : $"\nUpdated ID {codingSessionId}. Good job!"
                    );
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Could not update");
            }
        }

        public static void Delete(Int64 id)
        {
            try
            {
                using (SqliteConnection connection = Connection.GetConnection())
                {
                    connection.Open();
                    int rowsAffected = connection.Execute($@"DELETE FROM {TableName} WHERE Id = @Id", new { Id = id });

                    Console.WriteLine(
                        rowsAffected == 0 ? "\nCould not be deleted." : $"\nDeleted ID {id}"
                    );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n\tERROR: Could not delete. {ex.Message}");
            }
        }
    }
}