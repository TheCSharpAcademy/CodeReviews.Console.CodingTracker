using System.Configuration;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class CodingController
    {
        static string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        internal void Get()
        {
            TableVisualisation tableVisualisation = new();
            List<CodingSession> tableData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd;

                tableCmd = $"SELECT * FROM coding_hours ORDER BY date";

                tableData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            Console.Clear();

            if (tableData.Count == 0)
            {
                Console.WriteLine("No records found. Press enter to go back to main menu.");
                Console.ReadLine();
            }
            if (tableData.Count > 0)
                tableVisualisation.ShowTable(tableData);
        }

        internal void Post(CodingSession coding)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"INSERT INTO coding_hours (date, duration) VALUES ('{coding.Date}', '{coding.Duration}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal int Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM coding_hours WHERE Id = '{id}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();

                return rowCount;
            }
        }

        internal void Update(int id)
        {
            GetUserInput userInput = new();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"SELECT EXISTS(SELECT 1 FROM coding_hours WHERE Id = '{id}')";

                int rowCount = Convert.ToInt32(tableCmd.ExecuteScalar());

                if (rowCount == 0)
                {
                    Console.WriteLine($"Record with id {id} doesn't exist.");
                    Console.ReadLine();
                    userInput.ProcessUpdate();
                }

                string date = userInput.GetDateInput("Type the date you want to update to: ");
                string[] info = userInput.CalculateDuration();

                tableCmd.CommandText = $"UPDATE coding_hours SET date = '{date}', duration = '{info[2]}' WHERE Id = {id}";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal void Filter(int startDate, int endDate)
        {
            GetUserInput getUserInput = new();
            TableVisualisation tableVisualisation = new();
            List<CodingSession> tableData = new();
            List<CodingSession> filterData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd = $"SELECT * FROM coding_hours";

                tableData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            int[] dateRecords = new int[tableData.Count];
            int count = 0;
            foreach (var record in tableData)
            {
                string? date = record.Date.ToString();
                string[] dates = date.Split('-');
                Array.Reverse(dates);
                string time = "";
                foreach (var element in dates)
                {
                    time += element;
                }
                dateRecords[count] = Convert.ToInt32(time);
                count++;
            }
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT EXISTS(SELECT date FROM coding_hours)";

                int rowCount = Convert.ToInt32(tableCmd.ExecuteScalar());

                if (rowCount == 0)
                {
                    Console.WriteLine("No dates found. Press Enter to go back to main menu");
                    Console.ReadLine();
                    getUserInput.MainMenu();
                }
                count = 0;
                foreach (var element in tableData)
                {
                    tableCmd.CommandText = $"UPDATE coding_hours SET date = '{dateRecords[count]}' WHERE Id = '{element.Id}'";
                    tableCmd.ExecuteNonQuery();
                    count++;
                }
                connection.Close();
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd = $"SELECT * FROM coding_hours WHERE date BETWEEN '{startDate}' AND '{endDate}' ORDER BY date";

                filterData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            if (tableData.Count == 0)
            {
                Console.WriteLine("No records found. Press enter to go back to main menu.");
                Console.ReadLine();
            }
            if (tableData.Count > 0)
                tableVisualisation.FilteredTable(filterData);
        }

        internal string[] Report()
        {
            List<CodingSession> tableData = new();
            string[] info = new string[2];
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd = $"SELECT * FROM coding_hours";

                tableData = connection.Query<CodingSession>(tableCmd).ToList();

                connection.Close();
            }
            TimeSpan duration;
            TimeSpan totalDuration = new();
            
            foreach (var element in tableData)
            {
                if (tableData.Count < 1) element.Duration = "00:00:00";
                duration = TimeSpan.Parse(element.Duration);
                totalDuration += duration;
            }
            TimeSpan averageDuration = new TimeSpan(0, 0, 0);
            if (tableData.Count > 1)
            averageDuration = TimeSpan.FromSeconds(totalDuration.TotalSeconds / tableData.Count);

            info[0] = string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", totalDuration);
            info[1] = string.Format("{0:%h} hours {0:%m} minutes {0:%s} seconds", averageDuration);

            return info;

        }

        internal void ViewGoals()
        {
            GetUserInput getUserInput = new();
            TableVisualisation tableVisualisation = new();
            List<Goals> tableData = new();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string tableCmd;

                tableCmd = $"SELECT * FROM coding_goals";

                tableData = connection.Query<Goals>(tableCmd).ToList();

                connection.Close();
            }
            if (tableData.Count == 0)
            {
                Console.WriteLine("No goals found. Press enter to go back to main menu.");
                Console.ReadLine();
            }

            foreach (var goal in tableData)
            {
                string[] info = getUserInput.GetDueGoalInfo(Convert.ToInt32(goal.Hours), goal.Date);
                goal.RemainingDays = info[0];
                goal.RemainingHours = info[1];
                goal.HoursPerDay = info[2];
            }
            if (tableData.Count > 0)
            tableVisualisation.ShowGoalsTable(tableData);
        }

        internal void InsertGoal(Goals goals)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = @$"INSERT INTO 
                coding_goals (hours, date, remainingdays, remaininghours, hoursperday) 
                VALUES ('{goals.Hours}', '{goals.Date}', '{goals.RemainingDays}', '{goals.RemainingHours}', 
                '{goals.HoursPerDay}')";

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        internal int DeleteGoal(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = $"DELETE FROM coding_goals WHERE Id = '{id}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                connection.Close();

                return rowCount;
            }
        }
    }
}