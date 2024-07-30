using DataAcess;
using Model;
using System.Globalization;
using System.Numerics;
using Spectre.Console;

namespace CodingTrackerApp
{
    public class Logic
    {
        public static void Do(BigInteger userInput, out bool status)
        {
            status = true;
            switch((int)userInput)
            {
                case 0:
                    Thread.Sleep(1000);
                    Console.WriteLine("Exiting...\n");
                    Environment.Exit(0);
                    status = false;
                    break;
                case 1:
                    GetAllSession();
                    break;
                case 2:
                    InsertSession();
                    break;
                case 3:
                    UpdateSession();
                    break;
                case 4:
                    DeleteSession();
                    break;
                case 5:
                    Custom();
                    break;
            }
        }
        private static void GetAllSession()
        {
            Console.Clear();

            using (MyDbContext db = new MyDbContext())
            {
                IQueryable<CodingSession> codingSession = db.CodingSessions;

                if (!codingSession.Any()) AnsiConsole.Write(new Markup("\n[red]Database is Empty[/]\n"));
                else
                {
                    Table table = new Table();
                    table.Border = TableBorder.Ascii;
                    table.Title = new TableTitle("[black]All Session Record[/]");

                    table.AddColumn("Session ID");
                    table.AddColumn("Start Time");
                    table.AddColumn("End Time");
                    table.AddColumn("Duration");

                    foreach (CodingSession element in codingSession)
                    {
                        string formatStart = element.Start.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"));
                        string formatEnd = element.End.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"));

                        table.AddRow($"{element.CodingSessionID}", $"{formatStart}", $"{formatEnd}", $"{element.Duration} minutes");
                    }
                    AnsiConsole.Write(table);
                }
            }
        }
        private static void InsertSession()
        {
            Console.Clear();

            string strStartInput, strEndInput;
            DateTime startInput, endInput;
            bool isStartValid, isEndValid;
            double duration;

            Console.Write("Enter Start (yyyyMMddHHmm): ");
            strStartInput = UserInput.NumericInputOnly().ToString();
            isStartValid = IsValidDateTime(strStartInput, "yyyyMMddHHmm");
            Console.WriteLine();

            Console.Write("Enter End   (yyyyMMddHHmm): ");
            strEndInput = UserInput.NumericInputOnly().ToString();
            isEndValid = IsValidDateTime(strEndInput, "yyyyMMddHHmm");
            Console.WriteLine();

            if (isStartValid && isEndValid)
            {
                startInput = DateTimeConstruct(strStartInput);
                endInput = DateTimeConstruct(strEndInput);
                duration = CalculateDuration(startInput, endInput);

                if (duration > 0)
                {
                    using (MyDbContext db = new MyDbContext())
                    {
                        CodingSession codingSession = new CodingSession
                        {
                            Start = startInput,
                            End = endInput,
                            Duration = duration
                        };
                        db.CodingSessions.Add(codingSession);
                        db.SaveChanges();
                        AnsiConsole.Write(new Markup("[red]Added[/]\n"));
                    }
                } else
                {
                    AnsiConsole.Write(new Markup("\n[red]End < Start. Try again![/]\n"));
                    Thread.Sleep(1000);
                    InsertSession();
                }
            } else
            {
                AnsiConsole.Write(new Markup("\n[red]Invalid Entry. Try again![/]\n"));
                Thread.Sleep(1000); 
                InsertSession();
            }  
        }
        private static void UpdateSession()
        {
            Console.Clear();
            GetAllSession();

            int updateKey;

            Console.Write("\nUpdate Session (Enter ID): ");
            updateKey = (int)UserInput.NumericInputOnly();
            Console.WriteLine();

            using (MyDbContext db = new MyDbContext())
            {
                if (db.CodingSessions.Find(updateKey) == null) AnsiConsole.Write(new Markup("[red]Session not found[/]\n"));
                else
                {
                    Dictionary<string, int> choices = new Dictionary<string, int>
                    {
                        { "Update Start Time", 1 },
                        { "Update End Time", 2 }
                    };

                    string choice = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .AddChoices(choices.Keys)
                        .HighlightStyle(Style.Parse("red"))
                        ); 
                    int userInput = choices[choice];

                    switch (userInput)
                    {
                        case 1:
                            Console.Write("Enter New Start Time (yyyyMMddHHmm): ");
                            string start = UserInput.NumericInputOnly().ToString();
                            bool isStartValid = IsValidDateTime(start, "yyyyMMddHHmm");

                            if (isStartValid)
                            {
                                CodingSession? codingSession = db.CodingSessions.Find(updateKey);
                                if (codingSession != null)
                                {
                                    DateTime newStart = DateTimeConstruct(start);
                                    codingSession.Start = newStart;

                                    double newDuration = CalculateDuration(newStart, codingSession.End);
                                    codingSession.Duration = newDuration;

                                    db.SaveChanges();

                                    AnsiConsole.Write(new Markup("\n[red]Updated[/]\n"));
                                    Thread.Sleep(1000);
                                    GetAllSession();
                                }
                            }
                            else
                            {
                                AnsiConsole.Write(new Markup("\n[red]Invalid Entry. Try again![/]\n"));
                                Thread.Sleep(1000);
                                UpdateSession();
                                return;
                            }
                            break;
                        case 2:
                            Console.Write("Enter New End Time (yyyyMMddHHmm): ");
                            string end = UserInput.NumericInputOnly().ToString();
                            bool isEndValid = IsValidDateTime(end, "yyyyMMddHHmm");

                            if (isEndValid)
                            {
                                CodingSession? codingSession = db.CodingSessions.Find(updateKey);
                                if (codingSession != null)
                                {
                                    DateTime newEnd = DateTimeConstruct(end);
                                    codingSession.End = newEnd;

                                    double newDuration = CalculateDuration(codingSession.Start, newEnd);
                                    if (newDuration > 0)
                                    {
                                        codingSession.Duration = newDuration;
                                        db.SaveChanges();

                                        AnsiConsole.Write(new Markup("\n[red]Updated[/]\n"));
                                        Thread.Sleep(1000);
                                        GetAllSession();
                                    }
                                    else
                                    {
                                        AnsiConsole.Write(new Markup("\n[red]End < Start. Try again![/]\n"));
                                        Thread.Sleep(1000);
                                        UpdateSession();
                                        return;
                                    }   
                                }
                            }
                            else
                            {
                                AnsiConsole.Write(new Markup("\n[red]Invalid Entry. Try again![/]\n"));
                                Thread.Sleep(1000);
                                UpdateSession();
                                return;
                            }
                            break;
                    }
                }
            }
        }
        private static void DeleteSession()
        {
            Console.Clear();
            GetAllSession();

            int primaryKey;
            CodingSession? toBeDeleted;

            using (MyDbContext db = new MyDbContext())
            {
                IQueryable<CodingSession> codingSession = db.CodingSessions;
                if (!codingSession.Any()) return;
                else
                {
                    Console.Write($"\nSession Count: {codingSession.Count()}\n");
                    Console.Write($"Delete Session (Enter ID): ");
                    primaryKey = (int)UserInput.NumericInputOnly();
                    toBeDeleted = db.CodingSessions.Find(primaryKey);
                }
                if (toBeDeleted == null) AnsiConsole.Write(new Markup("\n[red]Session not found[/]\n"));
                else
                {
                     db.CodingSessions.Remove(toBeDeleted);
                    db.SaveChanges();
                    AnsiConsole.Write(new Markup("\n[red]Deleted[/]\n"));
                }
            }
        }
        private static double CalculateDuration(DateTime start, DateTime end)
        {
            TimeSpan duration = end - start;
            return duration.TotalMinutes;
        }
        private static DateTime DateTimeConstruct(string dateTime)
        {
            List<string> dateTimeString = new List<string>();

            for (int i = 0; i < dateTime.Length; i = i + 2)
            {
                dateTimeString.Add(dateTime.Substring(i, 2));
            }

            List<int> dateTimeInt = dateTimeString.Select(x =>
            {
                int.TryParse(x, out int result);
                return new { result };
            }).Select(x => x.result).ToList();

            int.TryParse(dateTimeString[0] + dateTimeString[1], out int year);

            DateTime dateTimeOut = new DateTime(
                year,
                dateTimeInt[2],
                dateTimeInt[3],
                dateTimeInt[4],
                dateTimeInt[5],
                0
                );
            return dateTimeOut;
        }
        private static bool IsValidDateTime(string input, string format)
        {
            return DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        private static void Custom()
        {
            Console.Clear();

            Dictionary<string, int> choices = new Dictionary<string, int>
            {
                { "Sort Sessions", 1 },
                { "User Report", 2 }
            };
            string choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .AddChoices(choices.Keys)
                .HighlightStyle(Style.Parse("red"))
                );
            int userInput = choices[choice];

            switch (userInput)
            {
                case 1:
                    using (MyDbContext db = new MyDbContext())
                    {
                        IQueryable<CodingSession> codingSession = db.CodingSessions;
                        if (!codingSession.Any()) AnsiConsole.Write(new Markup("\n[red]Database is Empty[/]\n"));
                        else
                        {
                            Table table = new Table();
                            table.Border = TableBorder.Ascii;
                            table.Title = new TableTitle("[black]Descending Order Sort[/]");

                            table.AddColumn("Session ID");
                            table.AddColumn("Start Time");
                            table.AddColumn("End Time");
                            table.AddColumn("Duration");

                            IQueryable<CodingSession> sort = codingSession.OrderByDescending(x => x.Duration);

                            foreach (CodingSession element in sort)
                            {
                                string formatStart = element.Start.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"));
                                string formatEnd = element.End.ToString("dd MMMM yyyy HH:mm:ss", new CultureInfo("en-US"));

                                table.AddRow($"{element.CodingSessionID}", $"{formatStart}", $"{formatEnd}", $"{element.Duration} minutes");
                            }
                            AnsiConsole.Write(table);
                        }
                    }
                    break;
                case 2:
                    using (MyDbContext db = new MyDbContext())
                    {
                        double totalMinutes = db.CodingSessions.Sum(x => x.Duration);
                        double avgMinutes = db.CodingSessions.Average(x => x.Duration);
                        int count = db.CodingSessions.Count();

                        Panel panel = new Panel($"Total number of sessions: {count}" +
                            $"\nTotal session minutes: {totalMinutes}" +
                            $"\nAverage minutes per session: {Math.Round(avgMinutes, 2)}");

                        panel.Header = new PanelHeader("User Report");
                        panel.Border = BoxBorder.Ascii;

                        AnsiConsole.Write(panel);
                    }
                    break;
            }

            
        }
    }
}
