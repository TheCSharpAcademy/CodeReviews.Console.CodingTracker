using System.Configuration;
using Spectre.Console;

namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {
        SessionController sessionController;
        string? connectionString = ConfigurationManager.AppSettings.Get("connectionString");
        if (connectionString != null)
        {
            Database.Initialize(connectionString);
            sessionController = new(connectionString);
        }
        else
        {
            AnsiConsole.Markup("\n[red]Unable to initialize.[/] Shutting down...\n\n");
            return;
        }

        CodingSession currentSession = new();

        bool keepRunning = true;
        while (keepRunning)
        {
            string action = UserInput.PromptUserAction();
            switch (action)
            {
                case ActionType.StartSession:
                    if (currentSession.StartTime != DateTime.MinValue)
                    {
                        AnsiConsole.Markup("[green]Session already started.[/][blue] Enjoy[/]");
                        break;
                    }

                    currentSession.StartTime = UserInput.PromptDateTime("start");
                    AnsiConsole.Markup($"\n[green]Session started at {currentSession.StartTime}.[/][blue] Enjoy[/]");
                    break;
                case ActionType.EndSession:
                    if (!Validate.CanEndSession(currentSession))
                    {
                        AnsiConsole.Markup("\n[red]No session to end.[/] Press enter to return to menu...");
                        break;
                    }

                    DateTime endTime = UserInput.PromptDateTime("end");
                    if (Validate.EndTimeIsAfterStartTime(currentSession, endTime))
                    {
                        currentSession.EndTime = endTime;
                    }
                    else
                    {
                        AnsiConsole.Markup("\n[red]Invalid end time.[/] Press enter to return to menu...");
                        break;
                    }

                    if (sessionController.CreateSession(currentSession) > 0)
                    {
                        currentSession = new();
                        AnsiConsole.Markup($"\n[green]Session ended at {endTime}.[/][blue] Well done![/]");
                    }
                    else
                    {
                        AnsiConsole.Markup("\n[red]Failed to end session.[/] Press enter to return to menu...\n\n");
                    }
                    break;
                case ActionType.EditSession:
                    Report.ShowSessions(sessionController.GetAllSessions());
                    int id = UserInput.PromptSessionId();
                    if (id < 0)
                    {
                        AnsiConsole.Markup("\n[red]Failed to read id[/]. Press enter to return to menu...");
                        break;
                    }
                    CodingSession sessionToEdit = new()
                    {
                        Id = id,
                        StartTime = UserInput.PromptDateTime("start")
                    };

                    DateTime newEndTime = UserInput.PromptDateTime("end");
                    if (Validate.EndTimeIsAfterStartTime(sessionToEdit, newEndTime))
                    {
                        sessionToEdit.EndTime = newEndTime;
                    }
                    else
                    {
                        AnsiConsole.Markup("\n[red]Invalid end time.[/] Press enter to return to menu...\n");
                        break;
                    }

                    if (sessionController.UpdateSession(sessionToEdit))
                    {
                        AnsiConsole.Markup($"\n[green]Session updated.[/] Press enter to return to menu...\n");
                    }
                    else
                    {
                        AnsiConsole.Markup("\n[red]Failed to update.[/] Press enter to return to menu...\n");
                    }
                    break;
                case ActionType.DeleteSession:
                    Report.ShowSessions(sessionController.GetAllSessions());

                    int idToDelete = UserInput.PromptSessionId();
                    if (idToDelete < 0)
                    {
                        AnsiConsole.Markup("\n[red]Failed to read id[/]. Press enter to return to menu...");
                        break;
                    }

                    if (sessionController.DeleteSession(idToDelete))
                    {
                        AnsiConsole.Markup($"\n[green]Session deleted.[/] Press enter to return to menu...");
                    }
                    else
                    {
                        AnsiConsole.Markup("\n[red]Failed to delete session.[/] Press enter to return to menu...");
                    }

                    break;
                case ActionType.ShowSessions:
                    Report.ShowSessions(sessionController.GetAllSessions().TakeLast(5));
                    Report.ShowAnalytics(sessionController);
                    AnsiConsole.Markup("\nPress enter to return to menu...");
                    break;
                case ActionType.ExitProgram:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }

            Console.ReadLine();
        }
    }
}