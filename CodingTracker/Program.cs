using CodingTracker;
using Spectre.Console;

class Program
{
    static void Main(string[] args)
    {
        bool exit = false;
        string startDate;
        string endDate;
        Record newRecord;
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
                    
                    newRecord = Input.NewRecord(startDate, endDate);
                    Controller.InsertData(newRecord);
                    
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

                            newRecord = Input.NewRecord(startDate, endDate);
                            if (newRecord != null)
                            {
                                Controller.InsertData(newRecord);
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

                    if (records != null)
                    {
                        foreach (var record in records)
                        {
                            table.AddRow($"{record.DateStart}", $"{record.DateEnd}", $"{record.Duration}");
                        }
                    }
                    AnsiConsole.Write(table);
                    if (AnsiConsole.Confirm("Return to main menu? "))
                    {
                        AnsiConsole.MarkupLine("Returning to main menu...");
                        Thread.Sleep(3000);
                        break;
                    }

                    break;
                case "Exit":
                    exit = true;
                    break;
            }
        } while (exit == false);

    }
}