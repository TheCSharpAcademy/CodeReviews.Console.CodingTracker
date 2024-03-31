using Spectre.Console;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Configuration;

namespace CodingTracker.JaegerByte
{
    enum MenuOption {insertSession, deleteSession, updateSession, viewAllSessions, CloseApplication}
    internal class Program
    {
        static public Validation validationHandler = new Validation();
        static public UserInput userInputHandler = new UserInput();
        static public DatabaseService databaseService = new DatabaseService();
        static public string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        static void Main()
        {
            CreateTable();

            while (true)
            {
                Console.Clear();
                MenuOption selectedOption = userInputHandler.GetOption();
                switch (selectedOption)
                {
                    case MenuOption.insertSession:
                        string startTime = userInputHandler.GetDateInput("Insert Start Time (Format dd-MM-yyyy HH:mm): ");
                        string endTime = userInputHandler.GetDateInput("Insert End Time (Format dd-MM-yyyy HH:mm): ");
                        databaseService.InsertSession(startTime, endTime);
                        break;
                    case MenuOption.deleteSession:
                        string indexToDelete = userInputHandler.GetIndexInput("Select index to delete: ");
                        databaseService.DeleteSession(indexToDelete);
                        break;
                    case MenuOption.updateSession:
                        string indexToUpdate = userInputHandler.GetIndexInput("Insert index to update: ");
                        string updatedStartTime = userInputHandler.GetDateInput("Insert Start Time (Format dd-MM-yyyy HH:mm): ");
                        string updatedEndTime = userInputHandler.GetDateInput("Insert End Time (Format dd-MM-yyyy HH:mm): ");
                        databaseService.UpdateSession(indexToUpdate, updatedStartTime, updatedEndTime);
                        break;
                    case MenuOption.viewAllSessions:
                        ViewAllSessions();
                        break;
                    case MenuOption.CloseApplication:
                        System.Environment.Exit(0);
                        break;
                }
            }
        }

        static void CreateTable()
        {
            try
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute(@"CREATE TABLE IF NOT EXISTS logs
                                (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                StartTime TEXT,
                                EndTime TEXT
                                )");
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating table: {ex.Message}");
                Console.ReadKey();
            }
        }

        static void ViewAllSessions()
        {
            AnsiConsole.Write(userInputHandler.GetAllSessionsGrid(databaseService.GetCodingSessions()));
            Console.WriteLine("press ANY key to get back to the main menu");
            Console.ReadKey(true);
        }
    }
}
