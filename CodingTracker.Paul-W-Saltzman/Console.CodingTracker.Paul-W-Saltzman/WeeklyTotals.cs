
using System.Data;
using System.Globalization;


namespace CodingTracker.Paul_W_Saltzman
{
    internal class WeeklyTotals
    {
        internal int WeeklyId;
        internal int YearWeek;
        internal TimeSpan TotalTime;
        internal bool GoalMet;
        internal bool TrophyPresented;


        internal static WeeklyTotals CalculateWeeklyTotal(WeeklyTotals weekly,int option)
        {
            List<CodingSession> sessions = Data.LoadSessions();
            foreach (CodingSession session in sessions)
            {
                if (weekly.YearWeek == YearWeekCreator(session.StartTime))
                {
                    weekly.TotalTime = weekly.TotalTime + session.TimeSpan; //summing up the date
                }

            }
            weekly = Goals.CheckGoals(weekly);
            weekly = Data.AddUpdateWeeklyTotals(weekly);
            List<WeeklyTotals> weeklyTotals = new List<WeeklyTotals>();
            weeklyTotals.Add(weekly);
            if (option == 1)
            {
                
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Helpers.ShowTable(BuildDataTable(weeklyTotals), "This Weeks Totals");
                Helpers.CenterText("Press Any Key to Continue");
                Helpers.CenterCursor();
                Console.ReadKey();
            }
            return weekly;
        }

        internal static int YearWeekCreator(DateTime dateTime)
        {
            int week = GetWeekOfYear(dateTime);
            int year = dateTime.Year;
            if (week == 53) 
            {
                week = 1;
                year = year + 1;
            }
            int yearWeek = (year * 100) + week;
            return yearWeek;
        }


        internal static int GetWeekOfYear(DateTime dateTime)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(dateTime, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }

        internal static DataTable BuildDataTable(List<WeeklyTotals> weeklyTotals)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id", typeof(int));
            dataTable.Columns.Add("Year", typeof(int));
            dataTable.Columns.Add("Week", typeof(int));
            dataTable.Columns.Add("TotalTime", typeof(TimeSpan));
            dataTable.Columns.Add("GoalMet", typeof(string));

            foreach (WeeklyTotals weeklyTotal in weeklyTotals)
            {
                string starString = weeklyTotal.GoalMet ? "*" : "";
                int year = weeklyTotal.YearWeek / 100;
                int week = weeklyTotal.YearWeek % 100;

                dataTable.Rows.Add(weeklyTotal.WeeklyId, year, week, weeklyTotal.TotalTime, starString);
            }

            return dataTable;
        }

        internal static List<WeeklyTotals> WeeklyBubbleSort(List<WeeklyTotals> weeklyTotals)
        {
            int n = weeklyTotals.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (weeklyTotals[j].YearWeek > weeklyTotals[j + 1].YearWeek)
                    {
                        // Swap the sessions
                        WeeklyTotals temp = weeklyTotals[j];
                        weeklyTotals[j] = weeklyTotals[j + 1];
                        weeklyTotals[j + 1] = temp;
                    }
                }
            }
            return weeklyTotals;
        }

        internal static List<WeeklyTotals> ReverseWeeklyBubbleSort(List<WeeklyTotals> weeklyTotals)
        {
            int n = weeklyTotals.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (weeklyTotals[j].YearWeek < weeklyTotals[j + 1].YearWeek)
                    {
                        // Swap the sessions
                        WeeklyTotals temp = weeklyTotals[j];
                        weeklyTotals[j] = weeklyTotals[j + 1];
                        weeklyTotals[j + 1] = temp;
                    }
                }
            }
            return weeklyTotals;
        }
    }
}
