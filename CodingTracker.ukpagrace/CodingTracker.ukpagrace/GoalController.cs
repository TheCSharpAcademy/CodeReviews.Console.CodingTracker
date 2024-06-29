using DatabaseLibrary;
using Spectre.Console;

namespace CodingTracker.ukpagrace
{
    
    internal class GoalController
    {
        readonly Goal goal = new();
        readonly Utility utility = new ();
        readonly Validation validate = new ();
        readonly UserInput userInput = new ();
        public void CreateGoalTable()
        {
            goal.Create();
        }

        public async void SetGoal()
        {
            int goalInput = validate.ValidateGoal();

            DateTime date = DateTime.Now;
            string month = $"{date.Year}-{date.Month:D2}";

            if (goal.GoalCreated(month))
            {
                bool updateGoal = userInput.ConfirmAction("Goal for the Month has been created would you like to update it?");

                if (updateGoal)
                {
                    goal.Update(month, goalInput);
                }
                else
                {
                    GetAverageCodePerDay();
                }
            }
            else
            {
                int affectedRows = await goal.Insert(month, goalInput);
                AnsiConsole.MarkupLine($"[white]{affectedRows}[/] [yellow]row(s) inserted[/]");
                GetAverageCodePerDay();
            }
        }

        public void SeeGoalProgress()
        {
            try
            {
                DateTime date = DateTime.Now;
                string month = $"{date.Year}-{date.Month:D2}";

                var result = goal.GoalProgress(month);
                var codingGoals = result.Item1;
                var totalDuration = result.Item2;

                string formattedCodedHours = utility.FormatTimeSpan(totalDuration);
                string codedHours = string.IsNullOrEmpty(formattedCodedHours) ? "nothing" : utility.FormatTimeSpan(totalDuration);

                int TotalToMilliseconds = (int)TimeSpan.Parse(totalDuration.ToString()).TotalMilliseconds;
                int codingHours = (int)TimeSpan.FromMilliseconds(TotalToMilliseconds).TotalHours;

                TimeSpan goalsTimeSpan = TimeSpan.FromHours(codingGoals);

                var remainingHours = utility.FormatTimeSpan(goalsTimeSpan - totalDuration);
                var surpassedHours = utility.FormatTimeSpan( totalDuration - goalsTimeSpan);

                if (codingHours < result.Item1)
                {
                    AnsiConsole.MarkupLine($"[yellow]Your coding goal this month is {codingGoals} hours and you have coded {codedHours}, you have {remainingHours} left to reach your goal[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"[blue]You have surpassed your goal by {surpassedHours}[/].");
                    AnsiConsole.Write(
                        new FigletText("Weldone Tiger")
                            .Centered()
                            .Color(Color.DodgerBlue3));
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine("[red]ex[/]");
            }
        }

        void GetAverageCodePerDay()
        {
            DateTime date = DateTime.Now;
            string month = $"{date.Year}-{date.Month:D2}";
            int days = Convert.ToInt32(DateTime.DaysInMonth(date.Year, date.Month));
            double codePerDay = goal.AverageTimePerDay(month, days);
            TimeSpan timespan = TimeSpan.FromHours(codePerDay);
            string timespanString = utility.FormatTimeSpan(timespan);

            AnsiConsole.MarkupLine($"[yellow]To achieve your goal this month you have to code a minimun of {timespanString} everday[/]");
            AnsiConsole.Write(
                new FigletText("GoodLuck")
                    .LeftJustified()
                    .Color(Color.Chartreuse3));
        }
    }
}
