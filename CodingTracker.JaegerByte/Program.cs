using Spectre.Console;
using Microsoft.Data.Sqlite;
using Dapper;
using System.Configuration;
using System.Collections.Specialized;

namespace CodingTracker.JaegerByte
{
    enum MenuOption {insertSession, deleteSession, updateSession, viewAllSessions, CloseApplication}
    internal class Program
    {
        static Validation validationHandler = new Validation();
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        static void Main()
        {
            CreateTable();

            while (true)
            {
                Console.Clear();
                MenuOption selectedOption = GetOption();
                switch (selectedOption)
                {
                    case MenuOption.insertSession:
                        InsertSession();
                        break;
                    case MenuOption.deleteSession:
                        DeleteSession();
                        break;
                    case MenuOption.updateSession:
                        UpdateSession();
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

        static MenuOption GetOption()
        {
            SelectionPrompt<MenuOption> selectionPrompt = new SelectionPrompt<MenuOption>();
            selectionPrompt.Title = "Select option";
            foreach (MenuOption option in Enum.GetValues(typeof(MenuOption)))
            {
                selectionPrompt.AddChoice(option);
            }
            return AnsiConsole.Prompt(selectionPrompt);
        }

        static void InsertSession()
        {
            string startTime = AnsiConsole.Ask<string>("Insert Start Time (Format HH:mm)?");
            string endTime = AnsiConsole.Ask<string>("Insert End Time (Format HH:mm)?");
            if (validationHandler.CheckDateInput(startTime, endTime))
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute($"INSERT INTO logs(StartTime, EndTime) VALUES('{startTime}', '{endTime}')");
                    connection.Close();
                }
            }
            else
            {
                AnsiConsole.Write(validationHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }

        static void DeleteSession()
        {
            List<CodingSession> sessions = GetCodingSessions();
            AnsiConsole.Write(GetAllSessionsGrid(sessions));
            string deleteIndex = AnsiConsole.Ask<string>("select ID to delete");
            if (validationHandler.CheckIntInput(deleteIndex) && validationHandler.CheckIndexExists(sessions, deleteIndex))
            {
                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();
                    connection.Execute($"DELETE FROM logs WHERE ID='{deleteIndex}'");
                }
            }
            else
            {
                AnsiConsole.Write(validationHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }

        static void ViewAllSessions()
        {
            AnsiConsole.Write(GetAllSessionsGrid(GetCodingSessions()));
            Console.WriteLine("press ANY key to get back to the main menu");
            Console.ReadKey(true);
        }

        static void UpdateSession()
        {
            List<CodingSession> sessions = GetCodingSessions();
            AnsiConsole.Write(GetAllSessionsGrid(sessions));
            string updateIndex = AnsiConsole.Ask<string>("select ID to update");
            if (validationHandler.CheckIntInput(updateIndex) && validationHandler.CheckIndexExists(sessions, updateIndex))
            {
                string startTime = AnsiConsole.Ask<string>("Insert Start Time (Format HH:mm)?");
                string endTime = AnsiConsole.Ask<string>("Insert End Time (Format HH:mm)?");
                if (validationHandler.CheckDateInput(startTime, endTime))
                {
                    using (SqliteConnection connection = new SqliteConnection(connectionString))
                    {
                        connection.Open();
                        connection.Execute($"UPDATE logs SET StartTime='{startTime}', EndTime='{endTime}' WHERE ID = {Convert.ToInt64(updateIndex)}");
                    }
                }
                else
                {
                    AnsiConsole.Write(validationHandler.GetInvalidResponse());
                    Console.ReadKey();
                }
            }
            else
            {
                AnsiConsole.Write(validationHandler.GetInvalidResponse());
                Console.ReadKey();
            }
        }

        static Grid GetAllSessionsGrid(List<CodingSession> sessionList)
        {
            Grid grid = new Grid();

            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Text[]
            {
                new Text("ID", new Style(Color.Blue)).LeftJustified(),
                new Text("Start Time", new Style(Color.Green)).Centered(),
                new Text("End Time", new Style(Color.Red)).Centered(),
                new Text("Duration", new Style(Color.Yellow)).LeftJustified(),
            });
            foreach (CodingSession item in sessionList)
            {
                grid.AddRow(new Text[]
                {
                    new Text(item.ID.ToString()),
                    new Text(item.StartTime.ToString("g")),
                    new Text(item.EndTime.ToString("g")),
                    new Text(item.Duration.ToString("hh\\:mm")),
                }); ;
            }
            return grid;
        }

        static List<CodingSession> GetCodingSessions()
        {
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                return connection.Query<CodingSession>("SELECT * FROM logs").ToList<CodingSession>();
            }
        }
    }
}
