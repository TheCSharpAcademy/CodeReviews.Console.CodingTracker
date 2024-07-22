using DB;
using Models.Dtos;
using Models.Entities;

namespace CodingTracker;

public class App(CodingTimeDBContext dbContext)
{
    private readonly CodingTimeDBContext db = dbContext;

    public void Run()
    {
        db.SeedDatabase();

        bool continueRunning = true;
        while (continueRunning)
        {
            string[] options = ["Exit", "Log a session", "Update a session", "View sessions", "Delete session"];
            var choice = UI.MenuSelection("[green]Coding[/] [red]Tracker[/] [blue]Menu[/]. Select an option below:", options);

            switch (choice)
            {
                case 0:
                    continueRunning = false;
                    break;
                case 1:
                    LogSession();
                    break;
                case 2:
                    UpdateSession();
                    break;
                case 3:
                    DisplaySessions();
                    break;
                case 4:
                    DeleteSession();
                    break;
            }

            if (!continueRunning)
            {
                break;
            }
        }
    }

    private void DeleteSession()
    {
        var sessions = GetAllCodingSessions();
        UI.DisplayCodingSessions(sessions);

        CodingTime? codingTime = null;
        long id = -1;
        while (codingTime == null)
        {
            id = UI.IntResponse("Enter the [blue]id[/] of the session you want to delete");
            codingTime = db.GetCodingTimeById(id);
            if (codingTime == null)
            {
                UI.InvalidationMessage("No coding session with that id was found");
            }
            else
            {
                break;
            }
        }

        db.DeleteCodingTime(id);
        UI.ConfirmationMessage("[green]Deleted coding session[/].");
    }

    private void UpdateSession()
    {
        var sessions = GetAllCodingSessions();
        UI.DisplayCodingSessions(sessions);

        CodingTime? codingTime = null;
        long id = -1;
        while (codingTime == null)
        {
            id = UI.IntResponse("Enter the [blue]id[/] of the session you want to edit");
            codingTime = db.GetCodingTimeById(id);
            if (codingTime == null)
            {
                UI.InvalidationMessage("No coding session with that id was found");
            }
            else
            {
                break;
            }
        }

        var updatedName = UI.StringResponseWithDefault("Enter the [blue]task name[/]", codingTime.Task);
        var updatedStartTime = UI.StringResponseWithDefault("Enter the [blue]start time[/]", codingTime.StartTime);
        var updatedEndTime = UI.StringResponseWithDefault("Enter the [blue]end time[/]", codingTime.EndTime);

        var updatedCodingTime = new CodingTime(id, updatedName, updatedStartTime, updatedEndTime);

        db.UpdateCodingTime(updatedCodingTime);

        UI.ConfirmationMessage("[green]Updated coding session[/].");
    }

    private void DisplaySessions()
    {
        List<CodingSession> sessions = GetAllCodingSessions();

        UI.DisplayCodingSessions(sessions);

        UI.ConfirmationMessage("");
    }

    private List<CodingSession> GetAllCodingSessions()
    {
        var codingTimes = db.GetAllCodingTimes();

        List<CodingSession> sessions = [];

        foreach (var codingTime in codingTimes)
        {
            var duration = Helpers.CalculateSecondsBetweenDates(codingTime.StartTime, codingTime.EndTime);
            var session = new CodingSession(codingTime.Id, codingTime.Task, codingTime.StartTime, codingTime.EndTime, duration);
            sessions.Add(session);
        }

        return sessions;
    }

    private void LogSession()
    {
        // get task
        string task = UI.StringResponse("Enter the [blue]task[/] name");

        // get start time
        string startTime = UI.TimeResponse("Enter the [blue]start time[/]");

        // get end time 
        string endTime = UI.TimeResponse("Enter the [blue]end time[/]");

        // save into database
        var codingTime = new CreateCodingTimeDto(task, startTime, endTime);
        db.CreateCodingTime(codingTime);

        UI.ConfirmationMessage("[green]Coding session saved[/].");
    }
}
