using CodingTracker.library.View;

namespace CodingTracker.library.Controller;

internal static class CodingSessionGoalHour
{
    internal static void CodingHoursGoalCheck()
    {
        Console.Clear();

        double goalHours = Helpers.GetGoalHours() * 60;

        DateTime lastWeekStartDate = DateTime.Now.AddDays(-7);
        DateTime lastWeekEndDate = lastWeekStartDate.AddDays(6).AddHours(23);

        string startDate = lastWeekStartDate.ToString("dd-MM-yyyy HH:mm");
        string endDate = lastWeekEndDate.ToString("dd-MM-yyyy HH:mm");

        double lastWeekTotalHours = QueriesCodingSessionGoal.TotalLastWeekCodingHoursQuery(startDate, endDate);

        if (lastWeekTotalHours > goalHours) 
        {
            
            double difference = lastWeekTotalHours - goalHours;
            string message = $"You haven't reached your coding hours goal.\nYou need to code extra {difference / 60} hours to reach your goal";
            TableVisualizationEngine.PrintGoalStatus(goalHours, lastWeekTotalHours,message);
        }

        else
        {
           string message = $"You have reached your coding hours goal.";
            TableVisualizationEngine.PrintGoalStatus(goalHours, lastWeekTotalHours,message);
        }
    }
}
