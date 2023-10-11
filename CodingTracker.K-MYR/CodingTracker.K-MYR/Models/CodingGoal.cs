namespace CodingTracker.K_MYR.Models;

internal class CodingGoal
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime Deadline { get; set; }

    public TimeSpan Goal { get; set; }

    public TimeSpan ElapsedTime { get; set; }

    public string ElapsedPercentage
    {
        get
        {
            if (ElapsedTime < Goal)
            {
                double percentage = Math.Round(ElapsedTime / Goal, 4);
                return string.Format("{0:P2}", percentage);
            }
            else
                return "100%";
        }
    }

    public TimeSpan AverageHoursToGoal
    {
        get
        {
            double days = (Deadline - DateTime.Now).TotalDays;

            if (days > 0 && Goal > ElapsedTime)
            {
                TimeSpan averageHours = ((Goal - ElapsedTime) / days);
                return TimeSpan.FromSeconds(Math.Round(averageHours.TotalSeconds));
            }
            else
            {
                return new TimeSpan(0, 0, 0);
            }
        }
    }
}
