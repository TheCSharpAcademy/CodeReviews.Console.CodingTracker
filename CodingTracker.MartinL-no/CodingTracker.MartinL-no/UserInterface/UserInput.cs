using CodingTracker.MartinL_no.Controllers;
using CodingTracker.MartinL_no.Models;

namespace CodingTracker.MartinL_no.UserInterface;

internal class UserInput
{
    private readonly CodingController _controller;
    private readonly DateValidator DateValidator;

    public UserInput(CodingController controller, DateValidator dateValidator)
	{
        _controller = controller;
        DateValidator = dateValidator;
    }

    public void Execute()
    {
        while (true)
        {
            ShowMainMenuOptions();
            var op = Ask("Your choice: ");

            switch (op.ToUpper())
            {
                case "S":
                    StartCodingSession();
                    break;
                case "A":
                    AddCodingSession();
                    break;
                case "U":
                    UpdateCodingSession();
                    break;
                case "D":
                    DeleteCodingSession();
                    break;
                case "G":
                    AddCodingGoal();
                    break;
                case "V":
                    RecordsReports();
                    break;
                case "0":
                    ShowMessage("Program ended");
                    return;
                default:
                    ShowMessage("Invalid option, please try again");
                    break;
            }
        }
    }

    private void ShowMainMenuOptions()
    {
        ShowHeader("Welcome to the Coding Tracker app!");

        Console.WriteLine("""
            Select an option:
                S - Start coding session
                A - Add coding session
                U - Update coding session
                D - Delete coding session
                G - Add coding goal
                V - View records/reports
                0 - Exit program
            """);

        ShowLine();
    }

    private void StartCodingSession()
    {
        var startTime = _controller.StartSession();
        var currentTime = startTime;

        while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
        {
            currentTime = currentTime.AddSeconds(1);

            ShowHeader("Recording Coding Session");

            Console.WriteLine($"Start time: {startTime.ToString("HH:mm:ss").PadLeft(12)}\n");
            Console.Write($"Current time: ");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{currentTime.ToString("HH:mm:ss").PadLeft(10)}");
            Console.ResetColor();

            ShowLine();
            Console.WriteLine("Press ESC to end session");
            Thread.Sleep(1000);
        }

        _controller.InsertCodingSession(startTime.ToString(), currentTime.ToString());

        ShowMessage("Coding session added!");
    }

    private void AddCodingSession()
    {
        while (true)
        {
            ShowHeader("Add coding session");

            var startTime = Ask($"When did you start coding (input must be in format - {DateValidator.Format}): ");
            var endTime = Ask($"When did you end coding (input must be in format - {DateValidator.Format}): ");

            if (!DateValidator.AreValidDates(startTime, endTime))
            {
                ShowMessage("Dates are invalid please try again");
                continue;
            }

            var isAdded = _controller.InsertCodingSession(startTime, endTime);

            if (isAdded)
            {
                ShowMessage("Session added!");
                break;
            }

            else ShowMessage("Invalid adding session, please try again");
        }
    }

    private void UpdateCodingSession()
    {
        while (true)
        {
            ShowHeader("Update coding session");

            var sessions = _controller.GetCodingSessions().OrderByDescending(s => s.StartTime).ToList();
            TableVisualizationEngine.ShowTable(sessions);

            ShowLine();

            var id = Ask("Enter the id of the session you would like to update: ");

            var startTime = Ask($"Enter the new start time (input must be in format - {DateValidator.Format}): ");
            var endTime = Ask($"Enter the new end time (input must be in format - {DateValidator.Format}): ");
            var intId = 0;

            if (!DateValidator.AreValidDates(startTime, endTime))
            {
                ShowMessage("Dates/ date format are invalid please try again");
                continue;
            }
            else if (Int32.TryParse(id, out intId) && _controller.UpdateCodingSession(intId, startTime, endTime))
            {
                ShowMessage("Session updated!");
                break;
            }

            else ShowMessage("Invalid input please try again");
        }
    }

    private void DeleteCodingSession()
    {
        while (true)
        {
            ShowHeader("Delete coding session");
            ShowAllSessions();

            var id = Ask("Enter the Id of the session you would like to delete: ");
            var intId = 0;

            if (Int32.TryParse(id, out intId) && _controller.DeleteCodingSession(intId))
            {
                ShowMessage("Session deleted!");
                break;
            }

            else ShowMessage("Incorrect Id please try again");
        }
    }

    private void AddCodingGoal()
    {
        while (true)
        {
            ShowHeader("Add coding goal");
            ShowAllSessions();

            var stringHours = Ask("How many hours of coding would you like to complete: ");
            var endTime = Ask($"When is the deadline (input must be in format - {DateValidator.Format})");

            var hours = 0;

            if (Int32.TryParse(stringHours, out hours) && DateValidator.IsCorrectFormat(endTime, out _) && _controller.InsertCodingGoal(endTime, hours))
            {
                ShowMessage("Coding goal added!");
                break;
            }

            else ShowMessage("Invalid entry please try again");
        }
    }

