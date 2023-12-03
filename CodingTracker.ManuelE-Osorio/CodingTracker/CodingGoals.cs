using System.Globalization;

namespace CodingTracker;

class CodingGoals
{
    public DateTime StartDate;
    public DateTime EndDate;
    public TimeSpan CodingGoal;

    public CodingGoals(string? startDate, string? endDate, string? codingGoal)
    {
        DateTime.TryParseExact(startDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, 
        DateTimeStyles.None, out StartDate);
        DateTime.TryParseExact(endDate, "yyyy/MM/dd", CultureInfo.InvariantCulture, 
        DateTimeStyles.None, out EndDate);
        TimeSpan.TryParseExact(codingGoal,"d\\.hh\\:mm",CultureInfo.InvariantCulture, out CodingGoal);
    }

    public bool GoalAchieved(string codingGoalProgress)
    {
        bool goalAchieved = false;
        TimeSpan.TryParseExact(codingGoalProgress, "d\\.hh\\:mm", 
        CultureInfo.InvariantCulture, out TimeSpan codingGoalProgressTimeSpan);        
        
        if(codingGoalProgressTimeSpan>CodingGoal)
        {
            goalAchieved = true;
        }

        return goalAchieved;
    }
    public TimeSpan RemainingGoal(string codingGoalProgress)
    {
        TimeSpan.TryParseExact(codingGoalProgress, "d\\.hh\\:mm", 
        CultureInfo.InvariantCulture, out TimeSpan codingGoalProgressTimeSpan);
        
        return CodingGoal-codingGoalProgressTimeSpan;
    }

    public TimeSpan DailyAverage(string codingGoalProgress)
    {
        TimeSpan dailyAverage = TimeSpan.Zero;
        if(!GoalAchieved(codingGoalProgress))
        {
            DateTime today = DateTime.Today;
            TimeSpan.TryParseExact(codingGoalProgress, "d\\.hh\\:mm", 
            CultureInfo.InvariantCulture, out TimeSpan codingGoalProgressTimeSpan);

            TimeSpan remainingDays = EndDate - today;
            dailyAverage = RemainingGoal(codingGoalProgress)/remainingDays.TotalDays;

        }
        return dailyAverage;
    }

    public string[] GetString()
    {
        string[] codingGoalString = new string[3];
        
        codingGoalString[0] = StartDate.Date.ToString("yyyy/MM/dd");
        codingGoalString[1] = EndDate.Date.ToString("yyyy/MM/dd");
        codingGoalString[2] = CodingGoal.ToString("d\\.hh\\:mm");
        return codingGoalString;
    }
}