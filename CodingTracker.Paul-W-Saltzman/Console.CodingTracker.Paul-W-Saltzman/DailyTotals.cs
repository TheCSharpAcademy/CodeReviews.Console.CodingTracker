
using System.Data;

namespace CodingTracker.Paul_W_Saltzman
{
    internal class DailyTotals
    {
        internal int DailyId;
        internal DateOnly Date;
        internal TimeSpan TotalTime;
        internal bool GoalMet;
        internal bool TrophyPresented;

        internal static DailyTotals CalculateDailyTotal(DailyTotals daily, int option)
        {
            List<CodingSession> sessions = Data.LoadSessions();
            foreach (CodingSession session in sessions)
            {
                if (daily.Date == DateOnly.FromDateTime(session.StartTime))
                {
                    daily.TotalTime = daily.TotalTime + session.TimeSpan; //summing up the date
                }

            }
            daily = Goals.CheckGoals(daily);
            daily = Data.AddUpdateDailyTotals(daily);
            List<DailyTotals> dailyTotal = new List<DailyTotals>();
            dailyTotal.Add(daily);
            if (option == 1)
            {
            Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
            Console.Clear();
            Helpers.ShowTable(BuildDataTable(dailyTotal), "Todays Totals");
            Helpers.CenterText("Press Any Key to Continue");
            Helpers.CenterCursor();
            Console.ReadKey();
            }
            return daily;
        }

        internal static List<DailyTotals> DailyBubbleSort(List<DailyTotals> dailyTotals)
        {
            int n = dailyTotals.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (dailyTotals[j].Date > dailyTotals[j + 1].Date)
                    {
                        // Swap the sessions
                        DailyTotals temp = dailyTotals[j];
                        dailyTotals[j] = dailyTotals[j + 1];
                        dailyTotals[j + 1] = temp;
                    }
                }
            }
            return dailyTotals;
        }

        internal static List<DailyTotals> ReverseDailyBubbleSort(List<DailyTotals> dailyTotals)
        {
            int n = dailyTotals.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - 1 - i; j++)
                {
                    if (dailyTotals[j].Date < dailyTotals[j + 1].Date)
                    {
                        // Swap the sessions
                        DailyTotals temp = dailyTotals[j];
                        dailyTotals[j] = dailyTotals[j + 1];
                        dailyTotals[j + 1] = temp;
                    }
                }
            }
            return dailyTotals;
        }
        internal static DataTable BuildDataTable(List<DailyTotals> dailyTotals)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id",typeof(int));
            dataTable.Columns.Add("Date", typeof(DateOnly));
            dataTable.Columns.Add("TotalTime", typeof(TimeSpan));
            dataTable.Columns.Add("GoalMet", typeof(string));

            foreach (DailyTotals dailyTotal in dailyTotals) 
            {
                string starString = dailyTotal.GoalMet ? "*" : "";

                dataTable.Rows.Add(dailyTotal.DailyId, dailyTotal.Date, dailyTotal.TotalTime,starString);
            }

            return dataTable;
        }

    }
}
