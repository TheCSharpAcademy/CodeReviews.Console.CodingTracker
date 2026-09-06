using CodingTracker.Controllers;
using CodingTracker.Model;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Threading;
using CodingTracker;

namespace CodingTracker.View
{
    internal class UserInterface
    {
        private readonly CodingController _controller;
        public UserInterface(CodingController controller)
        {
            _controller = controller;
        }


        public void Menu()
        {
            bool runMenu = true;
            AnsiConsole.MarkupLine("[#FFC0CB]Welcome to CodingTracker![/]");
            do
            {
                var codingMenu = UserInput.AnySelection("What do you want to do?", ["Start Coding Session", "Add Coding Session", "Coding History", "End Application"]);

                switch (codingMenu)
                {
                    case "Start Coding Session":
                        {
                            CodingSession? session = StartCodingSession();
                            if (session != null)
                            {
                                SaveSession(session);
                            }
                            break;
                        }
                    case "Add Coding Session":
                        {
                            CodingSession? session = ManualEnterCodingSession();
                            if (session != null)
                            {
                                SaveSession(session);
                            }
                            break;
                        }
                    case "Coding History":
                        {
                            List<CodingSession> historyList = GetHistory();
                            if (historyList.Count == 0)
                            {
                                AnsiConsole.MarkupLine("\n[yellow]No sessions found yet.[/]");
                                UserInput.WaitForUser();
                                Console.Clear();
                                break;
                            }

                            DisplayHistory(historyList);
                            List<int> validIDs = GetValidIDs(historyList);
                            CRUDoperation(validIDs);
                            break;
                        }
                    case "End Application":
                        {
                            AnsiConsole.MarkupLine("[blue]Good Job! See you soon![/]");
                            runMenu = false;
                            break;
                        }
                    default:
                        {
                            AnsiConsole.MarkupLine("[bold red]An unexpected Error has occurred.[/]");
                            break;
                        }
                }
            } while (runMenu);
        }
        private void SaveSession(CodingSession session)
        {
            bool inserted = _controller.InsertSql(session);
            if (inserted)
            {
                AnsiConsole.MarkupLine("[green]Session successfully saved to database![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[bold red]Failed to save session to database.[/]");
            }
        }
        private List<int> GetValidIDs(List<CodingSession> list)
        {
            List<int> idList = new List<int>();
            foreach (CodingSession session in list)
            {
                if(session.Id.HasValue) idList.Add(session.Id.Value);
            }
            return idList;
        }

        private void CRUDoperation(List<int> validIDs)
        {
            bool runCRUD = true;
            while (runCRUD)
            {
                if (validIDs.Count == 0) // check if any records left
                {
                    AnsiConsole.MarkupLine("[yellow]No records remaining. Returning to menu...[/]");
                    break;
                }

                string doContinue = UserInput.AnySelection("Do you want to create, update or delete DATA ?", ["Create","Update","Delete","Back to Menu"]);
                switch (doContinue)
                {
                    case "Create":
                        CodingSession? session = ManualEnterCodingSession();
                        if (session == null)
                        {
                            break;
                        }

                        if (_controller.InsertSql(session))
                        {
                            AnsiConsole.MarkupLine("[green]Added Session![/]");
                            List<CodingSession> historyList = GetHistory();
                            DisplayHistory(historyList);
                            validIDs = GetValidIDs(historyList);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[bold red]Failed to add session.[/]");
                        }
                        break;
                    case "Update":
                        {
                            AnsiConsole.MarkupLine("Enter [red]Id[/] of Session you want to update.");
                            int inputID = UserInput.GetIntFromUser(validIDs);

                            (DateTime startDateTime, DateTime endDateTime) = PromptStartAndEndTime();

                            // NEW SESSION
                            CodingSession newSession = new CodingSession(startDateTime, endDateTime, inputID);
                            bool updated = _controller.UpdateSessionHistory(newSession);
                            if (updated)
                            {
                                AnsiConsole.MarkupLine("[green]Updated successfully![/]");
                                List<CodingSession> historyList = GetHistory();
                                DisplayHistory(historyList);
                                validIDs = GetValidIDs(historyList);
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[bold red]Did not Update. Unexpected Error.[/]");
                            }
                            break;
                        }

                    case "Delete":
                        {
                            AnsiConsole.MarkupLine("Enter [red]Id[/] of Session you want to delete.");
                            int inputID = UserInput.GetIntFromUser(validIDs);
                            bool deleted = _controller.DeleteSession(inputID);
                            if (deleted)
                            {
                                AnsiConsole.MarkupLine("[green]Deleted successfully![/]");
                                List<CodingSession> historyList = GetHistory();
                                DisplayHistory(historyList);
                                validIDs.Remove(inputID);
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("[bold red]Did not Delete. Unexpected Error.[/]");
                            }
                            break;
                        }
                    case "Back to Menu":
                        Console.Clear();
                        runCRUD = false;
                        break;
                }
            }
        }

