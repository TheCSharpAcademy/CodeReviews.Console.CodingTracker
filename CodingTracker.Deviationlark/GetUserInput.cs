using System.Diagnostics;
using System.Globalization;
namespace CodingTracker
{
    internal class GetUserInput
    {
        CodingController codingController = new();
        internal void MainMenu()
        {
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("MAIN MENU");
                Console.WriteLine("----------");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. View Coding Menu");
                Console.WriteLine("2. View Goals Menu");

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        Console.WriteLine("Goodbye!");
                        closeApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        CodingMenu();
                        break;
                    case "2":
                        GoalsMenu();
                        break;
                }
            }
        }

        internal void CodingMenu()
        {
            Console.WriteLine("CODING");
            Console.WriteLine("----------");
            Console.WriteLine("0. Main Menu");
            Console.WriteLine("1. View records");
            Console.WriteLine("2. Insert a record");
            Console.WriteLine("3. Update a record");
            Console.WriteLine("4. Delete a record");
            Console.WriteLine("5. Stopwatch");

            string? userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    MainMenu();
                    break;
                case "1":
                    ProcessView();
                    break;
                case "2":
                    ProcessAdd();
                    break;
                case "3":
                    ProcessUpdate();
                    break;
                case "4":
                    ProcessDelete();
                    break;
                case "5":
                    StopWatch();
                    break;
                default:
                    Console.WriteLine("Invalid Command. Please type a number from 0 to 4.");
                    CodingMenu();
                    break;
            }
        }
        internal void GoalsMenu()
        {
            Console.WriteLine("GOALS");
            Console.WriteLine("----------");
            Console.WriteLine("0. Main Menu");
            Console.WriteLine("1. View goals");
            Console.WriteLine("2. Create a goal");
            Console.WriteLine("3. Delete a goal");

            string? userInput = Console.ReadLine();

            switch (userInput)
            {
                case "0":
                    MainMenu();
                    break;
                case "1":
                    codingController.ViewGoals();
                    break;
                case "2":
                    SetGoals();
                    break;
                case "3":
                    RemoveGoal();
                    break;
                default:
                    Console.WriteLine("Invalid Command. Please type a number from 0 to 3.");
                    GoalsMenu();
                    break;
            }
        }

        internal void ProcessView()
        {
            Console.WriteLine("1. View All Records");
            Console.WriteLine("2. Filter Records");
            Console.WriteLine("3. View total and average duration");
            var userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    codingController.Get();
                    break;
                case "2":
                    ProcessFilter();
                    break;
                case "3":
                    ViewReport();
                    break;
                default:
                    Console.WriteLine("That's not one of the options.");
                    Console.ReadLine();
                    ProcessView();
                    break;

            }
        }

        internal void ProcessFilter()
        {
            Console.WriteLine("Type the starting date you want to filter by(dd-mm-yy): ");
            string? userInput = Console.ReadLine();
            while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Wrong Format. Enter the date with the correct format(dd-mm-yy):");
                userInput = Console.ReadLine();
            }
            string startTime = userInput;

            Console.WriteLine("Type the ending date you want to filter by(dd-mm-yy): ");
            userInput = Console.ReadLine();
            while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Wrong Format. Enter the date with the correct format(dd-mm-yy):");
                userInput = Console.ReadLine();
            }
            string endTime = userInput;

            string[] startTimeArray = startTime.Split('-');
            Array.Reverse(startTimeArray);
            string startingTime = "";

            foreach (var element in startTimeArray)
            {
                startingTime += element;
            }

            string[] endTimeArray = endTime.Split('-');
            Array.Reverse(endTimeArray);
            string endingTime = "";

            foreach (var element in endTimeArray)
            {
                endingTime += element;
            }

            codingController.Filter(Convert.ToInt32(startingTime), Convert.ToInt32(endingTime));
        }

        internal void ProcessAdd(string duration = "")
        {
            string[] info = new string[2];
            var date = GetDateInput("Type the date you want to record: ");
            if (duration == "")
            {
                info = CalculateDuration();
                duration = info[2];
            }

            CodingSession coding = new();

            coding.Date = date;
            coding.Duration = duration;

            codingController.Post(coding);
        }

        internal void ProcessDelete()
        {
            codingController.Get();
            int id = GetNumInput("Type the ID of the session you want to delete. Type 0 to go back to Main Menu");

            var coding = codingController.Delete(id);

            while (coding == 0)
            {
                Console.WriteLine($"Record with id {id} doesn't exist");
                Console.ReadLine();

                codingController.Get();
                id = GetNumInput("Type the ID of the session you want to delete. Type 0 to go back to Main Menu");

                coding = codingController.Delete(id);
            }
        }

        internal void ViewReport()
        {
            string[] info = codingController.Report();

            Console.WriteLine($"Total duration of sessions in database: {info[0]}");
            Console.WriteLine($"Average duration: {info[1]}");
            Console.WriteLine("Press enter to go back to main menu.");
            Console.ReadLine();
        }

        internal void ProcessUpdate()
        {
            codingController.Get();
            int id = GetNumInput("Type the ID of the session you want to update. Type 0 to go back to Main Menu");

            codingController.Update(id);
        }

        internal void SetGoals()
        {
            DateTime dueDate = DateTime.Parse(GetDateInput("Type the date you want to finish this goal by: "));
            int hours;
            Goals goals = new();

            Console.WriteLine("Type the amount of hours you want to set a goal for: ");
            string? userInput = Console.ReadLine();

            while (!int.TryParse(userInput, out hours) || int.Parse(userInput) <= 0)
            {
                Console.WriteLine("Wrong format or Number is less than 1. Try again: ");
                userInput = Console.ReadLine();
            }
            TimeSpan goal = new TimeSpan(hours, 0, 0);

            goals.Hours = goal.TotalHours.ToString();
            goals.Date = dueDate.ToString("dd-MM-yy");

            string[] info = GetDueGoalInfo(hours, goals.Date);

            goals.RemainingDays = info[0];
            goals.RemainingHours = info[1];
            goals.HoursPerDay = info[2];


            codingController.InsertGoal(goals);
        }

        internal void RemoveGoal()
        {
            codingController.ViewGoals();
            int id = GetNumInput("Type the ID of the session you want to delete. Type 0 to go back to Main Menu");

            var coding = codingController.DeleteGoal(id);

            while (coding == 0)
            {
                Console.WriteLine($"Record with id {id} doesn't exist");
                Console.ReadLine();

                codingController.ViewGoals();
                id = GetNumInput("Type the ID of the session you want to delete. Type 0 to go back to Main Menu");

                coding = codingController.DeleteGoal(id);
            }

            Console.WriteLine("Goal Deleted. Press enter to go back to main menu.");
            Console.ReadLine();
        }

        internal int GetNumInput(string message)
        {
            bool validNumber;
            int userNum;
            do
            {
                Console.WriteLine(message);
                string? userInput = Console.ReadLine();

                if (userInput == "0") MainMenu();

                validNumber = int.TryParse(userInput, out userNum);
            } while (!validNumber);

            return userNum;
        }

        internal string GetDateInput(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("Type 0 to go back to Main Menu");
            string? userInput = Console.ReadLine();
            if (userInput == "0") MainMenu();

            while (!DateTime.TryParseExact(userInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("Wrong Format. Enter the date with the correct format(dd-mm-yy):");
                userInput = Console.ReadLine();
            }

            return userInput;
        }

        internal string[] CalculateDuration()
        {
            string[] codingInfo = new string[3];

            Console.WriteLine("Type the time you started coding (hh:mm): ");
            Console.WriteLine("Type 0 to go back to Main Menu.");
            string? userInput = Console.ReadLine();
            TimeSpan startTime;
            while (!TimeSpan.TryParseExact(userInput, "h\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out startTime))
            {
                Console.WriteLine("Wrong format. Type the starting time again (hh:mm): ");
                userInput = Console.ReadLine();
            }

            Console.WriteLine("Type the time you stopped coding (hh:mm): ");
            userInput = Console.ReadLine();
            TimeSpan endTime;
            while (!TimeSpan.TryParseExact(userInput, "h\\:mm", CultureInfo.InvariantCulture, TimeSpanStyles.None, out endTime))
            {
                Console.WriteLine("Wrong format. Type the starting time again (hh:mm): ");
                userInput = Console.ReadLine();
            }

            TimeSpan duration = endTime - startTime;

            codingInfo[0] = startTime.ToString();
            codingInfo[1] = endTime.ToString();
            codingInfo[2] = duration.ToString();
            return codingInfo;
        }

        internal string[] GetDueGoalInfo(int hours = 0, string date = "")
        {
            CodingController codingController = new();
            DateTime dueDate = DateTime.Parse(date);
            string[] codingInfo = codingController.Report();
            double totalHours = Convert.ToDouble(codingInfo[0].Substring(0, 2));
            DateTime currentDate = DateTime.Now;
            string[] info = new string[4];

            TimeSpan calculatedDays = dueDate - currentDate;
            double calculatedHours = hours - totalHours;
            double calculatedHoursPerDay = calculatedHours / Convert.ToDouble(calculatedDays.TotalDays);

            string remainingDays = calculatedDays.ToString("%d");
            int remainingHours = Convert.ToInt32(calculatedHours);
            double hoursPerDay;
            hoursPerDay = calculatedHoursPerDay;

            if (remainingHours <= 0)
            {
                info[0] = "Completed";
                info[1] = "Completed";
                info[2] = "Completed";
            }

            else
            {
                info[0] = remainingDays;
                info[1] = remainingHours.ToString();
                info[2] = hoursPerDay.ToString();
                info[3] = calculatedHoursPerDay.ToString();
            }


            return info;
        }

        internal void StopWatch()
        {
            GetUserInput getUserInput = new();
            Console.WriteLine("Starting Stopwatch. Do your work and stop the stopwatch when you're done.");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (!Console.KeyAvailable)
            {
                Console.Clear();
                Console.WriteLine("Elapsed Time: " + stopwatch.Elapsed.ToString(@"hh\:mm\:ss"));
                Console.WriteLine("Press any key to stop the stopwatch.");
                Thread.Sleep(1000);
            }

            stopwatch.Stop();
            string duration = stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
            string? userInput;
            do
            {
                Console.Clear();
                Console.WriteLine("Elapsed Time: " + duration);
                Console.WriteLine("0. Main Menu");
                Console.WriteLine("1. Enter duration in database");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "0":
                        getUserInput.MainMenu();
                        break;
                    case "1":
                        getUserInput.ProcessAdd(duration);
                        break;
                    default:
                        Console.WriteLine("Thats not an option. ");
                        Console.ReadLine();
                        break;
                }
            } while (userInput != "0" || userInput != "1");
        }
    }
}