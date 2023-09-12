using System.Diagnostics;
using System.Globalization;

namespace CodeTracker
{
    public class Manager
    {
        private SELECTOR Selector { get; set; }
        private SQLite SQL { get; set; }
        private List<CodingSession> SessionData { get; set; } = new();
        private Filter Filter { get; set; }
        private UI UI { get; set; }
        private int Goal { get; set; }
        public Manager()
        {
            SQL = new();
            UI = new UI();
            SessionData = SQL.GetSQLData();
            Filter = new(SessionData);
            Selector = UI.MainMenu();
            while (true)
            {
                Action();
            }
        }
        private void Action()
        {
            switch (Selector)
            {
                case SELECTOR.INSERT:
                    Insert();
                    break;
                case SELECTOR.DELETE:
                    Delete();
                    break;
                case SELECTOR.UPDATE:
                    Update();
                    break;
                case SELECTOR.DROP:
                    Drop();
                    break;
                case SELECTOR.VIEW:
                    View();
                    break;
                case SELECTOR.REPORT:
                    Report();
                    break;
                case SELECTOR.SET:
                    SetTheGoal();
                    break;
                case SELECTOR.EXIT:
                    Environment.Exit(0);
                    break;
                default:
                    UI.Write("Invalid Input");
                    break;
            }
            Selector = UI.GoToMainMenu("Type any keys to continue.");
        }
        private void Drop()
        {
            ViewAllRecords();
            SQL.DropTable();
            SessionData.Clear();
            SQL.CreateTable();
        }
        private void Insert()
        {
            ViewAllRecords();
            try
            {
                UI.Write("Track coding time.");
                UI.Write("The time input format should be like this : (yyyy-MM-dd HH:mm:ss)");
                UI.Write("Or press S to use Stopwatch.");

                var input = UI.GetInput("Input start time first.").str;
                
                if(input == "s")
                {
                    StopWatch();
                }
                else
                {
                    var start = Validation.ValidDateTime(input);
                    var end = Validation.ValidDateTime(UI.GetInput("Input end time.").str);

                    if (end < start) throw new Exception();

                    var code = new CodingSession(start, end);
                    SessionData.Add(code);
                    SQL.Insert(code);
                }

                ViewAllRecords();
            }
            catch
            {
                UI.Write("Invalid Input. Try again.");
            }
        }
        private void StopWatch()
        {
            Stopwatch stopwatch = new Stopwatch();
            var elapsed = new TimeSpan();
            var start = DateTime.Now;

            stopwatch.Start();
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true).Key;

