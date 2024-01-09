using Microsoft.Data.Sqlite;
using CodingTracker;
using ConsoleTableExt;
using System.Configuration;

class Program
{
    static int codingGoal;
    static double totalHours;

    static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

    DatabaseManager databaseManager = new();
 
    static void Main(string[] args)
    {
        DatabaseManager.CreateTable(connectionString);
        MainMenu();
    }

    static void MainMenu()
    {
        Console.Clear();
        bool closeApp = false;
        while (!closeApp)
        {

            Console.Clear();
            Console.WriteLine(@$"

CODING TRACKER
____________________________

{CalculateProgress()}
____________________________
MAIN MENU

Choose one of the following options:
0 - Exit the application
1 - View all records
2 - Insert record
3 - Delete record
4 - Update record
5 - Change or set goal");

            switch (Console.ReadLine())
            {
                case "0":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    ViewAllRecords();
                    break;
                case "2":
                    InsertRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    SetCodingGoal();
                    break;
                default:
                    Console.WriteLine("Insert a valid command");
                    MainMenu();
                    break;
            }
        }
    }

    private static void InsertRecord()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            Console.WriteLine("Please provide the coding session information.");
            string startDateTime = Helpers.GetDateTimeInput("Provide the session start time and date");
            string endDateTime = Helpers.GetDateTimeInput("Provide the session end time and date");
            string duration = Helpers.CalculateDuration(endDateTime, startDateTime);

            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO coding_sessions (StartDateTime, EndDateTime, Duration) VALUES ('{startDateTime}', '{endDateTime}', '{duration}') ";
            tableCmd.ExecuteNonQuery();
        }
    }
    private static void UpdateRecord()
    {
        Console.Clear();
        ViewAllRecords();

        string recordId = Helpers.GetNumperInput("\nProvide the id of the record you want to update.");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_sessions WHERE Id = {recordId})";
            int check = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (check == 0)
            {
                Console.WriteLine($"Record with Id {recordId} does not exist." +
                    $"\n Type 0 to go back to main menu, or any other key to update another record.");
                connection.Close();

                if (Console.ReadKey().Key == ConsoleKey.D0)
                {
                    Console.Clear();
                    MainMenu();
                }
                else
                {
                    UpdateRecord();
                }
            }

            string newDate = "";
            var tableCmd = connection.CreateCommand();

            Console.WriteLine("\nType 1 to update the start date, or type 2 to update the end date.");

            string userInput = Console.ReadLine();

            if (userInput == "1")
            {
                newDate = Helpers.GetDateTimeInput("Please provide a new start date");
                tableCmd.CommandText = $"UPDATE coding_sessions SET startDateTime = '{newDate}'";

            }
            else if (userInput == "2")
            {
                newDate = Helpers.GetDateTimeInput("Please providee a new end date");
                tableCmd.CommandText = $"UPDATE coding_sessions SET endDateTime = '{newDate}'";
            }

            tableCmd.ExecuteNonQuery();

            Console.WriteLine("The record with Id {recordId} was updated. Press any key to continue");
            Console.ReadLine();
            MainMenu();
        }
    }
    private static void DeleteRecord()
    {
        Console.Clear();
        ViewAllRecords();

        using (var connection = new SqliteConnection(connectionString))
        {
            string recordId = Helpers.GetNumperInput("\nPlease type the Id of the record you want to delete, or type 0 to return to main menu");

            if (recordId == "0")
            {
                MainMenu();
            }
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from coding_sessions WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} doesn't exist.");
                DeleteRecord();
            }

            Console.WriteLine($"\nRecord with Id {recordId} was deleted.");

            if (rowCount > 0)
            {
                Console.WriteLine("\nType 0 to continue to main menu or any other key to delete another record.");

                if (Console.ReadLine() == "0") MainMenu();
                else DeleteRecord();
            }
            else if (rowCount < 1)
            {
                Console.WriteLine("Press Enter to continue to main menu");
                Console.ReadKey();
            }
            MainMenu();
        }
    }
    private static void ViewAllRecords()
    {
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM coding_sessions ";

            List<CodingSession> tableData = new();
            using (SqliteDataReader reader = tableCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartDateTime = reader.GetString(1),
                            EndDateTime = reader.GetString(2),
                            Duration = reader.GetString(3)
                        });
                    }
                }
                else Console.WriteLine("No rows found");
            }
            ConsoleTableBuilder
                .From(tableData)
                .ExportAndWrite();
        }

        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }

    private static string CalculateProgress()
    {
        codingGoal = CodingGoal.LoadGoalValue();

        if (codingGoal > 0)
        {
            return @$"Your monthly coding goal: {codingGoal} 
You have {String.Format("{0:0.00}", codingGoal - SumDuration())} hours left to reach your goal.
To reach your goal for this month, you will have to code {(codingGoal / Helpers.DaysLeftInMonth(DateTime.Now)).ToString("F1")} hours a day.";

        }
        else
        {
            return "You have no set coding goal. Please set a goal to track.";
        }
    }

    public static void SetCodingGoal()
    {
        codingGoal = int.Parse(Helpers.GetNumperInput("Please set your coding goal for this month, counting in whole hours"));

        Console.WriteLine($"Your new coding goal: {codingGoal} hours.\n Good luck!");

        CodingGoal.SaveGoalValue(codingGoal);
    }

    private static double SumDuration()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                    @"SELECT
                        SUM(
                            (CAST(strftime('%H', duration) AS INTEGER) * 3600) +
                            (CAST(strftime('%M', duration) AS INTEGER) * 60)
                        ) as totalSeconds
                    FROM coding_sessions
                    WHERE strftime('%Y-%m', StartDateTime) = strftime('%Y-%m', CURRENT_DATE); ";

            var result = tableCmd.ExecuteScalar();

            if (result != null && double.TryParse(result.ToString(), out totalHours))
            {
                totalHours = totalHours / 3600.0;
            }
            return totalHours;
        }
    }
}
