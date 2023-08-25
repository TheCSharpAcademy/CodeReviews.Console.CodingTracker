using CodingTracker.library.View;
using Microsoft.Data.Sqlite;

namespace CodingTracker.library.Controller;

internal class QueriesPeriod
{
    private static string connectionString = Queries.ConnectionString;

    internal static void CodingSessionPerYearQuery(string year)
    {
        bool possible = PossibleYearQuery(year);
        if (possible)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $@"SELECT SUM(Duration) FROM sessions WHERE StartTime LIKE '%{year}%' AND EndTime LIKE '%{year}%'";

                SqliteDataReader reader = tableCommand.ExecuteReader();

               
                if (reader.Read())
                {
                    TableVisualizationEngine.PrintSingleValue(reader.GetDouble(0), $"Total Coding Session Duration in {year} in minutes");
                }

                connection.Close();
            }
        }

        else
        {
            Console.WriteLine($"No session that starts in year {year} exists.\n\nPress any key to get back to report menu...");
            Console.ReadKey();
        }
    }

    private static bool PossibleYearQuery(string year)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"SELECT EXISTS (SELECT 1 FROM sessions WHERE StartTime Like '%{year}%') ";

            int rowCount = Convert.ToInt32(tableCommand.ExecuteScalar());

            if (rowCount == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                connection.Close();
                return true;
            }
        }
    }

    internal static void CodingSessionPerMonthQuery(string year, string month, string monthName)
    {
        bool possible = PossibleMonthQuery(year, month);

        if (possible)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $@"SELECT SUM(Duration) FROM sessions WHERE StartTime LIKE '%{month}-{year}%' AND EndTime LIKE '%{month}-{year}%'";

                SqliteDataReader reader = tableCommand.ExecuteReader();

                if (reader.Read())
                {
                    TableVisualizationEngine.PrintSingleValue(reader.GetDouble(0), $"Total Coding Session Duration in {monthName} {year} in minutes");
                }

                connection.Close();
            }
        }

        else
        {
            Console.Clear();
            Console.WriteLine($"No session that starts in month {monthName} in {year} year exists.\n\nPress any key to get back to report menu...");
            Console.ReadKey();
        }
    }

    private static bool PossibleMonthQuery(string year, string month)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"SELECT EXISTS (SELECT 1 FROM sessions WHERE StartTime Like '%{month}-{year}%') ";

            int rowCount = Convert.ToInt32(tableCommand.ExecuteScalar());

            if (rowCount == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                connection.Close();
                return true;
            }
        }
    }

    internal static void CodingSessionPerDayQuery(string year, string month, string day, string monthName)
    {
        bool possible = PossibleDayQuery(year, month, day);
        if (possible)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $@"SELECT SUM(Duration) FROM sessions WHERE StartTime LIKE '{day}-{month}-{year}%' AND EndTime LIKE '%{month}-{year}%'";

                SqliteDataReader reader = tableCommand.ExecuteReader();

                if (reader.Read())
                {
                    TableVisualizationEngine.PrintSingleValue(reader.GetDouble(0), $"Total Coding Session Duration in {day} of {monthName} {year} in minutes");
                }

                connection.Close();
            }
        }

        else
        {
            Console.Clear();
            Console.WriteLine($"No session that starts in {day} of {monthName} in {year} year exists.\n\nPress any key to get back to report menu...");
            Console.ReadKey();
        }
    }

    private static bool PossibleDayQuery(string year, string month, string day)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"SELECT EXISTS (SELECT 1 FROM sessions WHERE StartTime Like '{day}-{month}-{year}%' AND EndTime LIKE '%{month}-{year}%') ";

            int rowCount = Convert.ToInt32(tableCommand.ExecuteScalar());

            if (rowCount == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                connection.Close();
                return true;
            }
        }
    }

    internal static void CodingSessionPerWeekQuery(string year, string month, string day, string monthName)
    {
        bool possible = PossibleWeekQuery(year, month, day);
        if (possible)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCommand = connection.CreateCommand();

                tableCommand.CommandText = $@"SELECT SUM(Duration) FROM sessions WHERE StartTime LIKE '{day}-{month}-{year}%' AND Duration <= 10080";

                SqliteDataReader reader = tableCommand.ExecuteReader();

                if (reader.Read())
                {
                    TableVisualizationEngine.PrintSingleValue(reader.GetDouble(0), $"Total Coding Session Duration in week where first day is: {day} of {monthName} {year} in minutes");
                }

                connection.Close();
            }
        }

        else
        {
            Console.Clear();
            Console.WriteLine($"No session that starts in {day} of {monthName} in {year} year and lasts for a week exists.\n\nPress any key to get back to report menu...");
            Console.ReadKey();
        }
    }

    private static bool PossibleWeekQuery(string year, string month, string day)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCommand = connection.CreateCommand();

            tableCommand.CommandText = $"SELECT EXISTS (SELECT 1 FROM sessions WHERE StartTime Like '{day}-{month}-{year}%' and Duration <= 10080) ";

            int rowCount = Convert.ToInt32(tableCommand.ExecuteScalar());

            if (rowCount == 0)
            {
                connection.Close();
                return false;
            }

            else
            {
                connection.Close();
                return true;
            }
        }
    }

}
