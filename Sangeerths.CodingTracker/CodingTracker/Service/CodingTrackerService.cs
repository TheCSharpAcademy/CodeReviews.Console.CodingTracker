using CodingTracker.Database;
using CodingTracker.Models;
using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.Service
{
    public class CodingTrackerService
    {
        private readonly TrackerDB _trackerDB;
        private readonly UserValidation _userValidation = new UserValidation();

        public CodingTrackerService(TrackerDB trackerDB)
        {
            _trackerDB = trackerDB;
        }

        public void CreateTables()
        {
            _trackerDB.Creation();
        }

        public void InsertCodingSession()
        {
            try
            {
                bool isValidInput = false;
                TimeSession times = new TimeSession();
                while (!isValidInput)
                {
                     times = (TimeSession)_userValidation.ValidateUserInputTime();
                    if (times != null)
                    {
                        AnsiConsole.MarkupLine("[green]Valid input received.[/]");
                        isValidInput = true;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                        var keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            
                            return;
                        }
                        Console.Clear();
                    }

                }
                    _trackerDB.InsertCodingSession(times);
                AnsiConsole.MarkupLine("\n[green]Coding session inserted successfully![/]");
                Exit();
                

            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An error occured during the Insertion {ex.Message}[/]");
                Exit();
            }
        }

        public void GetCodingSessions()
        {
            try
            {
                var sessionList = _trackerDB.GetCodingSessions();
                var table = new Table();
                table.Title("[yellow]Coding Sessions [/]");
                table.Border(TableBorder.Rounded);
                table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
                table.AddColumn(new TableColumn("[bold]Start Date[/]").Centered());
                table.AddColumn(new TableColumn("[bold]Start Time[/]").Centered());
                table.AddColumn(new TableColumn("[bold]End Date[/]").Centered());
                table.AddColumn(new TableColumn("[bold]End Time[/]").Centered());
                table.AddColumn(new TableColumn("[bold]Duration[/]").Centered());
                foreach (var list in sessionList)
                {
                    foreach (var session in list)
                    {
                        table.AddRow(
     session.Id.ToString(),
     session.startTime.ToString("dd-MMM-yyyy"),
     session.startTime.ToString("HH:mm:ss"),
     session.endTime.ToString("dd-MMM-yyyy"),
     session.endTime.ToString("HH:mm:ss"),
     session.duration.ToString()
 );
                    }
                }
                AnsiConsole.Write(table);
                Exit();

            }
            catch (Exception ex)
            {
               AnsiConsole.MarkupLine($"[red]An error occured during the retrieval of coding sessions {ex.Message}[/]");
                Exit();
            }
        }

        public void UpdateCodingSession()
        {
            try
            {
                
                int id = _userValidation.ValidateUserInputId();
                while(id == 0)
                {
                    AnsiConsole.MarkupLine($"[blue]Press any key to continue. To go Back press ESC[/]");
                    var keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {

                        return;
                    }
                    Console.Clear();
                    id = _userValidation.ValidateUserInputId();
                }
                while (!_trackerDB.CodingSessionExists(id))
                {
                    AnsiConsole.MarkupLine($"[red]Invalid ID provided. Please enter a valid ID. To go Back press ESC[/]");
                    var keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        
                        return;
                    }
                    Console.Clear();
                    id = _userValidation.ValidateUserInputId();
                    
                }
                bool isValidInput = false;
                TimeSession times = new TimeSession();
                while (!isValidInput)
                {
                    times = _userValidation.ValidateUserInputTime();
                    if (times != null)
                    {
                        AnsiConsole.MarkupLine("[green]Valid input received.[/]");
                        isValidInput = true;
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                        var keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        { 
                            return;
                        }
                        Console.Clear();
                        AnsiConsole.MarkupLine($"[yellow]Enter valid Start and End times for the coding session with ID {id}.[/]");
                    }
                }
                  
                CodingSessionModel sessionModel = new CodingSessionModel
                {
                    Id = id.ToString(),
                    startTime = times.StartTime,
                    endTime = times.EndTime,
                    duration = times.Duration
                };
                bool success = _trackerDB.UpdateCodingSession(sessionModel);
                if (success)
                {
                   AnsiConsole.MarkupLine($"[green]\nCoding session with ID {id} updated successfully.[/]");
                }
                else
                {
                   AnsiConsole.MarkupLine($"[red]\nNo coding session found with ID {id}.[/]");
                }
                Exit();
                
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An error occured during the update of coding sessions {ex.Message}[/]");
                Exit();
            }
        }

        public void DeleteCodingSession()
        {
            try
            {
                
                int id = _userValidation.ValidateUserInputId();
                while (id == 0)
                {
                    AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                    var keyInfo = Console.ReadKey(intercept: true);
                    if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                    Console.Clear();
                    id = _userValidation.ValidateUserInputId();
                }
                bool success = _trackerDB.DeleteCodingSession(id);
                if (success)
                {
                    AnsiConsole.MarkupLine($"[green]Coding session with ID {id} deleted successfully.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]No coding session found with ID {id}.[/]");
                }
                Exit();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]An error occured during the deletion of coding sessions {ex.Message}[/]");
                Exit();
            }
        }

        public void TimeCodingSession()
        {
            AnsiConsole.MarkupLine("[yellow]Press SPACEBAR to Start/Stop.[/]");
            AnsiConsole.MarkupLine("[blue]Press ESC to Exit.[/]");

            Stopwatch stopwatch = new Stopwatch();
            bool isRunning = false;
            TimeSession session = new TimeSession();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                    if (keyInfo.Key == ConsoleKey.Spacebar)
                    {
                        if (!isRunning)
                        {
                            stopwatch.Restart();
                            session.StartTime = DateTime.Now;
                            isRunning = true;

                            AnsiConsole.MarkupLine("[green]Session Started.[/]");
                        }
                        else
                        {
                            stopwatch.Stop();
                            session.EndTime = DateTime.Now;
                            isRunning = false;

                            TimeSpan duration = session.EndTime - session.StartTime;

                            AnsiConsole.MarkupLine("[red]Session Stopped.[/]");
                            AnsiConsole.MarkupLine($"[cyan]Duration:[/] {duration:hh\\:mm\\:ss}");

                            

                            break;
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        if (isRunning)
                        {
                            stopwatch.Stop();
                            session.EndTime = DateTime.Now;

                            TimeSpan duration = session.EndTime - session.StartTime;

                            AnsiConsole.MarkupLine("[yellow]Session cancelled.[/]");
                            AnsiConsole.MarkupLine($"Duration: {duration:hh\\:mm\\:ss}");
                            return;
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[yellow]Session cancelled. Nothing was timed.[/]");
                        }

                        return;
                       
                    }
                }

                Thread.Sleep(50);
            }
            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("[yellow]Do you want to save this session?[/]")
                .AddChoices(new[] { "Yes", "No" }));
            if(choice == "Yes")
            {
                _trackerDB.InsertCodingSession(session);
                AnsiConsole.MarkupLine("[green]Coding session saved successfully![/]");
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Coding session not saved.[/]");
            }
            Exit();
        }

        public void FilterCodingSessions()
        {
            string choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Filter Coding Session").AddChoices(new[] { "1. Filter by Date", "2. Filter by Week", "3.Filter by Month","4.Filter by Year", "5.Sort Session", "6.Back" }));
            char currentChoice = choice[0];
            List<CodingSessionModel> session = new List<CodingSessionModel>();
            switch(currentChoice)
            {
                case '1':
                    DateTime? date = _userValidation.ValidateDate();
                    while (date == null)
                    {
                        AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                        var keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            return;
                        }
                        Console.Clear();
                        date = _userValidation.ValidateDate();
                    }
                    Console.Clear();
                    session = _trackerDB.FilterCodingSessionsByDate(date);
                    
                    break;
                case '2':
                    DateTime? weekDate = _userValidation.ValidateDate();
                    while(weekDate == null)
                    {
                        AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                        var keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            return;
                        }
                        Console.Clear();
                        weekDate = _userValidation.ValidateDate();
                    }
                    session = _trackerDB.FilterCodingSessionsByWeek(weekDate);
                    break;
                case '3':
                    int month = _userValidation.ValidateMonth();
                    while(month ==-1)
                    {
                        AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                        var keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            return;
                        }
                        Console.Clear();
                        month = _userValidation.ValidateMonth();
                    }
                    session = (List<CodingSessionModel>)_trackerDB.FilterCodingSessionsByMonth(month);
                    break;
                case '4':
                    int year = _userValidation.ValidateYear();
                    while (year == -1)
                    {
                        AnsiConsole.MarkupLine("[blue]Press any key. Press ESC to exit the application.[/]");
                        var keyInfo = Console.ReadKey(intercept: true);
                        if (keyInfo.Key == ConsoleKey.Escape)
                        {
                            return;
                        }
                        Console.Clear();
                        year = _userValidation.ValidateYear();
                    }  
                    session = (List<CodingSessionModel>)_trackerDB.FilterCodingSessionsByYear(year);
                    break;
                case '5':
                    string sortChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Sort Coding Session").AddChoices(new[] { "Ascending", "Descending" }));
                    session = _trackerDB.SortCodingSession(sortChoice);
                    break;
                case '6':
                    return;
                default:
                    AnsiConsole.WriteLine("[red]Invalid choice. Please try again.[/]");
                    break;

            }
            if (session.Count >0)
            {
                var table = new Table();
                table.Title("[green]Filtered Coding Sessions[/]");
                table.Border(TableBorder.Rounded);
                table.AddColumn(new TableColumn("[bold]ID[/]").Centered());
                table.AddColumn(new TableColumn("[bold]Start Date[/]").Centered());
                table.AddColumn(new TableColumn("[bold]Start Time[/]").Centered());
                table.AddColumn(new TableColumn("[bold]End Date[/]").Centered());
                table.AddColumn(new TableColumn("[bold]End Time[/]").Centered());
                table.AddColumn(new TableColumn("[bold]Duration[/]").Centered());

                foreach (var sessionModel in session)
                {
                    table.AddRow(
     sessionModel.Id.ToString(),
     sessionModel.startTime.ToString("dd-MMM-yyyy"),
     sessionModel.startTime.ToString("HH:mm:ss"),
     sessionModel.endTime.ToString("dd-MMM-yyyy"),
     sessionModel.endTime.ToString("HH:mm:ss"),
     sessionModel.duration.ToString()
 );
                }
                AnsiConsole.Write(table);

            }
            else
            {
                AnsiConsole.MarkupLine("[red]No coding sessions found for the specified filter.[/]");
            }
            Exit();
        }
        public  void ShowHeader()
        {
            AnsiConsole.Write(new Rule().RuleStyle("grey"));
            AnsiConsole.Write(new FigletText("CodingTracker").Centered().Color(Color.Green));
            AnsiConsole.Write(new Rule().RuleStyle("grey"));
        }
        public void Exit()
        {
            AnsiConsole.WriteLine("\nEnter any Key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
