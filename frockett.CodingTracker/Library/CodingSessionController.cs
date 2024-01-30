using Spectre.Console;
using frockett.CodingTracker.Library;
using System.Diagnostics;

namespace Library;

public class CodingSessionController
{
    private readonly IDbMethods dbMethods;
    private readonly UserInputValidationService inputValidationService;
    private readonly StopwatchService stopwatchService;

    public CodingSessionController(IDbMethods dbMethods, UserInputValidationService ui, StopwatchService stopwatch)
    {
        this.dbMethods = dbMethods;
        inputValidationService = ui;
        stopwatchService = stopwatch;
    }

    public void InsertCodingSession()
    {
        dbMethods.InsertCodingSession(inputValidationService.GetStartEndTimeInput());
    }

    public void UpdateCodingSession()
    {
        int sessionIdToUpdate = inputValidationService.GetSessionId("Which session would you like to update? Enter 0 to return to main menu");

        if (sessionIdToUpdate == 0)
        {
            return;
        }
        
        if(!dbMethods.ValidateSessionById(sessionIdToUpdate))
        {
            AnsiConsole.WriteLine($"[red]Record {sessionIdToUpdate} does not exist");
            sessionIdToUpdate = inputValidationService.GetSessionId("Please enter a valid session ID");
        }

        CodingSession sessionToUpdate = inputValidationService.GetStartEndTimeInput();
        sessionToUpdate.Id = sessionIdToUpdate;
        
        dbMethods.UpdateCodingSession(sessionToUpdate);
    }

    public void DeleteCodingSession()
    {
        int sessionIdToDelete = inputValidationService.GetSessionId("Which session would you like to delete? Enter 0 to return to main menu.");

        if (sessionIdToDelete == 0)
        {
            return;
        }

        if (!dbMethods.ValidateSessionById(sessionIdToDelete))
        {
            AnsiConsole.WriteLine($"[red]Record {sessionIdToDelete} does not exist");
            sessionIdToDelete = inputValidationService.GetSessionId("Please enter a valid session ID");
        }

        dbMethods.DeleteCodingSession(sessionIdToDelete);
    }

    public Stopwatch StartCodingSession()
    {
        return stopwatchService.StartStopwatch();
    }

    public void StopCodingSession(Stopwatch stopwatch)
    {
        CodingSession timedSession = new CodingSession();
        timedSession.Duration = stopwatch.Elapsed;
        timedSession.EndTime = DateTime.Now;
        timedSession.StartTime = DateTime.Now - stopwatch.Elapsed;
        dbMethods.InsertCodingSession(timedSession);
        stopwatchService.StopStopwatch();
        stopwatchService.ResetStopwatch();
    }

    public List<CodingSession> FetchAllSessionHistory()
    {
        if (!dbMethods.CheckForTableData())
        {
            AnsiConsole.MarkupLine("[red]There are no sessions available to display! Get coding![/]\n");
            AnsiConsole.MarkupLine("Press [green]any[/] key to return to the main menu...");
            Console.ReadKey(true);
        }

        List<CodingSession> sessions = dbMethods.GetAllCodingSessions();
        return sessions;
    }

    public List<CodingSession> FetchCustomSessionHistory(bool isListOfAverages)
    {
        DateOnly year;
        DateOnly monthAndYear;

        if (isListOfAverages)
        {
            year = inputValidationService.GetYearOnly("Please enter the year you wish to display: ");

            if (!dbMethods.CheckForTableData(year.Year))
            {
                AnsiConsole.MarkupLine("[red]There are no sessions available to display! Get coding![/]\n");
                AnsiConsole.MarkupLine("Press [green]any[/] key to return to the main menu...");
                Console.ReadKey(true);
            }
            return dbMethods.GetCustomCodingSessions(isListOfAverages, year);
        }
        else
        {
            monthAndYear = inputValidationService.GetMonthAndYear("Please enter the month and year to display in the format 'mm-yyyy'\n");

            if (!dbMethods.CheckForTableData(monthAndYear.Year, monthAndYear.Month))
            {
                AnsiConsole.MarkupLine("[red]There are no sessions available to display! Get coding![/]\n");
                AnsiConsole.MarkupLine("Press [green]any[/] key to return to the main menu...");
                Console.ReadKey(true);
            }
            return dbMethods.GetCustomCodingSessions(isListOfAverages, monthAndYear);
        }
    }

    public void SeedRandomSessions(int iterations)
    {
        dbMethods.SeedRandomData(iterations);

        AnsiConsole.WriteLine("Seeding successful. Don't overdo it, now! Press enter to return to main menu.");
        Console.ReadLine();
    }
}
