namespace CodingTrackerJMS;
//probably delete this file; its not currently being used
public class Control
{
    DatabaseLogic databaseLogic;
    public string MainMenuInput { get; set; }
    public int Id { get; set; }
    public string StartDate { get; set; }
    public string StartTime { get; set; }
    public string EndDate { get; set; }
    public string EndTime { get; set; }
    public int TotalTime { get; set; }

    public void MenuProcess(string userInput)
    {
        MainMenuInput = userInput;
    }
    void IDProcess(int id)
    {
        Id = id;
    }
    public void DataProcess(string startDate, string startTime, string endDate, string endTime, int totalTime)
    {
        StartDate = startDate;
        StartTime = startTime;
        EndDate = endDate;
        EndTime = endTime;
        TotalTime = totalTime;
        Console.WriteLine(MainMenuInput);
    }


}
