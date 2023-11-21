using CodingTracker.Models;
using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker
{
    internal class CrudController
    {
        internal static void InsertRecord()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            Console.Clear();
            Console.WriteLine("\t++++++++++++++++++++++++++");
            Console.WriteLine("\t++ Inserting new record ++");
            Console.WriteLine("\t++++++++++++++++++++++++++");

            DateTime parsedStartDate, parsedEndDate;
            TimeSpan duration;
            do
            {
                parsedStartDate = Validation.ParseAndValidateDate("\nEnter a start time. (d/M/yyyy HH:mm)");
                parsedEndDate = Validation.ParseAndValidateDate("\nEnter an end time. (d/M/yyyy HH:mm)");
                duration = parsedEndDate - parsedStartDate;

                if (duration.TotalMinutes < 0)
                {
                    Console.WriteLine("End date should be greater than or equal to start date. Please try again.");
                }
            } while (duration.TotalMinutes < 0);

            //write to db
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"INSERT INTO coding_tracker (StartTime, EndTime, Duration)" +
                    $"VALUES ('{parsedStartDate.ToString()}', '{parsedEndDate.ToString()}', {duration.TotalMinutes})";


                int rowsAdded = tableCmd.ExecuteNonQuery();
                Console.WriteLine($"\nYou added {rowsAdded} row.");

                connection.Close();
            }
            Console.ReadLine();
        }

        internal static void ViewRecords(bool includeMainMenuPrompt = true)
        {
            Console.Clear();
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
            List<CodingSessionModel> codingSessions = new List<CodingSessionModel>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM coding_tracker";

                using (var reader = tableCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        codingSessions.Add(
                            new CodingSessionModel
                            {
                                Id = reader.GetInt32(0),
                                StartTime = DateTime.ParseExact(reader.GetString(1), "d/M/yyyy H:mm:ss", null),
                                EndTime = DateTime.ParseExact(reader.GetString(2), "d/M/yyyy H:mm:ss", null),
                                Duration = TimeSpan.FromMinutes(reader.GetInt32(3))
                            });
                    }
                }
                //create and print table with results
                var listOfRecords = new List<List<object>>();
                foreach (CodingSessionModel v in codingSessions)
                {
                    string displayString = $"ID: {v.Id} -- Start: {v.StartTime.Day:D2}/{v.StartTime.Month:D2}/{v.StartTime.Year:D4} at {v.StartTime.Hour:D2}:{v.StartTime.Minute:D2} -- End: {v.EndTime.Day:D2}/{v.EndTime.Month:D2}/{v.EndTime.Year:D4} at {v.EndTime.Hour:D2}:{v.EndTime.Minute:D2} -- Duration(mins): {v.Duration.TotalMinutes} mins";
                    listOfRecords.Add(new List<object> { displayString });
                }
                ConsoleTableBuilder
                    .From(listOfRecords)
                    .ExportAndWriteLine();
                //to make a distinction between ViewRecords for DeleteRecord or Main function, 
                //I am VERY unsure if this is frowned upon or not, it doesn't seem very clean to me
                if (includeMainMenuPrompt)
                {
                    Console.WriteLine("Press any key to return to main menu.");
                    Console.ReadLine();
                }
            }
        }
        internal static void DeleteRecord()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            Console.Clear();
            Console.WriteLine("\t++++++++++++++++++++++++++");
            Console.WriteLine("\t++ Deleting record ++");
            Console.WriteLine("\t++++++++++++++++++++++++++");
            Console.WriteLine();

            int parsedIDInteger = 0;
            bool isValidID = false;
            while (isValidID == false)
            {
                ViewRecords(false);
                Console.WriteLine("Enter the ID of the record you want to delete. Press Q to cancel.");
                var userInputID = Console.ReadLine();

                if (userInputID.ToLower() == "q")
                {
                    TableVisualisationEngine.MainMenu();
                }
                else if (string.IsNullOrEmpty(userInputID) || !int.TryParse(userInputID, out parsedIDInteger) || parsedIDInteger < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer and correct ID, ENTER to try again.");
                    Console.ReadLine();
                }
                else
                {
                    isValidID = true;
                }
                Console.Clear();
            }
            // delete from db
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE FROM coding_tracker WHERE Id = {parsedIDInteger}";


                int rowsEdited = tableCmd.ExecuteNonQuery();

                if (rowsEdited == 0)
                {
                    Console.WriteLine("That record doesn't exist brotato! Enter a correct ID. Press Enter to continue.");
                    Console.ReadLine();
                }
                Console.WriteLine($"You deleted {rowsEdited} row(s).");
                Console.ReadLine();
                connection.Close();
            }
        }
        internal static void UpdateRecord()
        {
            string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

            Console.Clear();
            Console.WriteLine("\t++++++++++++++++++++++++++");
            Console.WriteLine("\t++ Updating record ++");
            Console.WriteLine("\t++++++++++++++++++++++++++");
            Console.WriteLine();

            int parsedIDInteger = 0;
            bool isValidID = false;
            while (isValidID == false)
            {
                ViewRecords(false);
                Console.WriteLine("Enter the ID of the record you want to update. Press Q to cancel.");
                var userInputID = Console.ReadLine();

                if (userInputID.ToLower() == "q")
                {
                    TableVisualisationEngine.MainMenu();
                }
                else if (string.IsNullOrEmpty(userInputID) || !int.TryParse(userInputID, out parsedIDInteger) || parsedIDInteger < 0)
                {
                    Console.WriteLine("Invalid input. Please enter a positive integer and correct ID, ENTER to try again.");
                    Console.ReadLine();
                }
                else
                {
                    isValidID = true;
                }
                Console.Clear();
            }

            DateTime newStartDate, newEndDate;
            TimeSpan newDuration;
            do
            {
                newStartDate = Validation.ParseAndValidateDate("\nEnter new start time. (d/M/yyyy HH:mm)");
                newEndDate = Validation.ParseAndValidateDate("\nEnter new end time. (d/M/yyyy HH:mm)");
                newDuration = newEndDate - newStartDate;

                if (newDuration.TotalMinutes < 0)
                {
                    Console.WriteLine("End date should be greater than or equal to start date. Please try again.");
                }
            } while (newDuration.TotalMinutes < 0);
            

            // update in db
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"UPDATE coding_tracker " +
                    $"SET StartTime = '{newStartDate}', EndTime = '{newEndDate}', Duration = {newDuration.TotalMinutes} " +
                    $"WHERE Id = {parsedIDInteger}";


                int rowsEdited = tableCmd.ExecuteNonQuery();

                if (rowsEdited == 0)
                {
                    Console.WriteLine("That record doesn't exist brotato! Enter a correct ID. Press Enter to continue.");
                    Console.ReadLine();
                }
                Console.WriteLine($"You updated {rowsEdited} row(s).");
                Console.ReadLine();
                connection.Close();
            }
        }
    }
}
