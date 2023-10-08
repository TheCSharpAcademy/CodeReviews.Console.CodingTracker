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

        internal static TimeSpan CalculateDuration(string sTime, string eTime)
        {
            DateTime startDate = DateTime.Parse(sTime);
            DateTime endDate = DateTime.Parse(eTime);

            TimeSpan duration = endDate - startDate;

            return duration;
        }
    }
}
