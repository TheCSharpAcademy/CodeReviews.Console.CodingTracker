using Microsoft.Data.Sqlite;
namespace CodingTracker
{
    public class UserInput
    {
        static string connectionString = System.Configuration.ConfigurationManager.AppSettings["connectionString"];

        public static void GetUserInput()
        {
            bool exit = false;
            Console.WriteLine($"\nWelcome to JsPeanut's CodingTracker! You can start tracking your coding sessions. Here you've got a list of all the available commands in the application: \n\n C: Insert the dates in which you started and finished your session, to calculate it. \n S: Start tracking a coding session via stopwatch. \n R: See all your coding sessions.\n U: Update a coding session.\n D: Delete a coding session.\n E: Exit the application.\n G: Set a new goal.\n T: See your goals.");
            CodingController.GetAllGoalRecords();
            CodingController.GetAllRecords("load");
            CodingController.CodingSessions.Clear();

            TimeSpan progressRemaining = TimeSpan.Zero;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText = "SELECT ProgressRemaining FROM goals";
                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        progressRemaining = TimeSpan.Parse(reader.GetString(0));
                    }
                }
                if (CodingController.Goals.Count != 0)
                {
                    if (CodingController.Goals.FirstOrDefault().ProgressRemaining != TimeSpan.Zero || CodingController.Goals.FirstOrDefault().ProgressRemaining < TimeSpan.Zero)
                    {
                        Console.WriteLine($"Reminder: You are {progressRemaining} apart from completing your last goal! ({CodingController.Goals.FirstOrDefault().GoalValue})");
                    }
                }
            }

            string userInput = Console.ReadLine();
            while (exit == false)
            {
                switch (userInput)
                {
                    case "C":
                        Console.Clear();
                        CodingController.Insert();
                        break;
                    case "S":
                        CodingController.StopwatchSession();
                        break;
                    case "R":
                        Console.Clear();
                        CodingController.GetAllRecords("display", "filter");
                        break;
                    case "U":
                        CodingController.Update();
                        break;
                    case "D":
                        CodingController.Delete();
                        break;
                    case "E":
                        Console.WriteLine("You have exited the app successfully.");
                        exit = true;
                        Environment.Exit(0);
                        break;
                    case "G":
                        CodingController.SetGoal();
                        break;
                    case "T":
                        CodingController.GetAllGoalRecords();
                        break;
                    default:
                        Console.WriteLine("That option does not exist.");
                        GetUserInput();
                        break;
                }
            }
        }

        public static string GetStartTimeInput()
        {
            string format = "dd/MM/yyyy HH:mm";

            Console.WriteLine("Please input the date and the hour in which you started the coding session in the following format: dd/mm/yyyy HH:MM \n\nThe hour MUST be in 24 hour format. \n\nType M if you want to get back to the main menu.");

            string date = Console.ReadLine();

            if (date == "M")
            {
                GetUserInput();
                Console.Clear();
            }

            Validation.ValidateDate(date, GetStartTimeInput, format);

            return date;
        }

        public static string GetEndTimeInput()
        {
            string format = "dd/MM/yyyy HH:mm";

            Console.WriteLine("Please input the date and the hour in which the coding session ended in the following format: dd/mm/yyyy HH:MM \n\nThe hour MUST be in 24 hour format. \n\nType M if you want to get back to the main menu.");

            string date = Console.ReadLine();

            if (date == "M")
            {
                GetUserInput();
                Console.Clear();
            }

            Validation.ValidateDate(date, GetEndTimeInput, format);
            return date;
        }

        public static string GetGoalMeasureInput()
        {
            Console.WriteLine("Please type if you want your input to be measured in 'years', 'months', 'days', 'hours' or 'minutes'.");

            string goalTimePeriod = Console.ReadLine();

            return goalTimePeriod;
        }

        public static string GetGoalInput()
        {
            Console.WriteLine("Please type your goal. It must be a number.");

            string goalInput = Console.ReadLine();

            return goalInput;
        }

        public static int GetNumberInput(string message)
        {
            string errorMessage = "\n\nInvalid number. Type M to return to the main menu or try again.\n\n";

            Console.WriteLine(message);

            string numberInput = Console.ReadLine();

            if (numberInput == "M")
            {
                GetUserInput();
                Console.Clear();
            }

            Validation.ValidateNumber(numberInput, () => GetNumberInput(errorMessage));

            int finalInput = Convert.ToInt32(numberInput);

            return finalInput;
        }
    }
}