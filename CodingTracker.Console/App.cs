using System.Diagnostics;
using DB;
using Models.Dtos;
using Models.Entities;

namespace CodingTracker;

public class App(CodingTimeDBContext dbContext)
{
    private readonly CodingTimeDBContext db = dbContext;
    private readonly Stopwatch watch = new();
    public void Run()
    {
        db.SeedDatabase();

        bool continueRunning = true;
        while (continueRunning)
        {
            string quickSessionOption = "Quickstart a session";
            if (watch.ElapsedTicks > 0)
            {
                quickSessionOption = "Stop current session";
            }

            string[] options = ["Exit", "Log a session", "Update a session", "View sessions", "Delete session", quickSessionOption, "Time in current session"];
            var choice = UI.MenuSelection("[green]Coding[/] [red]Tracker[/] [blue]Menu[/]. Select an option below:", options);

            switch (choice)
            {
                case 0:
                    if (CheckForOpenSession())
                    {
                        UI.ConfirmationMessage("You have an active coding session open. End the session first!");
                    }
                    else
                    {
                        continueRunning = false;
                    }
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
                case 5:
                    QuickLog();
                    break;
                case 6:
                    TimeInCurrentSession();
                    break;
            }

            if (!continueRunning)
            {
                break;
            }
        }
    }

    private bool CheckForOpenSession()
    {
        var openSession = db.GetOpenCodingSession();
        return openSession != null;
    }

    private void TimeInCurrentSession()
    {
        if (watch.ElapsedTicks > 0)
        {
            string elapsedMinutesString = (watch.ElapsedMilliseconds / 1000 / 60).ToString();
            UI.ConfirmationMessage("Current session has been [green]" + elapsedMinutesString + "[/] minutes long.");
        }
        else
        {
            UI.ConfirmationMessage("No session is being tracked right now.");
        }
    }

    private void QuickLog()
    {
        if (watch.ElapsedTicks > 0)
        {
            var choice = UI.MenuSelection("Stop current session?", ["yes", "no"]);

            switch (choice)
            {
                case 0:
                    var codingTime = db.GetOpenCodingSession();

                    var now = DateTime.Now;
                    var endTime = now.ToString("HH:mm dd-MM-yy");
                    string elapsedMinutesString = (watch.ElapsedMilliseconds / 1000 / 60).ToString();

                    var endCodingTime = new CodingTime(codingTime!.Id, codingTime.Task, codingTime.StartTime, endTime);

                    db.UpdateCodingTime(endCodingTime);

                    UI.ConfirmationMessage("Ended " + elapsedMinutesString + " minute(s) session.");
                    watch.Reset();
                    break;
                case 1:
                    UI.ConfirmationMessage("Happy coding :).");
                    break;
            }
        }
        else
        {
            var choice = UI.MenuSelection("Start a coding session?", ["yes", "no"]);

            switch (choice)
            {
                case 0:
                    string task = UI.StringResponse("Enter the [blue]task[/] name");
                    var now = DateTime.Now;
                    var startTime = now.ToString("HH:mm dd-MM-yy");
                    var endTime = "";

                    var newCodingTime = new CreateCodingTimeDto(task, startTime, endTime);

                    db.CreateCodingTime(newCodingTime);
                    watch.Start();
                    UI.ConfirmationMessage("Happy coding :).");
                    break;
                case 1:
                    UI.ConfirmationMessage("Coding session has [red]not[/] been started.");
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
        var updatedStartTime = UI.TimeResponseWithDefault("Enter the [blue]start time[/]", codingTime.StartTime);
        var updatedEndTime = UI.TimeResponseWithDefault("Enter the [blue]end time[/]", codingTime.EndTime);

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
        string task = UI.StringResponse("Enter the [blue]task[/] name");
        string startTime = UI.TimeResponse("Enter the [blue]start time[/]");
        string endTime = UI.TimeResponse("Enter the [blue]end time[/]");

        var codingTime = new CreateCodingTimeDto(task, startTime, endTime);
        db.CreateCodingTime(codingTime);

        UI.ConfirmationMessage("[green]Coding session saved[/].");
    }
}
