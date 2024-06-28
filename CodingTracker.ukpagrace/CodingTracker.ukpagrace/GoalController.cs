using DatabaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.ukpagrace
{
    
    internal class GoalController
    {
        Goal goal = new();
        Utility utility = new ();
        void CreateGoalTable()
        {
            goal.Create();
        }

        void SetGoal()
        {
            Console.WriteLine("Set a coding goal for this month, goal must be an integer");

            var userInput = Console.ReadLine();
            int goalInput;

            while (!int.TryParse(userInput, out goalInput) || Convert.ToInt32(userInput) < 0)
            {
                Console.WriteLine("Enter a valid goal");
                userInput = Console.ReadLine();
            }
            DateTime date = DateTime.Now;
            string month = $"{date.Year}-{date.Month:D2}";

            if (goal.GoalCreated(month))
            {
                Console.WriteLine("Goal for the Month has been created would you like to update it?");
                Console.WriteLine("1 - yes");
                Console.WriteLine("2 - no");
                userInput = Console.ReadLine();

                if (userInput == "1")
                {
                    goal.Update(month, goalInput);
                }
                else if (userInput == "2")
                {
                    GetAverageCodePerDay();
                }
                else
                {
                    Console.WriteLine("Invalid Input");
                }
            }
            else
            {
                goal.Insert(month, goalInput);
                GetAverageCodePerDay();
            }


        }

        void SeeGoalProgress()
        {
            DateTime date = DateTime.Now;
            string month = $"{date.Year}-{date.Month:D2}";

            goal.GoalProgress(month);
        }

        void GetAverageCodePerDay()
        {
            DateTime date = DateTime.Now;
            string month = $"{date.Year}-{date.Month:D2}";
            int days = Convert.ToInt32(DateTime.DaysInMonth(date.Year, date.Month));
            double codePerDay = goal.AverageTimePerDay(month, days);
            TimeSpan timespan = TimeSpan.FromHours(codePerDay);
            string timespanString = utility.FormatTimeSpan(timespan);

            Console.WriteLine("------------------------------------------------\n"); ;
            Console.WriteLine($"To achieve your goal this month you have to code a minimun of {timespanString} everday, GoodLuck");
            Console.WriteLine("------------------------------------------------\n"); ;
        }
    }
}
