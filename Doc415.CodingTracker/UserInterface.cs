using Doc415.CodingTracker;
using Spectre.Console;
using System.Diagnostics;
using System.Globalization;
using static Doc415.CodingTracker.Enums;
internal static class UserInterface
{
    public static void MainMenu()
    {
        var isMenuRunning = true;
        while (isMenuRunning)
        {
            AnsiConsole.Write(new FigletText("Coding Tracker").LeftJustified().Color(Color.DarkGreen));

            var selection = AnsiConsole.Prompt(new SelectionPrompt<MainMenuSelections>()
             .Title("Welcome to [green] Coding Tracker [/]\nWhat would you like to do?")
             .PageSize(10)
             .MoreChoicesText("Use arrow keys for more selection")
             .AddChoices(
                MainMenuSelections.LiveCodingSession
              , MainMenuSelections.AddRecord
              , MainMenuSelections.ViewRecords
              , MainMenuSelections.DeleteRecord
              , MainMenuSelections.UpdateRecord
              , MainMenuSelections.Statistics,
                MainMenuSelections.Quit)
             );

            switch (selection)
            {
                case MainMenuSelections.LiveCodingSession:
                    StartLiveSession();
                    Console.Clear();
                    break;

                case MainMenuSelections.AddRecord:
                    Console.WriteLine(MainMenuSelections.AddRecord);
                    AddRecord();
                    Console.Clear();
                    break;

                case MainMenuSelections.ViewRecords:
                    var dataAccess = new DataAccess();
                    var records = dataAccess.GetAllRecords();
                    ViewRecords(records);
                    Console.Clear();
                    break;

                case MainMenuSelections.DeleteRecord:
                    DeleteRecord();
                    Console.Clear();
                    break;

                case MainMenuSelections.UpdateRecord:
                    UpdateRecord();
                    Console.Clear();
                    break;

                case MainMenuSelections.Statistics:
                    Statistics();
                    Console.Clear();
                    break;

                case MainMenuSelections.Quit:
                    Console.Clear();
                    AnsiConsole.Write(new FigletText("Goodbye").LeftJustified().Color(Color.Yellow));
                    isMenuRunning = false;
                    break;
            }
        }
    }

    private static void StartLiveSession()
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        DateTime sessionStart = DateTime.Now;
        Console.Clear();
        Console.CursorVisible = false;
        bool exit = false;
        Task.Factory.StartNew(() =>
        {
            while (Console.ReadKey().Key != ConsoleKey.Enter) ;
            exit = true;
        });

        while (!exit)
        {
            TimeSpan elapsedTime = stopwatch.Elapsed;
            Console.SetCursorPosition(0, 0);
            AnsiConsole.Write(new FigletText(string.Format("{0} : {1} : {2}  ", elapsedTime.Hours, elapsedTime.Minutes, elapsedTime.Seconds % 60)).Color(Color.Chartreuse1));
            AnsiConsole.Markup("[DarkMagenta]Press Enter to end session...[/]");
        }
        stopwatch.Stop();
        Console.Clear();
        Console.CursorVisible = true;

        DateTime sessionEnd = DateTime.Now;
        CodingRecord record = new CodingRecord();
        record.DateStart = sessionStart;
        record.DateEnd = sessionEnd;
        DataAccess dataAccess = new DataAccess();
        dataAccess.InsertRecord(record);

