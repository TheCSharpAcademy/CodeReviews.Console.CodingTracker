using Spectre.Console;

namespace CodingTracker
{
    public class CodingController(Database db)
    {
        private readonly Database database = db;

        public void Create()
        {
            CodingSession obj = UserInput.PromptNewCodingSession();
            database.Save(obj);

            AnsiConsole.WriteLine("saved");
        }

        public void Read()
        {
            AnsiConsole.WriteLine("filter?");
            bool confirmation = UserInput.Confirm();
            Enums.SessionFilter? filter = null;
            if (confirmation)
            {
                filter = UserInput.SelectFilter();
            }

            AnsiConsole.WriteLine("order?");
            confirmation = UserInput.Confirm();
            Enums.SessionOrder? order = null;
            if (confirmation)
            {
                order = UserInput.SelectOrder();
            }

            List<FilteredCodingSession> filteredCodingSessions;
            switch (filter)
            {
                case Enums.SessionFilter.Day:
                    filteredCodingSessions = database.GetGroupByDay(order);
                    ShowFilteredCodingSessionTable(filteredCodingSessions);
                    break;
                case Enums.SessionFilter.Week:
                    filteredCodingSessions = database.GetGroupByWeek(order);
                    ShowFilteredCodingSessionTable(filteredCodingSessions);
                    break;
                case Enums.SessionFilter.Year:
                    filteredCodingSessions = database.GetGroupByYear(order);
                    ShowFilteredCodingSessionTable(filteredCodingSessions);
                    break;
                default:
                    List<CodingSession> codingSessions = database.Get(order);
                    ShowCodingSessionTable(codingSessions);
                    break;
            }
        }

        public void Update()
        {
            List<CodingSession> codingSessions = database.Get();
            if (codingSessions.Count == 0)
            {
                AnsiConsole.WriteLine("There is no record of coding session");
                return;
            }

            CodingSession selectedSession = UserInput.SelectCodingSession(codingSessions);

            CodingSession updatedSession = UserInput.PromptNewCodingSession(selectedSession);
            selectedSession.Start = updatedSession.Start;
            selectedSession.End = updatedSession.End;
            selectedSession.UpdateDuration();

            database.Update(selectedSession);
        }

        public void Delete()
        {
            List<CodingSession> updateData = database.Get();
            CodingSession deletedId = UserInput.SelectCodingSession(updateData);
            ShowCodingSessionTable([deletedId]);
            bool confirmation = UserInput.Confirm();
            database.Delete(deletedId);

            AnsiConsole.WriteLine(confirmation);
        }

        public void Start()
        {
            CodingSession? codingSession = null;
            DateTime start = DateTime.Now;

            bool confirmation = false;
            while (!confirmation)
            {
                AnsiConsole.WriteLine("Happy Coding");
                AnsiConsole.WriteLine("Press any key to stop");
                _ = Console.ReadKey();

                DateTime end = DateTime.Now;
                codingSession = new(start, end);
                AnsiConsole.WriteLine($"You have coded for {codingSession.DurationToString()}, Confirm to stop");
                confirmation = UserInput.Confirm();
            }

            if (codingSession != null)
            {
                database.Save(codingSession);
            }
        }

        public void Report()
        {
            Report report = database.GetReport();
            Table readTable = new();
            _ = readTable.AddColumn("period")
                .AddColumn("total session")
                .AddColumn("total duration");

            _ = readTable.AddRow(new Text("Today"), new Text(report.CountToday.ToString()), new Text(report.TodayDurationToString()));
            _ = readTable.AddRow(new Text("This Week"), new Text(report.CountWeek.ToString()), new Text(report.WeekDurationToString()));
            _ = readTable.AddRow(new Text("This Year"), new Text(report.CountYear.ToString()), new Text(report.YearDurationToString()));
            _ = readTable.AddRow(new Text("All Time"), new Text(report.Count.ToString()), new Text(report.TotalDurationToString()));

            AnsiConsole.Write(readTable);
        }

        public void Goal()
        {
            CodingGoal? activeGoal = database.GetActiveCodingGoal();
            if (activeGoal is null)
            {
                if (!UserInput.Confirm())
                {
                    return;
                }

                CreateGoal();
            }

            ShowGoalProgress();

            return;
        }

        private void CreateGoal()
        {
            CodingGoal obj = UserInput.PromptNewCodingGoal();
            database.SaveCodingGoal(obj);
            return;
        }

        private void ShowGoalProgress()
        {
            CodingGoal? goal = database.GetActiveCodingGoal();
            if (goal is null)
            {
                return;
            }

            // get all from start till now, see if achieved
            List<CodingSession> codingSessions = database.GetAllCodingSessionsBetweenDates(goal.Start, goal.End);
            double totalDuration = 0.0;
            foreach (CodingSession cs in codingSessions)
            {
                totalDuration += cs.Duration;
            }

            // TODO: goal duration is in hours, maybe to someting to class to compare with session duration
            double remainingDuration = goal.GetRemainingDuration(totalDuration);
            if (remainingDuration <= 0)
            {
                // update if achieved
                goal.IsFinished = 1;
                goal.IsAchieved = 1;
                database.UpdateCodingGoal(goal);
                AnsiConsole.WriteLine("congrats, goal achieved");
                return;
            }

            if (goal.End < DateTime.Now)
            {
                goal.IsFinished = 1;
                goal.IsAchieved = 0;
                database.UpdateCodingGoal(goal);
                AnsiConsole.WriteLine("time out");
                return;
            }
            // TODO: change duration to readable format
            AnsiConsole.WriteLine($"you did {totalDuration} sec, remaining duration {remainingDuration}");
            int remainingDays = goal.RemainingDays;
            double dailyAverageNeeded = goal.GetDailyAverageNeeded(remainingDuration);
            AnsiConsole.WriteLine($"you need to do {dailyAverageNeeded} for {remainingDays} days to achieve your goal");

            return;
        }

        private static void ShowCodingSessionTable(List<CodingSession> codingSessions)
        {
            Table readTable = new();
            _ = readTable.AddColumn("id")
            .AddColumn("start")
            .AddColumn("end")
            .AddColumn("duration");
            foreach (CodingSession cs in codingSessions)
            {
                // TODO: handle locale thing in one place
                cs.UpdateDuration();
                _ = readTable.AddRow(new Text(cs.Id.ToString()), new Text(cs.Start.ToString()), new Text(cs.End.ToString()), new Text(cs.DurationToStringComplete()));
            }

            AnsiConsole.Write(readTable);
        }

        private static void ShowFilteredCodingSessionTable(List<FilteredCodingSession> filteredCodingSessions)
        {
            Table readTable = new();
            _ = readTable.AddColumn("filter id")
                .AddColumn("total duration");
            foreach (FilteredCodingSession fcs in filteredCodingSessions)
            {
                // TODO: handle locale thing in one place
                _ = readTable.AddRow(new Text(fcs.FilterId), new Text(fcs.DurationToStringComplete()));
            }

            AnsiConsole.Write(readTable);
        }
    }
}