                    if (key == ConsoleKey.S)
                    {
                        stopwatch.Stop();
                        UI.Write("Stopwatch stopped. Track recorded.");
                        elapsed = stopwatch.Elapsed;
                        var end = start + elapsed;
                        var code = new CodingSession(start, end);
                        SessionData.Add(code);
                        SQL.Insert(code);
                        break;
                    }
                }
                Console.Clear();
                elapsed = stopwatch.Elapsed;
                UI.Write($"Stopwatch is running. Elapsed Time: {elapsed}");
                Thread.Sleep(100);
            }
        }
        private void Delete()
        {
            ViewAllRecords();
            try
            {
                var input = UI.GetInput("Select the ID of log to delete.").val;
                SQL.Delete(input);
                for (int i = 0; i < SessionData.Count; i++)
                {
                    if ((int)SessionData[i].Id == input)
                    {
                        SessionData.RemoveAt(i);
                        break;
                    }
                }
            }
            catch
            {
                UI.Write("Invalid Input. Try again.");
            }
        }
        private void Update()
        {
            ViewAllRecords();
            try
            {
                var id = UI.GetInput("Select the ID of the log to update").val;
                UI.Write("The time input format should be like this : (yyyy-MM-dd HH:mm:ss)");
                var start = Validation.ValidDateTime(UI.GetInput("Input start time first.").str);
                var end = Validation.ValidDateTime(UI.GetInput("Input end time.").str);
                if (end < start) throw new Exception();

                for (int i = 0; i < SessionData.Count; i++)
                {
                    if ((int)SessionData[i].Id == id)
                    {
                        var code = new CodingSession(start, end);
                        code.Id = id;
                        SessionData[i] = code;
                        SQL.Update(code);
                        break;
                    }
                }
            }
            catch
            {
                UI.Write("Invalid Input. Try again.");
            }
        }

        private void ViewAllRecords()
        {
            var sessionList = SessionData.Select(session => session.GetField()).ToList();
            var period = "Records";
            UI.MakeTable(sessionList, period);
        }
        private void View()
        {
            List<List<object>> sessionList =
                SessionData.Select(session => session.GetField()).ToList();

            var param = UI.FilterMenu();
            Filter.SetParameters(param);
            var selector = (FILTER_SELECTOR)param[0];
            var period = "";

            if (selector == FILTER_SELECTOR.YEAR)
            {
                sessionList = Filter.FilterByYear();
                period = "Years";
            }
            else if (selector == FILTER_SELECTOR.WEEK)
            {
                sessionList = Filter.FilterByWeek();
                period = "Weeks";
            }
            else if (selector == FILTER_SELECTOR.DAY)
            {
                sessionList = Filter.FilterByDays();
                period = "Days";
            }
            else if (selector == FILTER_SELECTOR.ALL)
            {
                ViewAllRecords();
                return;
            }
            else return;

            UI.MakeTable(sessionList, period);
        }

        private void Report()
        {
            var select = UI.ReportMenu();
            Console.Clear();
            switch (select)
            {
                case 1:
                    ReportYearlySession();
                    break;
                case 2:
                    ReportWeeklySession();
                    break;
                case 3:
                    CheckGoal();
                    break;
                case 0:
                    break;
                default:
                    UI.Write("Invalid Input");
                    break;
            }
        }
        private void SetTheGoal()
        {
            Console.Clear();
            Goal = UI.GetInput("Set the coding goal for this week. (hour)").val;
            UI.Write("Your goal is set.");
        }

        private void CheckGoal()
        {
            UI.Write($"Your weekly goal is {Goal} hours");

            CultureInfo cultureInfo = new CultureInfo("en-US");
            Calendar calendar = cultureInfo.Calendar;
            CalendarWeekRule weekRule = cultureInfo.DateTimeFormat.CalendarWeekRule;
            DayOfWeek firstDayOfWeek = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            int year = calendar.GetYear(DateTime.Now);
            int week = calendar.GetWeekOfYear(DateTime.Now, weekRule, firstDayOfWeek) - 1;
            var day = calendar.GetDayOfWeek(DateTime.Now);

            double sum = 0;

            foreach (var session in SessionData)
            {
                sum += session.WeekDuration[$"{year}-{week}"];
            }

            if (sum >= Goal)
            {
                UI.Write("Good job! You already have accomplished the goal of the week!");
            }
            else
            {
                UI.Write($"You need to code {(Goal - sum) / (int)(7 - day)} hours a day more");
            }
        }

        private void ReportYearlySession()
        {
            var input = UI.GetInput("Input the year").val;
            double duration = 0;
            var count = 0;

            foreach (var session in SessionData)
            {
                if (session.YearDuration.ContainsKey(input))
                {
                    duration += session.YearDuration[input];
                    count++;
                }
            }
            UI.Write($"{input} \n======================\n total sessions : {count} \n total duration : {duration} \n average time : {duration / count}");
        }

        private void ReportWeeklySession()
        {
            var input = UI.GetInput("Input the week").str;
            double duration = 0;
            var count = 0;
            foreach (var session in SessionData)
            {
                if (session.WeekDuration.ContainsKey(input))
                {
                    duration += session.WeekDuration[input];
                    count++;
                }
            }
            UI.Write($"{input} \n======================\n total sessions : {count} \n total duration : {duration} \n average time : {duration / count}");
        }
    }
}
