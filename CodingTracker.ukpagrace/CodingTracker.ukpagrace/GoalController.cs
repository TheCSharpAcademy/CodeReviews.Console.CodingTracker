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
            DateTime date = DateTime.Now;
            string month = $"{date.Year}-{date.Month:D2}";

            var result = goal.GoalProgress(month);
            var codingGoals = result.Item1;
            var codingHours = result.Item2;

            if(codingGoals == -1)
            {
                AnsiConsole.MarkupLine("[red] No Coding Goal has been set[/]");
                return;
            }
            var remainingHours = codingGoals - codingHours;
            var surpassedHours = codingHours - codingGoals;

            if (result.Item2 < result.Item1)
            {
                AnsiConsole.MarkupLine($"[yellow]Your coding goal this month is {codingGoals} hours and you have coded {codingHours} hours, you have {remainingHours} hours left to reach your goal[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[blue]You have surpassed your goal by {surpassedHours} hours,[/]  [yellow] Weldone Tiger[/]");
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
