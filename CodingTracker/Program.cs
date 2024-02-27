using CodingTracker;
using Spectre.Console;
using System.Configuration;

class Program
{
    static void Main(string[] args)
    {
        bool exit = false;

        // config
        string? connectionString = ConfigurationManager.AppSettings["connectionString"];
        string? databasePath = ConfigurationManager.AppSettings["databasePath"];

        if (connectionString == null || databasePath == null)
        {
            connectionString = "Data Source=database.db; Version = 3;";
        }
        else
        {
            connectionString = "Data Source=" + databasePath + connectionString;
        }

        Controller.CreateDatabase(connectionString);

        do
        {
            // 
            AnsiConsole.Clear();
            var title = new Rule("[red]Coding Tracker[/]").Centered();
            AnsiConsole.Write(title);
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Menu")
                .PageSize(5)
                .MoreChoicesText("[green](Move up and down to reveal more menu options)[/)")
                .AddChoices(new[] {
                "Start Session (Automatic)",
                "Manually Register Session",
                "View Session History",
                "Exit"
                }));

            switch (option)
            {
                case "Start Session (Automatic)":
                    StartSession(connectionString);
                    break;

                case "Manually Register Session":
                    RegisterManualSession(connectionString);
                    break;

                case "View Session History":
                    SessionHistory(connectionString);
                    break;

                case "Exit":
                    exit = true;
                    break;
            }
        } while (exit == false);

    }

    static void StartSession(string connectionString)
    {
        string startDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        string option;

        do
        {
            AnsiConsole.Clear();
            var title = new Rule("[red]Session In Progress[/]");
            AnsiConsole.Write(title);
            option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("End Session?")
                .PageSize(5)
                .AddChoices(new[]
                {
                            "Confirm"
                }));
        } while (option != "Confirm");

        string endDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

        Record newRecord = Input.NewRecord(startDate, endDate);
        Controller.InsertData(newRecord, connectionString);
    }

    static void RegisterManualSession(string connectionString)
    {
        bool isValid;
        string startDate;
        string endDate;
        Record newRecord;

        do
        {
            AnsiConsole.Clear();
            isValid = false;
            var title = new Rule("[red]Manually Register Session[/]").Centered();
            AnsiConsole.Write(title);

            if (!AnsiConsole.Confirm("Register new Session?"))
            {
                AnsiConsole.MarkupLine("Returning to main menu...");
                Thread.Sleep(3000);
                break;
            }

            else
            {
                startDate = AnsiConsole.Ask<string>("Please enter the start date [green]dd-MM-yyyy HH:mm:ss[/]:");
                endDate = AnsiConsole.Ask<string>("Please enter the end date [green]dd-MM-yyyy HH:mm:ss[/]:");
                if (Validation.StringToDateTime(startDate, endDate))
                {
                    newRecord = Input.NewRecord(startDate, endDate);
                    if (newRecord != null)
                    {
                        Controller.InsertData(newRecord, connectionString);
                        isValid = true;
                        continue;
                    }
                }
            }

            if (!isValid && !AnsiConsole.Confirm("[red]ERROR![/]\tInput was Invalid. Try Again?"))
            {
                AnsiConsole.MarkupLine("Returning to main menu...");
                Thread.Sleep(3000);
                break;
            }

        } while (!isValid);
    }

    static void SessionHistory(string connectionString)
    {
        AnsiConsole.Clear();
        var title = new Rule("[red]Session History[/]").Centered();
        AnsiConsole.Write(title);
        List<Record> records = Controller.ReadData(connectionString);
        var table = new Table();
        table.Border(TableBorder.Rounded);
        table.AddColumn("Start Date").Centered();
        table.AddColumn("End Date").Centered();
        table.AddColumn("Duration (minutes)").Centered();

        if (records != null)
        {
            foreach (var record in records)
            {
                table.AddRow($"{record.DateStart}", $"{record.DateEnd}", $"{record.Duration}");
            }
        }
        AnsiConsole.Write(table);
        AnsiConsole.Write("Press any key to return to menu.");
        Console.ReadKey();
    }
}