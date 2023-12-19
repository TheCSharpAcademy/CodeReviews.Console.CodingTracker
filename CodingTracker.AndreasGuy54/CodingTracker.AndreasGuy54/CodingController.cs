using ConsoleTableExt;
using Microsoft.Data.Sqlite;

namespace CodingTracker.AndreasGuy54
{
    internal static class CodingController
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connString");

        internal static void InsertRecord()
        {
            Console.WriteLine("\n\nStart date. (Format: dd-MM-yy HH:mm)");
            string startDate = UserInput.GetDateInput();

            Console.WriteLine("\n\nEnd date. (Format: dd-MM-yy HH:mm)");
            string endDate = UserInput.GetDateInput();

            bool isValidatedTime = Validation.IsValidatedTimes(startDate, endDate);

            if (!isValidatedTime)
            {
                Console.WriteLine("\nEnd Date cannot be lower than the Start Date\n");
                InsertRecord();
            }

            else
            {
                TimeSpan duration = CalculateDuration(startDate, endDate);
                string formattedDuration = duration.ToString(@"hh\:mm");

                using (SqliteConnection connection = new SqliteConnection(connectionString))
                {
                    connection.Open();

                    SqliteCommand insertCommand = connection.CreateCommand();
                    insertCommand.CommandText = $@"INSERT INTO coding_hours(starttime, endtime, duration) 
                                                    VALUES ('{startDate}','{endDate}', '{formattedDuration}')";
                    insertCommand.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        internal static void ShowRecords()
        {
            Console.Clear();

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand showAllCommand = connection.CreateCommand();
                showAllCommand.CommandText = $@"SELECT * FROM coding_hours";

                List<CodingSession> codingSessions = new();

                SqliteDataReader reader = showAllCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        codingSessions.Add(
                            new CodingSession
                            {
                                Id = reader.GetInt32(0),
                                StartTime = DateTime.Parse(reader.GetString(1)),
                                EndTime = DateTime.Parse(reader.GetString(2)),
                                Duration = TimeSpan.Parse(reader.GetString(3))
                            }
                        );
                    }
                    ConsoleTableBuilder.From(codingSessions).ExportAndWriteLine();

                    Console.WriteLine("Hit Enter/Return Key");
                    Console.ReadLine();
                }

                else
                {
                    Console.WriteLine("No records found");
                }
                connection.Close();
            }
        }

        internal static void UpdateRecord()
        {
            Console.Clear();
            ShowRecords();

            int recordId = UserInput.GetNumberInput("\n\nPlease type the Id of the record you want to update");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand selectCmd = connection.CreateCommand();
                selectCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_hours WHERE Id = {recordId})";
                int checkQuery = Convert.ToInt32(selectCmd.ExecuteScalar());

                if (checkQuery == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist \n\n");
                    connection.Close();
                    UpdateRecord();
                }

                Console.WriteLine("\n\nStart date. (Format: dd-MM-yy HH:mm)");
                string startDate = UserInput.GetDateInput();

                Console.WriteLine("\n\nEnd date. (Format: dd-MM-yy HH:mm)");
                string endDate = UserInput.GetDateInput();

                bool isValidatedTime = Validation.IsValidatedTimes(startDate, endDate);

                if (!isValidatedTime)
                {
                    Console.WriteLine("\nEnd Date cannot be lower than the Start Date\n");
                    UpdateRecord();
                }

                else
                {
                    TimeSpan duration = CalculateDuration(startDate, endDate);
                    string formattedDuration = duration.ToString(@"hh\:mm");

                    SqliteCommand updateCmd = connection.CreateCommand();
                    updateCmd.CommandText = $@"UPDATE coding_hours SET starttime = '{startDate}', 
                        endtime = '{endDate}', duration = '{formattedDuration}' WHERE Id = {recordId}";

                    updateCmd.ExecuteNonQuery();
                }

                connection.Close();
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was updated\n\n");
            Console.WriteLine("Hit Enter/Return Key to return to Main Menu");
            
            Console.ReadLine();
            UserInput.GetUserInput();
        }

        internal static void DeleteRecord()
        {
            Console.Clear();
            ShowRecords();

            int recordId = UserInput.GetNumberInput("\n\nPlease type the Id of the record you want to delete");

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                SqliteCommand deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = $"DELETE FROM coding_hours WHERE Id = '{recordId}'";

                int rowCount = deleteCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist \n\n");
                    DeleteRecord();
                }

                connection.Close();
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was deleted\n\n");
            Console.ReadLine();
            
            UserInput.GetUserInput();

        }

        internal static TimeSpan CalculateDuration(string sTime, string eTime)
        {
            DateTime startDate = DateTime.Parse(sTime);
            DateTime endDate = DateTime.Parse(eTime);

            TimeSpan duration = endDate - startDate;

            return duration;
        }
    }
}
