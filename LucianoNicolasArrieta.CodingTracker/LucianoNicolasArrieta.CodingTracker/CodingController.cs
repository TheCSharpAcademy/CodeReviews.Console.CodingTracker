using System.Configuration;
using System.Data.SQLite;

namespace coding_tracker
{
    internal class CodingController
    {
        string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        public void Insert(CodingSession codingSession)
        {
            string start_time = codingSession.StartTime.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string end_time = codingSession.EndTime.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string duration = codingSession.Duration.Hours.ToString() + "h " + codingSession.Duration.Minutes.ToString() + "min";

            using (SQLiteConnection myConnection = new SQLiteConnection(connectionString))
            {
                string query = "INSERT INTO coding_tracker ('start_time', 'end_time', 'duration') VALUES (@start_time, @end_time, @duration)";
                SQLiteCommand command = new SQLiteCommand(query, myConnection);

                myConnection.Open();

                command.Parameters.AddWithValue("@start_time", start_time);
                command.Parameters.AddWithValue("@end_time", end_time);
                command.Parameters.AddWithValue("@duration", duration);
                command.ExecuteNonQuery();
            }

            Console.Clear();
            Console.WriteLine("Record added to database successfully!");
        }
    }
}