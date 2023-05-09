using ConsoleTableExt;
using Microsoft.Data.Sqlite;

namespace CodeTracker.csm_stough
{
    public class Calender
    {
        private static string connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("connectionString");

        public Calender()
        {
            Console.WriteLine("Time Per Day This Month ~~~~~~~~~~~~~~~~~");
            ConsoleTableBuilder.From(CreateCurrentCalenderData())
                .WithTitle(GetYearMonth())
                .WithColumn("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday")
                .ExportAndWriteLine(TableAligntment.Left);
        }

        public static List<List<Object>> CreateCurrentCalenderData()
        {
            int chunkSize = 7;
            int daysThisMonth = GetDaysThisMonth();
            int startingOffset = GetDateOfFirst();
            string yearMonth = GetYearMonth();
            List<Object> source = new List<Object>();

            for(int d = 1; d < startingOffset; d++)
            {
                source.Add("");
            }

            for(int d = 0; d < daysThisMonth; d++)
            {
                DateTime date = DateTime.Parse($"{yearMonth}-{(d + 1).ToString().PadLeft(2, '0')}");
                TimeSpan duration = SessionDao.GetTotalDuration(date);
                string durationLabel = duration == TimeSpan.Zero ? "" : ": " + duration.ToString();
                source.Add((d + 1).ToString() + durationLabel);
            }

            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        private static int GetDaysThisMonth()
        {
            int scalar;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT CAST(STRFTIME('%d', DATE('now', 'start of month','+1 month', '-1 day')) AS INTEGER)";

                scalar = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
            return scalar;
        }

        private static int GetDateOfFirst()
        {
            int scalar;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT strftime('%w','now', 'start of day')";

                scalar = Convert.ToInt32(command.ExecuteScalar());

                connection.Close();
            }
            return scalar;
        }

        private static string GetYearMonth()
        {
            string text = string.Empty;

            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                SqliteCommand command = connection.CreateCommand();

                command.CommandText = "SELECT STRFTIME('%Y-%m', 'now')";

                SqliteDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        text = reader.GetString(0);
                    }
                }

                connection.Close();
            }

            return text;
        }
    }
}
