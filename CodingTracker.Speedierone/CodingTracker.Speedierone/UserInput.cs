using Microsoft.Data.Sqlite;
using System;
using System.Globalization;

namespace CodeTracker;
internal class UserInput
{
    internal static int GetNumberInput(string message)
    {
        Console.WriteLine(message);

        string numberInput = Console.ReadLine();

        if (numberInput == "0") MainMenu.ShowMenu();

        while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
        {
            Console.WriteLine("\nInvalid number. Try again");
            numberInput = Console.ReadLine();
        }
        int finalInput = Convert.ToInt32(numberInput);

        return finalInput;
    }
    internal static void ViewRecords()
    {
        string connectionString = "Data Source=Coding-Tracker.db";
        Console.Clear();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"SELECT * FROM code_tracker";

            List<CodingSession> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    tableData.Add(new CodingSession
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")),
                        TimeSpan = TimeSpan.ParseExact(reader.GetString(2), "c", new CultureInfo("en-GB"))
                    });
                }                
            }
            else
            {
                Console.WriteLine("No rows found. Press enter to go back to main menu");
                Console.ReadLine();
            }
            connection.Close();
            Console.WriteLine("--------------------------------");
            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MM-yy")} - {dw.TimeSpan.ToString("c")}");                
            }
            Console.WriteLine("--------------------------------");
            Console.WriteLine("Press enter to return to main menu");
            Console.ReadLine();
        }
    }
    internal static void AddRecord()
    {
        Console.Clear();
        string date = Helpers.GetDate();
        string timeSpan = Helpers.GetTime();
        string connectionString = "Data Source=Coding-Tracker.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO code_tracker(Date, TimeSpan) VALUES('{date}','{timeSpan}')";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    internal static void DeleteRecord()
    {
        Console.Clear();
        ViewRecords();
        string connectionString = "Data Source=Coding-Tracker.db"; //TODO make this class variable.
        var recordId = GetNumberInput("\n\nPlease enter Id of entry you would like to delete or press 0 to return to main menu");
        

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"DELETE from code_tracker WHERE Id = '{recordId}'";

            int rowCount = tableCmd.ExecuteNonQuery();

            if (rowCount == 0)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. Please press enter to try again.");
                Console.ReadLine();
                DeleteRecord();
            }
            Console.WriteLine($"Record Id {recordId} deleted");
        }
               
    }
    internal static void UpdateRecord()
    {
        Console.Clear();
        ViewRecords();

        string connectionString = "Data Source=Coding-Tracker.db";

        var recordId = GetNumberInput("\nPlease enter Id of record you wish to update or press 0 to go back");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM code_tracker WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} doesnt exists");
                connection.Close();
                UpdateRecord();
            }

            string date = Helpers.GetDate();

            string timeSpan = Helpers.GetTime();

            var tableCmd = connection.CreateCommand() ;
            tableCmd.CommandText = $"UPDATE code_tracker SET Date = '{date}', TimeSpan = '{timeSpan}' WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
