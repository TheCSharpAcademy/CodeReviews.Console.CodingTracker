using Microsoft.Data.Sqlite;
using Spectre.Console;
using Dapper;
namespace CodingTracker.JaegerByte
{
    internal class DatabaseService
    {
        public void InsertSession(string startTime, string endTime)
        {
            if (Program.validationHandler.CheckDateInput(startTime, endTime))
            {
                using (SqliteConnection connection = new SqliteConnection(Program.connectionString))
                {
                    connection.Open();
                    connection.Execute($"INSERT INTO logs(StartTime, EndTime) VALUES('{startTime}', '{endTime}')");
                    connection.Close();
                }
            }
            else
            {
                AnsiConsole.Write(Program.validationHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }
        public void DeleteSession(string deleteIndex)
        {
            List<CodingSession> sessions = Program.databaseService.GetCodingSessions();
            if (Program.validationHandler.CheckIntInput(deleteIndex) && Program.validationHandler.CheckIndexExists(sessions, deleteIndex))
            {
                using (SqliteConnection connection = new SqliteConnection(Program.connectionString))
                {
                    connection.Open();
                    connection.Execute($"DELETE FROM logs WHERE ID='{deleteIndex}'");
                }
            }
            else
            {
                AnsiConsole.Write(Program.validationHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }
        public List<CodingSession> GetCodingSessions()
        {
            using (SqliteConnection connection = new SqliteConnection(Program.connectionString))
            {
                connection.Open();
                return connection.Query<CodingSession>("SELECT * FROM logs").ToList<CodingSession>();
            }
        }
        public void UpdateSession(string updateIndex, string startTime, string endTime)
        {
            List<CodingSession> sessions = Program.databaseService.GetCodingSessions();
            if (Program.validationHandler.CheckIntInput(updateIndex) && Program.validationHandler.CheckIndexExists(sessions, updateIndex))
            {
                if (Program.validationHandler.CheckDateInput(startTime, endTime))
                {
                    using (SqliteConnection connection = new SqliteConnection(Program.connectionString))
                    {
                        connection.Open();
                        connection.Execute($"UPDATE logs SET StartTime='{startTime}', EndTime='{endTime}' WHERE ID = {Convert.ToInt64(updateIndex)}");
                    }
                }
                else
                {
                    AnsiConsole.Write(Program.validationHandler.GetInvalidResponse());
                    Console.ReadKey();
                }
            }
            else
            {
                AnsiConsole.Write(Program.validationHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }
    }
}
