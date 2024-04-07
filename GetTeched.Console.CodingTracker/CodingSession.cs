namespace coding_tracker;

public class CodingSession
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Duration { get; set; }
}

public class CurrentCodingGoal
{
    public int Id { get; set; }
    public string Date { get; set; }
    public int Hours { get; set; }
    public int Completed { get; set; }
    public string Datestamp { get; set; }
}

public class CodingGoal
{
    public int Id { get; set; }
    public string Date { get; set; }
    public int Hours { get; set; }
    public int Completed { get; set; }
}
