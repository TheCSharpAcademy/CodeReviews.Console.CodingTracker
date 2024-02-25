using CodingTracker;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        bool exit = false;
        string startDate;
        string endDate;
        Record CodingRecord;
        do
        {
            // menu
            AnsiConsole.Clear();
            var title = new Rule("[red]Coding Tracker[/]").Centered(); // Conflict with System.Data had to include namespace
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

                    startDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                    
                    do {
                        AnsiConsole.Clear();
                        title = new Rule("[red]Session In Progress[/]");
                        AnsiConsole.Write(title);
                        option = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .Title("End Session?")
                            .PageSize(5)
                            .AddChoices(new[]
                            {
                            "Confirm"
                            }));
                    }while (option!="Confirm");
                    
                    endDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                    
                    CodingRecord = Input.ParseData(startDate, endDate);
                    Controller.InsertData(CodingRecord);
                    
                    break;
                case "Manually Register Session":
                    bool isValid;
                    do
                    {
                        AnsiConsole.Clear();
                        isValid = false;
                        title = new Rule("[red]Manually Register Session[/]").Centered();
                        AnsiConsole.Write(title);
                        if (!AnsiConsole.Confirm("Register new Session?"))
                        {
                            AnsiConsole.MarkupLine("Returning to main menu...");
                            Thread.Sleep(3000);
                            break;
                        }

                        else
                        {
                            startDate = AnsiConsole.Ask<string>("Please enter the start date [green]dd/MM/yyyy HH:mm[/]:");
                            endDate = AnsiConsole.Ask<string>("Please enter the end date [green]dd/MM/yyyy HH:mm[/]:");

                            CodingRecord = Input.ParseData(startDate, endDate);
                            if (CodingRecord != null)
                            {
                                Controller.InsertData(CodingRecord);
                                isValid = true;
                                continue;
                            }
                        }

                        if (!isValid && !AnsiConsole.Confirm("[red]ERROR![/]\tInput was Invalid. Try Again?"))
                        {
                            AnsiConsole.MarkupLine("Returning to main menu...");
                            Thread.Sleep(3000);
                            break;
                        }

                    } while (!isValid);
                    
                    break;

                case "View Session History":

                    // Table
                    AnsiConsole.Clear();
                    title = new Rule("[red]Session History[/]").Centered();
                    AnsiConsole.Write(title);
                    var records = Controller.ReadData();
                    var table = new Table();
                    table.Border(TableBorder.Rounded);
                    table.Title("[red]Session History[/]");
                    table.AddColumn("Start Date").Centered();
                    table.AddColumn("End Date").Centered();
                    table.AddColumn("Duration (minutes)").Centered();

                    
                    foreach (var record in records)
                    {
                        table.AddRow($"{record.DateStart}", $"{record.DateEnd}", $"{record.Duration}");
                    }
                    
                    AnsiConsole.Write(table);
                    AnsiConsole.Write("Press any key to continue.");
                    Console.ReadKey();
                    
                    break;
                case "Exit":
                    exit = true;
                    break;
            }
        } while (exit == false);

    }
}