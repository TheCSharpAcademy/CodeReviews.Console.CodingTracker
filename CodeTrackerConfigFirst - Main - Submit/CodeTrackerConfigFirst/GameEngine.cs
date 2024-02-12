
using Microsoft.Data.Sqlite;
using System.Globalization;
using CodeTrackerConfigFirst;
using System.Configuration;

namespace CodeTrackerConfigFirst
{
    internal class GameEngine
    {
        static readonly string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");

        internal static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT * FROM coding_session";

                List<CodingSession> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(
                        new CodingSession
                        {
                            Id = reader.GetInt32(0),
                            StartTime = DateTime.Parse(reader.GetString(1)),
                            EndTime = DateTime.Parse(reader.GetString(2)),
                            Duration = reader.GetInt32(3)
                        });
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();

                Console.WriteLine("----------------------------------------------------\n");
                foreach (var dw in tableData)
                {
                    Console.WriteLine($"{dw.Id} - {dw.StartTime.ToString("yyyy-MM-dd HH:mm")} - {dw.EndTime.ToString("yyyy-MM-dd HH:mm")} - {dw.Duration} minutes");
                }
                Console.WriteLine("----------------------------------------------------\n");
            }
        }

        internal static void InsertTime()
        {
            string startTimeInput = GetStartTimeInput();

            string endTimeInput = GetEndTimeInput();

            CultureInfo culture = new CultureInfo("en-US");
            DateTime startTime = Convert.ToDateTime(startTimeInput, culture);
            DateTime endTime = Convert.ToDateTime(endTimeInput, culture);

            double durationDouble = CalculateDuration(startTime, endTime);
            int duration = Convert.ToInt32(durationDouble);

            if (duration < 0) 
            {
                Console.WriteLine("Duration cannot be negative. Type 0 to Start from scartch");
                string goBacktoMainMenu = Console.ReadLine();
                if (goBacktoMainMenu == "0") MainMenu.GetUserInput();                
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                $"INSERT INTO coding_session (StartTime,EndTime, Duration) VALUES('{startTime}', '{endTime}', '{duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static double CalculateDuration(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime).TotalMinutes;
        }
        internal static void Delete()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = Validation.GetNumberInput("\n\nPlease type the Id of the record you want to delete ot type 0 to back to Main Menu\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"DELETE from coding_session WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.\n\n");
                    Delete();
                }
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

            MainMenu.GetUserInput();
        }
        internal static void Update()
        {
            GetAllRecords();

            var recordId = Validation.GetNumberInput("\n\nPlease type Id of the record you would like to update. Type 0 to go back to Main Menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var checkCmd = connection.CreateCommand();
                checkCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_session WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n");
                    connection.Close();
                    Update();
                }

                string startTimeInput = GetStartTimeInput();

                string endTimeInput = GetEndTimeInput();

                CultureInfo culture = new CultureInfo("en-US");
                DateTime startTime = Convert.ToDateTime(startTimeInput, culture);
                DateTime endTime = Convert.ToDateTime(endTimeInput, culture);

                double durationDouble = CalculateDuration(startTime, endTime);
                int duration = Convert.ToInt32(durationDouble);

                if (duration < 0)
                {
                    Console.WriteLine("Duration cannot be negative. Type 0 to Start from scartch");
                    string goBacktoMainMenu = Console.ReadLine();
                    if (goBacktoMainMenu == "0") MainMenu.GetUserInput();
                }

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = $"UPDATE coding_session SET StartTime = '{startTime}', EndTime = '{endTime}', Duration = '{duration}' WHERE Id = {recordId}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }
        internal static string GetStartTimeInput()
        {
            Console.WriteLine("\n\nPlease insert the StartTime: (Format: 12/28/2010 12:10:15 PM). Type 0 to return to main menu");
            string startTimeInput = Console.ReadLine();

            if (startTimeInput == "0") MainMenu.GetUserInput();

            while (!DateTime.TryParseExact(startTimeInput, "MM/dd/yyyy hh:mm:ss tt", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\n\nInvalid date. (Format: 12/28/2010 12:10:15 PM). Type 0 to return to main menu or try again.\n\n");
                startTimeInput = Console.ReadLine();
            }

            return startTimeInput;
        }
        internal static string GetEndTimeInput()
        {
            Console.WriteLine("\n\nPlease insert the EndTime: (Format: 12/28/2010 12:10:15 PM). Type 0 to return to main menu");
            string endTimeInput = Console.ReadLine();

            if (endTimeInput == "0") MainMenu.GetUserInput();

            return endTimeInput;
        }
    }
}