        private void DisplayHistory(List<CodingSession> historyList)
        {
            var table = new Table();

            // Add columns
            table.AddColumn("Id");
            table.AddColumn("Start Time");
            table.AddColumn("End Time");
            table.AddColumn("Duration");

            // Add rows
            foreach (CodingSession rawSession in historyList)
            {
                CodingSession completedSession = new CodingSession(rawSession.StartTimeString, rawSession.EndTimeString, rawSession.Id);
                table.AddRow($"{completedSession.Id.ToString()}", $"{completedSession.StartTimeString}", $"{completedSession.EndTimeString}", $"{completedSession.DurationString}");
            }
            AnsiConsole.Write(table);
        }

        private List<CodingSession> GetHistory()
        {
            List<CodingSession> list = _controller.GetSessionHistory();
            return list;
        }

        private CodingSession? ManualEnterCodingSession()
        {
            do
            {
                (DateTime startDateTime, DateTime endDateTime) =PromptStartAndEndTime();
                // TIMESPAN
                TimeSpan manualTimeSpan = TimeCalculator.GetTimeSinceStart(startDateTime, endDateTime);

                var doContinue = UserInput.AnySelection($"Is {manualTimeSpan.ToString(@"hh\:mm\:ss")} the correct timespan?", ["Correct!", "Try again!", "Abort, back to menu."]);
                
                switch (doContinue)
                {
                    case "Correct!":
                        return new CodingSession(startDateTime, endDateTime);

                    case "Try again!":
                        break;

                    case "Abort, back to menu.":
                        return null;
                }
            } while (true);
            
        }

        private DateTime GetDateTimeFromUser()
        {
            do
            {
                AnsiConsole.Markup("Enter [bold red]valid[/] Date and Time(dd-MM-yyyy  HH:mm:ss): ");
                string dateTime = UserInput.GetString();
                bool checkDateTimeValid = Validation.ValidateStringToDateTime(dateTime, out DateTime parsedDateTime);
                if (checkDateTimeValid)
                {
                    AnsiConsole.MarkupLine($"[green]Valid:[/] {parsedDateTime}");
                    return parsedDateTime;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid Format![/]");
                }

            } while (true);
            
        }

        private CodingSession? StartCodingSession()
        {
            DateTime startDateTime = DateTime.Now;
            AnsiConsole.MarkupLine($"[#FFA500]Current Date(dd-MM-yyyy): {startDateTime.ToString("dd-MM-yyyy")}[/]");
            AnsiConsole.MarkupLine($"[blue]Current Time(HH:mm:ss): {startDateTime.ToString("HH:mm:ss")}[/]");

            var doContinue = UserInput.AnySelection("Do you want to start the timer?", ["Start Coding", "Go back to Menu"]);
            
            if (doContinue != "Start Coding")
            {
                return null;
            }

            DateTime currentDateTime = DateTime.Now;
            startDateTime = DateTime.Now;

            var table = new Table().AddColumn("DateTime").Width(80).AddColumn("Trait");
            table.AddRow($"{startDateTime.ToString("dd-MM-yyyy     HH:mm:ss")}", "Start");
            table.AddRow($"", "");
            table.AddRow($"", "");

            AnsiConsole.MarkupLine($"[#FFA500]Started at:[/] {startDateTime:dd-MM-yyyy HH:mm:ss}");
            AnsiConsole.MarkupLine("[grey]Press any key to end session...[/]\n");

            UserInput.ClearInputBuffer();
            AnsiConsole.Live(table)
                .Start(ctx =>
                {
                    while (!Console.KeyAvailable)
                    {
                        currentDateTime = DateTime.Now;
                        table.RemoveRow(1);
                        table.InsertRow(1, $"{currentDateTime.ToString("dd-MM-yyyy     HH:mm:ss")}", "Current");
                        table.RemoveRow(2);
                        TimeSpan timePassed = TimeCalculator.GetTimeSinceStart(startDateTime, currentDateTime);
                        table.InsertRow(2, $"[#FFA500]{timePassed.ToString(@"hh\:mm\:ss")}[/]", "[#FFA500]Time passed since start[/]");
                        ctx.Refresh();
                        Thread.Sleep(200);
                    }
                });

            UserInput.StopSessionOnKeyPress();
            AnsiConsole.MarkupLine("Congrats! You have finished your Coding Session.");
            return new CodingSession(startDateTime, currentDateTime);
        }
        private (DateTime start, DateTime end) PromptStartAndEndTime()
        {
            AnsiConsole.MarkupLine("Enter start time.");
            DateTime start = GetDateTimeFromUser();

            DateTime end;
            bool invalid;
            do
            {
                AnsiConsole.MarkupLine("Enter end time.");
                end = GetDateTimeFromUser();
                invalid = !Validation.IsEndTimeValid(start, end);

                if (invalid)
                {
                    AnsiConsole.MarkupLine("[bold red]End time must be after start time.[/]");
                }
            } while (invalid);

            return (start, end);
        }
    }
}
