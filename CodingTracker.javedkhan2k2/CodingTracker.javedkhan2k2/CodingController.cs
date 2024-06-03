using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal class CodingController
{
    internal CodingTrackerDbContext dbContext;

    public CodingController(string connectionString)
    {
        dbContext = new CodingTrackerDbContext(connectionString);
    }

    internal void RunApp()
    {
        bool runApplication = true;

        while (runApplication)
        {
            Console.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What do you want to do?")
                    //.PageSize(3)
                    .AddChoices(new[] {
                        "View All Sessions",
                        "Add New Session",
                        "Add New Session using Stop Watch",
                        "Update Session",
                        "Delete Session",
                        "Reports",
                        "Exit"
                    }));
            switch (choice)
            {
                case "Add New Session":
                    AddNewSession();
                    break;
                case "Add New Session using Stop Watch":
                    AddSessionUsingStopWatch();
                    break;
                case "Update Session":
                    UpdateSession();
                    break;
                case "View All Sessions":
                    ViewAllSessions();
                    AnsiConsole.Markup("Press [green]Enter[/] to continue");
                    Console.ReadLine();
                    break;
                case "Delete Session":
                    DeleteSession();
                    break;
                case "Reports":
                    Reports();
                    break;
                case "Exit":
                    runApplication = false;
                    break;
            }
        }
    }

    private void Reports()
    {
        while (true)
        {
            AnsiConsole.Clear();
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a Report")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                    "Daily Report",
                    "Weekly Report",
                    "Monthly Report",
                    "Yearly Report",
                    "Back to Main Menu",
                    "Exit"
                    }
            ));
            switch (choice)
            {
                case "Daily Report":
                case "Weekly Report":
                case "Monthly Report":
                    GenerateReport(choice);
                    break;
                case "Yearly Report":
                    GenerateYearlyReport();
                    break;
                case "Back to Main Menu":
                    return;
                case "Exit":
                    System.Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }
    }

    private void GenerateReport(string choice)
    {
        ReportDto reportInput = UserInput.GetDailyReportInput();
        if (reportInput != null)
        {
            switch (choice)
            {
                case "Daily Report":
                    VisualisationEngine.DisplayDailyReport(dbContext.GetDailyReport(reportInput), reportInput);
                    break;
                case "Weekly Report":
                    VisualisationEngine.DisplayWeeklyReport(dbContext.GetWeeklyReport(reportInput), reportInput);
                    break;
                case "Monthly Report":
                    VisualisationEngine.DisplayMonthlyReport(dbContext.GetMonthlyReport(reportInput), reportInput);
                    break;
            }
            AnsiConsole.Markup("Press [green]Enter[/] to continue");
            Console.ReadLine();
        }
    }

    private void GenerateYearlyReport()
    {
        ReportDto reportInput = UserInput.GetYearlyReportInput();
        if (reportInput != null)
        {
            VisualisationEngine.DisplayYearlyReport(dbContext.GetYearlyReport(reportInput), reportInput);
            AnsiConsole.Markup("Press [green]Enter[/] to continue");
            Console.ReadLine();
        }
    }

    private void AddSessionUsingStopWatch()
    {
        var startInput = UserInput.GetStopWatchInput("Press [bold green]'S'[/] to start the timer or Press [bold green]'0'[/] to cancel: ");
        if (!startInput)
        {
            AnsiConsole.Markup("[bold orange1]User canceled the Stopwatch.[/]\n");
            AnsiConsole.Markup("Press [green]Enter[/] to continue: ");
            Console.ReadLine();
            return;
        }
        StopWatch stopWatch = new StopWatch();
        stopWatch.StartDateTime = DateTime.Now;
        AnsiConsole.Markup($"Start Time: [bold green]{stopWatch.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss")}[/]\n");
        var input = UserInput.GetStopWatchInput("Press [bold green]'S'[/] to stop the timer or Press [bold green]'0'[/] to cancel: ");
        if (input)
        {
            stopWatch.EndDateTime = DateTime.Now;
            AnsiConsole.Markup($"End Time: [bold green]{stopWatch.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss")}[/]\n");
            long duration = (long)stopWatch.EndDateTime.Subtract(stopWatch.StartDateTime).Duration().TotalSeconds;
            CodingSessionDto codingSessionDto = new CodingSessionDto
            {
                StartTime = stopWatch.StartDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = stopWatch.EndDateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                Duration = duration
            };
            dbContext.AddSessionCode(codingSessionDto);
            AnsiConsole.Markup("[bold green]New Coding Session added successfully.[/]\n");
        }
        else
        {
            AnsiConsole.Markup("[bold orange1]User canceled the Stopwatch.[/]\n");
        }
        AnsiConsole.Markup("Press [green]Enter[/] to continue");
        Console.ReadLine();
    }

    private void UpdateSession()
    {
        ViewAllSessions();
        int id = UserInput.GetIntegerValue("To update a Coding Session, please Enter an Id from the above Table: ");
        if (id == 0)
        {
            AnsiConsole.Markup("[bold orange1]User canceled the Update Operation.[/]\n");
            AnsiConsole.Markup("Press [green]Enter[/] to continue: ");
            Console.ReadLine();
            return;
        }
        CodingSession? record = dbContext.GetCodingSessionById(id);
        if (record != null)
        {
            CodingSessionDto codingSession = UserInput.UpdateCodingSessionDialog(record);
            if (codingSession != null)
            {
                if (dbContext.UpdateSessionCode(id, codingSession))
                {
                    AnsiConsole.Markup("[bold green]Coding Session updated successfully[/]");
                }
                else
                {
                    Console.WriteLine($"The record with id {id} could not found in database");
                }
            }
            else
            {
                AnsiConsole.Markup("[bold orange1]User canceled the Update Operation.[/]\n");
                AnsiConsole.Markup("Press [green]Enter[/] to continue: ");
                Console.ReadLine();
                return;
            }
        }
        else
        {
            Console.WriteLine($"The record with id {id} could not found in database");
        }
        AnsiConsole.Markup("Press [green]Enter[/] to continue");
        Console.ReadLine();
    }

    private void DeleteSession()
    {
        ViewAllSessions();
        int id = UserInput.GetIntegerValue("To Delete a Coding Session, please Enter an Id from the above Table: ");
        if (id == 0)
        {
            AnsiConsole.Markup("[bold orange1]User canceled the Delete Operation.[/]\n");
            AnsiConsole.Markup("Press [green]Enter[/] to continue: ");
            Console.ReadLine();
            return;
        }
        if (dbContext.DeleteSessionCode(id))
        {
            AnsiConsole.Markup($"[bold green]Coding Session deleted successfully[/]\n");
        }
        else
        {
            Console.WriteLine($"The record with id {id} could not found in database\n");
        }
        AnsiConsole.Markup("Press [green]Enter[/] to continue");
        Console.ReadLine();
    }

    private void AddNewSession()
    {
        CodingSessionDto codingSession = UserInput.NewCodingSessionDialog();
        if (codingSession != null)
        {
            dbContext.AddSessionCode(codingSession);
            AnsiConsole.Markup("New Coding Session added successfully. Press [green]Enter[/] to continue");
            Console.ReadLine();
        }
        else
        {
            AnsiConsole.Markup("[bold orange1]User canceled the Add New Session Operation.[/]\n");
            AnsiConsole.Markup("Press [green]Enter[/] to continue: ");
            Console.ReadLine();
            return;
        }
    }
    
    private void ViewAllSessions()
    {
        var sessions = dbContext.GetAllCodingSessions();
        VisualisationEngine.DisplayAllSessions(sessions);
    }

}