    private void RecordsReports()
    {
        while (true)
        {
            ShowRecordsReportsMenu();

            var op = Ask("Your choice: ");

            switch (op.ToUpper())
            {
                case "A":
                    ShowAllSessions();
                    break;
                case "G":
                    ShowCodingGoals();
                    break;
                case "D":
                    PreviousDaysMenu();
                    break;
                case "M":
                    PreviousMonthsMenu();
                    break;
                case "Y":
                    PreviousYearsMenu();
                    break;
                case "SD":
                    ShowStatisticsByDay();
                    break;
                case "SW":
                    ShowStatisticsByWeek();
                    break;
                case "SM":
                    ShowStatisticsByMonth();
                    break;
                case "SY":
                    ShowStatisticsByYear();
                    break;
                case "0":
                    return;
                default:
                    ShowMessage("Invalid option, please try again");
                    break;
            }
        }
    }

    private void ShowRecordsReportsMenu()
    {
        ShowHeader("View records/reports");

        Console.WriteLine("""
            Select an option:
                A  - All sessions
                G  - Coding goals
                D  - View by previous amount of days
                M  - View by previous amount of months
                Y  - View by previous amount of years
                SD - Statistics by day
                SW - Statistics bt week
                SM - Statistics by month
                SY - Statistics by year
                0  - Return to main menu
            """);

        ShowLine();
    }

    private void ShowAllSessions()
    {
        var sessions = _controller.GetCodingSessions().OrderByDescending(s => s.StartTime).ToList();
        ShowSessionTablePage("All sessions", sessions);
    }

    private void ShowSessionTablePage(string pageTitle, List<CodingSession> sessions)
    {
        ShowHeader(pageTitle);

        TableVisualizationEngine.ShowTable(sessions);

        ShowFooter();
    }

    private void ShowCodingGoals()
    {
        var goals = _controller.GetCodingGoals().OrderBy(g => g.StartTime).ToList();

        ShowHeader("Coding goals");

        TableVisualizationEngine.ShowTable(goals);

        ShowFooter();
    }

    private void PreviousDaysMenu()
    {
        while (true)
        {
            ShowHeader("View records/reports");

            var daysString = Ask("How many days back would you like to see reports for: ");
            var order = Ask("View starting from newest date (y/n)? ");
            var days = 0;

            if (Int32.TryParse(daysString, out days))
            {
                var sessions = _controller.GetCodingSessionsByDays(days);

                if (order == "y") sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                else sessions = sessions.OrderBy(s => s.StartTime).ToList();

                var title = $"Sessions in the past {(days > 1 ? days.ToString() + " days" : "day")}";
                ShowSessionTablePage(title, sessions);

                break;
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void PreviousMonthsMenu()
    {
        while (true)
        {
            ShowHeader("View records/reports");
            var monthsString = Ask("How many previous months would you like to see reports for: ");
            var order = Ask("View starting from newest date (y/n)? ");

            var months = 0;

            if (Int32.TryParse(monthsString, out months))
            {
                var sessions = _controller.GetCodingSessionsByYears(months);

                if (order == "y") sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                else sessions = sessions.OrderBy(s => s.StartTime).ToList();

                var title = $"Sessions in the past {(months > 1 ? months.ToString() + " months" : "month")}";
                ShowSessionTablePage(title, sessions);

                break;
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void PreviousYearsMenu()
    {
        while (true)
        {
            ShowHeader("View records/reports");
            var yearsString = Ask("How many previous years would you like to see reports for: ");
            var order = Ask("View starting from newest date (y/n)? ");

            var years = 0;

            if (Int32.TryParse(yearsString, out years))
            {
                var sessions = _controller.GetCodingSessionsByYears(years);

                if (order == "y") sessions = sessions.OrderByDescending(s => s.StartTime).ToList();
                else sessions = sessions.OrderBy(s => s.StartTime).ToList();

                var title = $"Sessions in the past {(years > 1 ? years.ToString() + " years" : "year")}";
                ShowSessionTablePage(title, sessions);

                break;
            }

            else ShowMessage("Invalid entry, please try again");
        }
    }

    private void ShowStatisticsByDay()
    {
        var statistics = _controller.GetStatisicsByDay().OrderBy(s => s.StartDate).ToList();
        ShowStatisticsTablePage("Statistics by Day", statistics);
    }

    private void ShowStatisticsByWeek()
    {
        var statistics = _controller.GetStatisicsByWeek().OrderBy(s => s.StartDate).ToList();
        ShowStatisticsTablePage("Statistics by Week", statistics);
    }

    private void ShowStatisticsByMonth()
    {
        var statistics = _controller.GetStatisicsByMonth().OrderBy(s => s.StartDate).ToList();
        ShowStatisticsTablePage("Statistics by Month", statistics);
    }

    private void ShowStatisticsByYear()
    {
        var statistics = _controller.GetStatisicsByYear().OrderBy(s => s.StartDate).ToList();
        ShowStatisticsTablePage("Statistics by Year", statistics);
    }

    private void ShowStatisticsTablePage(string title, List<CodingStatistic> statistics)
    {
        ShowHeader(title);

        TableVisualizationEngine.ShowTable(statistics);

        ShowFooter();
    }

    private void ShowHeader(string title)
    {
        Console.Clear();
        Console.WriteLine(title);
        Console.WriteLine("---------------------------------\n");
    }

    private void ShowFooter()
    {
        ShowLine();
        Console.Write("Press any key to return to the menu ");
        Console.ReadKey();
    }

    private void ShowLine()
    {
        Console.WriteLine("\n---------------------------------");
    }

    private void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        Thread.Sleep(2500);
    }

    private string Ask(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }
}
