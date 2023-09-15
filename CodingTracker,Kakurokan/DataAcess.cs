using Dapper;
using Spectre.Console;
using System.Data.SQLite;

namespace CodingTracker.Kakurokan
{
    internal class DataAcces
    {
        public SQLiteConnection MyConnection { get; private set; }
        public DataAcces()
        {
            MyConnection = new("Data Source=CodingSessions.db;Version=3;");

            if (!File.Exists("./CodingSessions.db"))
            {
                SQLiteConnection.CreateFile("CodingSessions.db");
                AnsiConsole.Markup("[green]Database created[/]");
            }
            using (var conn = new SQLiteConnection(MyConnection))
            {
                conn.Open();
                var command = conn.CreateCommand();

                command.CommandText =
                    @"CREATE TABLE IF NOT EXISTS Sessions (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT NOT NULL,
    Duration TEXT NOT NULL,	
    StartTime TEXT NOT NULL,
	EndTime TEXT NOT NULL)";
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void Insert(CodingSession session)
        {
            using (var conn = new SQLiteConnection(MyConnection))
            {
                int id = conn.QuerySingle<int>("INSERT INTO Sessions (Date, Duration, StartTime, EndTime) VALUES (@Date, @Duration, @StartTime, @EndTime) returning Id;", new { Date = session.Date, Duration = session.Duration, StartTime = session.StartTime, EndTime = session.EndTime });
                session.Id = id;
            }
            Program.Main();
        }
        public void Delete()
        {
            List<CodingSession> todelete = new();
            using (var conn = new SQLiteConnection(MyConnection))
            {
                var output = conn.Query<CodingSession>("SELECT * FROM Sessions", new DynamicParameters());
                output = output.ToList();

                var deletes = new MultiSelectionPrompt<string>().Title("What Coding Session you want to [red]delete[/]?").NotRequired()
                .MoreChoicesText("[grey](Move up and down to reveal more Sessions)[/]").InstructionsText(
            "[grey](Press [blue]<space>[/] to toggle a Session, " +
            "[green]<enter>[/] to accept)[/]");

                foreach (CodingSession item in output)
                {
                    deletes.AddChoice(item.ToStringWithoutStartAndEnd());
                }

                List<string> deleted = AnsiConsole.Prompt(deletes);

                AnsiConsole.MarkupLine("You want to [red]delete:[/]");
                foreach (var item in deleted)
                {
                    AnsiConsole.WriteLine();
                    AnsiConsole.WriteLine(item);
                }

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "[red]Delete all[/]", "Return to menu" }));
                if (select_menu == "Return to menu") Program.Main();

                foreach (CodingSession item in output)
                {
                    foreach (string item2 in deleted)
                    {
                        if (item.ToStringWithoutStartAndEnd() == item2)
                        {
                            todelete.Add(item);
                        }
                    }
                }

                foreach (CodingSession item in todelete)
                {
                    conn.Execute("DELETE FROM Sessions WHERE Id = @Id;", new { Id = item.Id });
                }

                AnsiConsole.Markup(@"
You habit was [green]successfully deleted[/]
");
                Program.DisplayReturningTomenu();
                Program.Main();
            }


        }
        public void ViewAll()
        {
            using (var conn = new SQLiteConnection(MyConnection))
            {
                var output = conn.Query<CodingSession>("SELECT * FROM Sessions", new DynamicParameters());
                output = output.ToList();

                var table = new Table();
                table.AddColumns(new[] { "Id", "Date", "Duration", "StartTime", "EndTime" });
                foreach (var item in output)
                {
                    table.AddRow(new[] { item.Id.ToString(), item.Date.ToString("dd-MM-yyyy"), item.Duration.ToString("hh\\:mm\\:ss"), item.StartTime.ToString("HH:mm"), item.EndTime.ToString("HH:mm") }).Centered();
                }
                table.Border(TableBorder.Square);
                AnsiConsole.Write(table);
                AnsiConsole.WriteLine();

                var select_menu = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "Insert new", "Return to menu" }));
                switch (select_menu)
                {
                    case "Insert new":
                        Insert(Program.CreateNewSession());
                        break;
                    case "Return to menu":

                        Program.Main();
                        break;
                };
            }
        }
        public void Goals()
        {
            using (var conn = new SQLiteConnection(MyConnection))
            {
                var output = conn.Query<CodingSession>("SELECT * FROM Sessions", new DynamicParameters());

                string select_period = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Select a period of time to see how far you are to achieve a [green]goal[/]")
        .AddChoices(new[] { "This week", "This month", "This year" }));
                AnsiConsole.Clear();
                int goal = AnsiConsole.Ask<int>($"How many coding hours you want to achieve [green]{select_period}[/]? ");

                List<CodingSession> final_sessions = output.ToList();

                foreach (var item in output) if (item.Date.Year != DateTime.Now.Year) final_sessions.Remove(item);

                switch (select_period)
                {
                    case "This week":
                        var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
                        var d2 = DateTime.Now.Date.AddDays(-1 * (int)cal.GetDayOfWeek(DateTime.Now));

                        foreach (var item in output)
                        {
                            if (item.Date.Month != DateTime.Now.Month) final_sessions.Remove(item);
                            else
                            {
                                var d1 = item.Date.Date.AddDays(-1 * (int)cal.GetDayOfWeek(item.Date));
                                if (d1 != d2) final_sessions.Remove(item);
                            }
                        }
                        break;
                    case "This month":
                        foreach (var item in output) if (item.Date.Month != DateTime.Now.Month) final_sessions.Remove(item);
                        break;
                    case "This year":
                        break;
                }
                TimeSpan total_duration = new();
                foreach (var item in final_sessions) total_duration += item.Duration;

                AnsiConsole.Clear();
                AnsiConsole.Write(new BarChart()
    .Label($"[green bold]Comparison of your total coding hours {select_period} and your goal[/]")
    .CenterLabel()
    .AddItem("Total hours", (int)total_duration.TotalHours, Color.Green)
    .AddItem("Goal", goal, Color.Red));

                int how_many_coding_last_for_goal = goal - (int)total_duration.TotalHours;

                if (how_many_coding_last_for_goal > 0)
                {
                    int how_many_days_left = AnsiConsole.Ask<int>("In how many days you want to achieve your goal?");
                    AnsiConsole.MarkupLine($"To achieve your goal in time you need to code[underline][green] {how_many_coding_last_for_goal / how_many_days_left} [/][/]hours every day");
                }
                else AnsiConsole.MarkupLine("[green]Looks like you already achivied your goal!![/]");

                select_period = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(new[] { "See a new goal", "Return to menu" }));
                if (select_period == "See a new goal") Goals();
                Program.Main();
            }
        }

    }
}