using Spectre.Console;
using System.Globalization;

namespace CodingTracker.Kakurokan
{
    internal class Program
    {
        public static void Main()
        {
            try
            {
                DataAcces SessionsDB = new();
                AnsiConsole.Clear();

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(@"  Welcome to your [blue]Coding Tracker![/] 
  What would you like to do?").MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                                                                                            .AddChoices(new[] { "Insert new Code Session", "View all Code Sessions", "Delete a session", "See a Goal", "Exit Coding Tracker" }));
                AnsiConsole.Clear();
                switch (select_menu)
                {
                    case "Insert new Code Session":
                        select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(@"How do you want insert your [blue]Coding Session[/]?").AddChoices(new[] { "Insert Code Session manually", "Insert with stopwatch" }));
                        switch (select_menu)
                        {
                            case ("Insert Code Session manually"):
                                SessionsDB.Insert(CreateNewSession());
                                break;
                            case ("Insert with stopwatch"):
                                SessionsDB.Insert(CreateNewCronometedSession());
                                break;
                        }
                        break;
                    case "View all Code Sessions":
                        SessionsDB.ViewAll();
                        break;
                    case "Delete a session":
                        SessionsDB.Delete();
                        break;
                    case "See a Goal":
                        SessionsDB.Goals();
                        break;
                    case "Exit Coding Tracker":
                        Environment.Exit(0);
                        break;
                };
            }
            catch (Exception ex)
            {
                AnsiConsole.Markup("Sorry a [red]error[/] ocurred. Here is the information: " + ex.Message);
                Thread.Sleep(4000);
                Main();
            }
        }
        public static CodingSession CreateNewCronometedSession()
        {
            DateTime start = new();
            AnsiConsole.Status().Start(@"Running stopwatch...", ctx =>
    {
        AnsiConsole.MarkupLine("[green]Starting in 1...[/]");
        Thread.Sleep(1000);

        AnsiConsole.MarkupLine("[green]2...[/]");
        Thread.Sleep(1000);

        AnsiConsole.MarkupLine("[green]3...[/]");
        Thread.Sleep(1000);
        AnsiConsole.MarkupLine("[green]Started![/]");
        AnsiConsole.WriteLine();
        start = DateTime.Now;

        var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().Title(@"Do you want to Finish your [blue]Coding Session[/]?").AddChoices(@"Stop and Insert Code Session
"));
    });

            CodingSession session = new CodingSession(DateTime.Now, start, DateTime.Now);
            AnsiConsole.Markup("Your Coding Session was [green]successfully created[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine(session.ToStringWithoutId());
            DisplayReturningTomenu();
            return session;
        }
        public static CodingSession CreateNewSession()
        {
            AnsiConsole.Clear();
            string date = AnsiConsole.Ask<string>("What`s the starting date?[red](Format: dd-MM-yyyy)[/]");
            DateTime clean_date;
            while (!DateTime.TryParseExact(date, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out clean_date))
            {
                date = AnsiConsole.Ask<string>("[red][[Invalid date]][/] Please, try again with the format (dd-MM-yyyy): ");
            }

            string start_time = AnsiConsole.Ask<string>("Which time do you started?[red](HH:mm, 24hours format)[/] ");
            TimeOnly clean_start_time;
            while (!TimeOnly.TryParseExact(start_time, "HH:mm", out clean_start_time))
            {
                start_time = AnsiConsole.Ask<string>("[red][[Invalid time]][/] Please, try again with the format (HH:MM): ");
            }

            string end_time = AnsiConsole.Ask<string>("Which time do you finished?[red](HH:mm, 24hours format)[/] ");
            TimeOnly clean_end_time;
            while (!TimeOnly.TryParseExact(end_time, "HH:mm", out clean_end_time))
            {
                end_time = AnsiConsole.Ask<string>("[red][[Invalid time]][/] Please, try again with the format (HH:MM): ");
            }

            DateTime final_end_time = clean_date.Add(clean_end_time.ToTimeSpan());
            DateTime final_start_time = clean_date.Add(clean_start_time.ToTimeSpan());

            if ((int)(final_end_time - final_start_time).TotalMinutes < 0)
            {
                AnsiConsole.MarkupLine(@"[red][[ERROR]][/] Invalid EndTime
Returning to menu...");
                Thread.Sleep(2700);
                Main();
            }

            CodingSession session = new(clean_date, final_start_time, final_end_time);

            AnsiConsole.Markup("Your Coding Session was [green]successfully created[/]");
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine(session.ToStringWithoutId());
            DisplayReturningTomenu();
            return session;
        }


        public static void DisplayReturningTomenu() => AnsiConsole.Status()
        .Start("Returning to menu...", ctx =>
        {
            Thread.Sleep(3000);
        });
    }
}