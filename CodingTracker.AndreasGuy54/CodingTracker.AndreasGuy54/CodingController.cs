using ConsoleTableExt;
using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker.AndreasGuy54
{
    internal static class CodingController
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("connString");

        internal static void InsertRecord()
        {
            Console.WriteLine("\n\nStart date. (Format: dd-MM-yy HH:mm)");
            string startDate = UserInput.GetDateInput();

            Console.WriteLine("\n\nEnd date. (Format: dd-MM-yy HH:mm)");
            string endDate = UserInput.GetDateInput();

            TimeSpan duration = CalculateDuration(startDate, endDate);
            string formattedDuration = duration.ToString(@"hh\:mm");
            //string formattedDuration = duration.ToString("hh:mm");

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

                    Console.WriteLine("Hit Enter/Return Key to return to Main Menu");
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

                TimeSpan duration = CalculateDuration(startDate, endDate);
                string formattedDuration = duration.ToString(@"hh\:mm");

                SqliteCommand updateCmd = connection.CreateCommand();
                updateCmd.CommandText = $@"UPDATE coding_hours SET starttime = '{startDate}', 
                        endtime = '{endDate}', duration = '{formattedDuration}' WHERE Id = {recordId}";

                updateCmd.ExecuteNonQuery();
                connection.Close();
            }

            Console.WriteLine($"\n\nRecord with Id {recordId} was updated\n\n");
            Console.WriteLine("Hit Enter/Return Key to return to Main Menu");
            
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
