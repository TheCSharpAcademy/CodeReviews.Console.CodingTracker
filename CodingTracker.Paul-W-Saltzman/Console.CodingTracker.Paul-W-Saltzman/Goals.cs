
using System.Data;

namespace CodingTracker.Paul_W_Saltzman
{
    internal class Goals
    {
        internal int GoalId;
        internal String GoalType;
        internal TimeSpan Goal;

        internal static DailyTotals CheckGoals(DailyTotals dailyTotal)
        {
            Goals dailyGoal = Data.LoadSingleGoal(1);
            if (dailyTotal.TotalTime >= dailyGoal.Goal)
            {
                dailyTotal.GoalMet = true;
            }
            else if (dailyTotal.TotalTime < dailyGoal.Goal)
            {
                dailyTotal.GoalMet = false;
            }
            return dailyTotal;
        }

        internal static WeeklyTotals CheckGoals(WeeklyTotals weeklyTotal)
        {
            
            Goals weeklyGoal = Data.LoadSingleGoal(2);
            if (weeklyTotal.TotalTime >= weeklyGoal.Goal)
            {
                weeklyTotal.GoalMet = true;     
            }
            else if (weeklyTotal.TotalTime < weeklyGoal.Goal)
            {
                weeklyTotal.GoalMet = false;
            }
            return weeklyTotal;  
        }
        internal static void ModifyGoals(int goalID)
        {
            Goals goal = Data.LoadSingleGoal(goalID);
            Console.SetCursorPosition(0, Console.CursorTop -3);
            Helpers.ClearLine();
            Helpers.CenterText($@"Please Enter the Time Span of the Goal {goal.GoalType}. Format hh:mm:ss");
            Helpers.ClearLine();
            Console.WriteLine();
            Helpers.ClearLine();
            Helpers.CenterCursor();
            string userInput = Console.ReadLine();
            userInput.Trim();
            bool parsible = UserInput.CanParseTimeSpan(userInput);
            if (parsible)
            {
                TimeSpan goalTime = UserInput.ParseTimeSpan(userInput);
                Data.UpdateGoal(goal, goalTime);
            }
            else
            {
                Console.SetCursorPosition(0, Console.CursorTop - 3);
                Helpers.ClearLine();
                Helpers.CenterText("Invalid entry press ENTER to continue.");
                Helpers.ClearLine();
                Console.WriteLine();
                Helpers.ClearLine();
                Helpers.CenterCursor();
                Console.ReadLine();
            }
            
        }

        internal static DataTable BuildGoalTable()
        {
            List<Goals> goals = Data.LoadGoals();
            DataTable goalTable = new DataTable();
            goalTable.Columns.Add("Id",typeof(int));
            goalTable.Columns.Add("Type", typeof(string));
            goalTable.Columns.Add("Goal", typeof(TimeSpan));

            foreach (Goals goal in goals) 
            {
                goalTable.Rows.Add(goal.GoalId, goal.GoalType, goal.Goal);
            }
            return goalTable;
        }
    }

    
}
