namespace CodingTracker.Models;

public class CodingSession
{
    public int Id { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }

    public String CalculateDuration()
    {
        try
        {
            TimeSpan timeSpan = DateTime.Parse(EndTime) - DateTime.Parse(StartTime);
            string time = "";

            if (timeSpan.Ticks < 0) return null;
            if (timeSpan.Hours > 0)
            {
                time = $"{timeSpan.Hours} Hours {timeSpan.Minutes} Minutes";

            }
            else
            {
                time = $"{timeSpan.Minutes} Minutes";
            }
            return time;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Unable to parse date");
        }
        return null;
    }

}
