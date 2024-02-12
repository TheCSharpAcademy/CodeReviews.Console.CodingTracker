namespace CodingTracker.Mateusz_Platek;

public class Session
{
    public int id { get; set; }
    public string name { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }

    public Session(int id, string name, DateTime start, DateTime end)
    {
        this.id = id;
        this.name = name;
        this.start = start;
        this.end = end;
    }
    
    public Session(string name, DateTime start, DateTime end)
    {
        this.id = 0;
        this.name = name;
        this.start = start;
        this.end = end;
    }

    public string GetStart()
    {
        string day = start.Day.ToString("00");
        string month = start.Month.ToString("00");
        string year = start.Year.ToString("0000");
        string hour = start.Hour.ToString("00");
        string minute = start.Minute.ToString("00");
        string second = start.Second.ToString("00");
        return $"{day}-{month}-{year} {hour}:{minute}:{second}";
    }
    
    public string GetEnd()
    {
        string day = end.Day.ToString("00");
        string month = end.Month.ToString("00");
        string year = end.Year.ToString("0000");
        string hour = end.Hour.ToString("00");
        string minute = end.Minute.ToString("00");
        string second = end.Second.ToString("00");
        return $"{day}-{month}-{year} {hour}:{minute}:{second}";
    }

    public TimeSpan GetDuration()
    {
        return end - start;
    }

    public override string ToString()
    {
        return $"Id: {id}\nName: {name}\nStart: {start}\nEnd: {end}\nDuration: {end - start}";
    }
}