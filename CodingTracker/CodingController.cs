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

            AnsiConsole.WriteLine("Saved");
        }

        public void Read()
        {
            AnsiConsole.WriteLine("Use Filter?");
            bool confirmation = UserInput.Confirm();
            Enums.SessionFilter? filter = null;
            if (confirmation)
            {
                filter = UserInput.SelectFilter();
            }

            AnsiConsole.WriteLine("Use Order?");
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
            // selectedSession.UpdateDuration();

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
                AnsiConsole.WriteLine("Press Any Key To Stop");
                _ = Console.ReadKey();

                DateTime end = DateTime.Now;
                codingSession = new(start, end);
                AnsiConsole.WriteLine($"You have coded for {DurationFormatter.DurationToHourString(codingSession.Duration)}, Confirm to stop");
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
            _ = readTable.AddColumn("Period")
                .AddColumn("Total Session")
                .AddColumn("Total Duration");

            _ = readTable.AddRow(new Text("Today"), new Text(report.CountToday.ToString()), new Text(DurationFormatter.DurationToHourString(report.TotalToday)));
            _ = readTable.AddRow(new Text("This Week"), new Text(report.CountWeek.ToString()), new Text(DurationFormatter.DurationToHourString(report.TotalWeek)));
            _ = readTable.AddRow(new Text("This Year"), new Text(report.CountYear.ToString()), new Text(DurationFormatter.DurationToHourString(report.TotalYear)));
            _ = readTable.AddRow(new Text("All Time"), new Text(report.Count.ToString()), new Text(DurationFormatter.DurationToHourString(report.Total)));

            AnsiConsole.Write(readTable);
        }

        public void Goal()
        {
            CodingGoal? activeGoal = database.GetActiveCodingGoal();
            if (activeGoal is null)
            {
                AnsiConsole.WriteLine("No Active Goal. Create New Goal?");
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
                AnsiConsole.WriteLine("Congratulation, Goal Achieved");
                return;
            }

            if (goal.End < DateTime.Now)
            {
                goal.IsFinished = 1;
                goal.IsAchieved = 0;
                database.UpdateCodingGoal(goal);
                AnsiConsole.WriteLine("You Ran Out of Time");
                return;
            }
            AnsiConsole.WriteLine($"You coded for {DurationFormatter.DurationToHourString(totalDuration)}, remaining duration {DurationFormatter.DurationToHourString(remainingDuration)}");
            int remainingDays = goal.RemainingDays;
            double dailyAverageNeeded = goal.GetDailyAverageNeeded(remainingDuration);
            AnsiConsole.WriteLine($"You need to do {DurationFormatter.DurationToHourString(dailyAverageNeeded)} daily for {remainingDays} days to achieve your goal");

            return;
        }

        private static void ShowCodingSessionTable(List<CodingSession> codingSessions)
        {
            Table readTable = new();
            _ = readTable.AddColumn("Id")
            .AddColumn("Start")
            .AddColumn("End")
            .AddColumn("Duration");
            foreach (CodingSession cs in codingSessions)
            {
                // TODO: handle locale thing in one place
                _ = readTable.AddRow(new Text(cs.Id.ToString()), new Text(cs.Start.ToString(DateFormats.DateStringFormat)), new Text(cs.End.ToString(DateFormats.DateStringFormat)), new Text(DurationFormatter.DurationToHourString(cs.Duration)));
            }

            AnsiConsole.Write(readTable);
        }

        private static void ShowFilteredCodingSessionTable(List<FilteredCodingSession> filteredCodingSessions)
        {
            Table readTable = new();
            _ = readTable.AddColumn("Period")
                .AddColumn("Total Duration");
            foreach (FilteredCodingSession fcs in filteredCodingSessions)
            {
                // TODO: handle locale thing in one place
                _ = readTable.AddRow(new Text(fcs.FilterId), new Text(DurationFormatter.DurationToHourString(fcs.Duration)));
            }

            AnsiConsole.Write(readTable);
        }
    }
}
