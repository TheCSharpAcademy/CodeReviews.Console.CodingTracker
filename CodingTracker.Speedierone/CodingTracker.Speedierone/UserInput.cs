using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.Globalization;
using System.Configuration;

namespace CodeTracker;
internal class UserInput
{
    internal static DateTime CheckDate(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
        {
            Console.WriteLine("Invalid entry. End time is earlier then start time.");
            Helpers.GetEndTime();
        }
        return endTime;
    }
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
        Console.Clear();
        string sAttr;
        sAttr = ConfigurationManager.AppSettings.Get("dbconnectionString");
        
        using (var connection = new SqliteConnection(sAttr))        
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
                                Date = DateOnly.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-GB")),
                                TimeStart = TimeOnly.ParseExact(reader.GetString(2), "HH:mm", new CultureInfo("en-GB")),
                                TimeEnd = TimeOnly.ParseExact(reader.GetString(3), "HH:mm", new CultureInfo("en-GB")),
                                TimeSpan = TimeSpan.ParseExact(reader.GetString(4), "c", new CultureInfo("en-GB"))
                            });                   //TODO change DateTime to TimeOnly.
                        }
                        TableLayout.DisplayTable(tableData);
                    }
                    else
                    {
                        Console.WriteLine("No rows found. Press enter to go back to main menu");
                        Console.ReadLine();
                    }
                    connection.Close();
                    Console.WriteLine("Press any key to continue.");
                    Console.ReadLine();
        }             
    }
    internal static void AddRecord()
    {
        Console.Clear();
        string date = Helpers.GetDate();
        string timeStart = Helpers.GetStartTime();
        string endTime = Helpers.GetEndTime();
        CheckDate(DateTime.Parse(timeStart), DateTime.Parse(endTime));
        string timeSpan = Helpers.CodingTime(timeStart, endTime);
        string connectionString = ConfigurationManager.AppSettings.Get("dbconnectionString");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO code_tracker(Date, TimeStart, TimeEnd, TimeSpan) VALUES('{date}', '{timeStart}', '{endTime}', '{timeSpan}')";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    internal static void DeleteRecord()
    {
        Console.Clear();
        ViewRecords();
        string connectionString = ConfigurationManager.AppSettings.Get("dbconnectionString");
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
            Console.WriteLine($"Record Id {recordId} deleted. Press any key to continue.");
            Console.ReadLine();
        }
               
    }
    internal static void UpdateRecord()
    {
        Console.Clear();
        ViewRecords();

        string connectionString = ConfigurationManager.AppSettings.Get("dbconnectionString");

        var recordId = GetNumberInput("\nPlease enter Id of record you wish to update or press 0 to go back");

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM code_tracker WHERE Id = {recordId})";
            int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (checkQuery == 0)
            {
                Console.WriteLine($"\nRecord with Id {recordId} doesn't exists");
                connection.Close();
                UpdateRecord();
            }

            string date = Helpers.GetDate();
            string startTime = Helpers.GetStartTime();
            string endTime = Helpers.GetEndTime();
            string timeSpan = Helpers.CodingTime(startTime, endTime);

            var tableCmd = connection.CreateCommand() ;
            tableCmd.CommandText = $"UPDATE code_tracker SET Date = '{date}', StartTime = '{startTime}', EndTime'{endTime}', TimeSpan = '{timeSpan}' WHERE Id = {recordId}";

            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
    internal static void AddTimerRecord()
    {
        Console.WriteLine("Press enter to start timer or 0 to return to menu.");
        var startTimer = Console.ReadLine();
        if (startTimer == "0") MainMenu.ShowMenu();
        var timer = new Stopwatch();                  
        timer.Start();
        var todayDateTime = DateTime.Now;
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var parsedStartDate = startDate.ToString("dd-MM-yy");
        var timeStart = TimeOnly.FromDateTime(todayDateTime);
        var parsedTimeStart = TimeOnly.ParseExact(timeStart.ToString(), "HH:mm", new CultureInfo("en-GB"));
             
        Console.WriteLine("Press enter to stop timer or 0 to return to main menu.");
        var endTimer = Console.ReadLine();
        if (endTimer == "0") MainMenu.ShowMenu();
            timer.Stop();
            var timeEnd = TimeOnly.FromDateTime(DateTime.Now);
        var parsedTimeEnd = TimeOnly.ParseExact(timeEnd.ToString(), "HH:mm", new CultureInfo("en-GB"));
        

        var timeSpan = Helpers.CodingTime(timeStart.ToString(), timeEnd.ToString());

        string connectionString = ConfigurationManager.AppSettings.Get("dbconnectionString");
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                $"INSERT INTO code_tracker(Date, TimeStart, TimeEnd, TimeSpan) VALUES('{parsedStartDate}', '{parsedTimeStart}', '{parsedTimeEnd}', '{timeSpan}')";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        }
    }
}