        TimeSpan sessionTime = sessionEnd - sessionStart;
        AnsiConsole.Write($"Session ended.\nTotal session time: {sessionTime.Hours}:{sessionTime.Minutes}:{sessionTime.Seconds}\n\nSession recorded.\n\nPress Enter to return main menu...");
        Console.ReadLine();
        Console.Clear();
    }
    private static void AddRecord()
    {
        CodingRecord record = new();

        var dateInputs = GetDateInputs();
        record.DateStart = dateInputs[0];
        record.DateEnd = dateInputs[1];

        var dataAccess = new DataAccess();
        dataAccess.InsertRecord(record);
        Console.Clear();
    }

    private static void ViewRecords(IEnumerable<CodingRecord> records, bool isToBeCleared = true)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Duration");

        foreach (var record in records)
        {
            table.AddRow(record.Id.ToString(), record.DateStart.ToString(), record.DateEnd.ToString(), string.Format("{0:N0} hours {1:N0} minutes", record.Duration.TotalHours, record.Duration.TotalMinutes % 60));
        }

        AnsiConsole.Write(table);
        Console.Write("Press Enter to continue...");
        Console.ReadLine();
        if (isToBeCleared)
            Console.Clear();
    }

    private static void DeleteRecord()
    {
        var dataAccess = new DataAccess();
        var records = dataAccess.GetAllRecords();
        if (records.Count() > 0)
        {
            var notValidId = true;
            CodingRecord record;
            do
            {
                ViewRecords(records, false);

                var id = GetNumber("Please type the id of the record you want to delete.");
                try
                {
                    record = records.Where(x => x.Id == id).Single();
                    notValidId = false;
                    dataAccess.DeleteRecord(record.Id);

                }
                catch
                {
                    Console.WriteLine("There is no record with that Id, please enter a valid Id.\nPress Enter to continue...");
                    Console.ReadLine();
                }
            } while (notValidId);
        }
        else Console.WriteLine("There is no record !");
    }

    private static void UpdateRecord()
    {
        var dataAccess = new DataAccess();
        var records = dataAccess.GetAllRecords();
        ViewRecords(records, false);

        var id = GetNumber("Please type the id of the record you want to update.");

        var record = records.Where(x => x.Id == id).Single();
        var dates = GetDateInputs();

        record.DateStart = dates[0];
        record.DateEnd = dates[1];

        dataAccess.UpdateRecord(record);
    }

    private static void Statistics()
    {
        Console.WriteLine("Enter the starting and ending days");
        var dates = GetDateInputs();
        var dataAccess = new DataAccess();
        var records = dataAccess.GetRecordsBetween(dates[0], dates[1]);

        if (records.Count() > 0)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Start Date");
            table.AddColumn("End Date");
            table.AddColumn("Duration");

            TimeSpan total = TimeSpan.Zero;
            TimeSpan average = TimeSpan.Zero;
            TimeSpan best = TimeSpan.Zero;
            foreach (var record in records)
            {
                table.AddRow(record.Id.ToString(), record.DateStart.ToString(), record.DateEnd.ToString(), string.Format("{0:N0} hours {1:N0} minutes", record.Duration.TotalHours, record.Duration.TotalMinutes % 60));
                total += record.Duration;
                if (record.Duration > best)
                    best = record.Duration;
            }
            average = total / records.Count();
            string totals = string.Format("[purple] Total coding time: {0:N0}:{1:N0}:{2:N0}[/]", (total.Days / 24) + total.Hours, total.Minutes, total.Seconds);
            string bests = string.Format("[red] Your best time: {0:N0}:{1:N0}:{2:N0}[/]", best.Hours, best.Minutes, best.Seconds);
            string averages = string.Format("[blue] Average coding time: {0:N0}:{1:N0}:{2:N0}[/]", average.Hours, average.Minutes, average.Seconds);

            AnsiConsole.Write(table);
            AnsiConsole.Markup($"{totals}\n" +
                               $"{bests}\n" +
                               $"{averages}\n");
            Console.Write("Press Enter to continue...");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("There are no records between given dates\nPress Enter to continue...");
        }
    }

    private static int GetNumber(string message)
    {
        string numberInput = AnsiConsole.Ask<string>(message);

        if (numberInput == "0") { Console.Clear(); MainMenu(); }

        int output = 0;
        while (!int.TryParse(numberInput, out output) || Convert.ToInt32(numberInput) < 0)
        {
            numberInput = AnsiConsole.Ask<string>("Invalid number: " + message);
        }

        return output;
    }
    private static DateTime[] GetDateInputs()
    {
        var startDateInput = AnsiConsole.Ask<string>("Input Start Date with the format: dd-mm-yy hh:mm (24 hour clock). Or enter 0 to return to main menu.");

        if (startDateInput == "0") { Console.Clear(); MainMenu(); }

        DateTime startDate;
        while (!DateTime.TryParseExact(startDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
        {
            startDateInput = AnsiConsole.Ask<string>("\n\nInvalid date. Format: dd-mm-yy hh:mm (24 hour clock). PLease try again\n\n");
        }

        var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-mm-yy hh:mm (24 hour clock). Or enter 0 to return to main menu.");

        if (endDateInput == "0") { Console.Clear(); MainMenu(); }

        DateTime endDate;
        while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date. Format: dd-mm-yy hh:mm (24 hour clock). PLease try again\n\n");
        }

        while (startDate > endDate)
        {
            endDateInput = AnsiConsole.Ask<string>("\n\nEnd date can't be before start date. Please try again\n\n");

            while (!DateTime.TryParseExact(endDateInput, "dd-MM-yy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                endDateInput = AnsiConsole.Ask<string>("\n\nInvalid date. Format: dd-mm-yy hh:mm (24 hour clock). PLease try again\n\n");
            }
        }

        return [startDate, endDate];
    }
}
