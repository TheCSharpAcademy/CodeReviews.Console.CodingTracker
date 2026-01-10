using Spectre.Console;

internal class CodingSession
{
    public int Id { get; set; }
    public string RecordName { get; set; }
    public string Date { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string Duration { get; set; }

    public CodingSession(string name, string date, string start, string end)
    {
        this.Id = 0;
        this.RecordName = name;
        this.Date = date;
        this.StartTime = start;
        this.EndTime = end;
        this.Duration = CalculateDuration();
    }

    public CodingSession(string record)
    {
        var recordDetails = record.Split(' ');

        try
        {
            this.Id = Int32.Parse(recordDetails[0]);
            this.RecordName = recordDetails[1];
            this.Date = recordDetails[2];
            this.StartTime = recordDetails[3];
            this.EndTime = recordDetails[4];
            this.Duration = recordDetails[5];
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine("Failed! record may be corrupted or in the wrong format");
            AnsiConsole.Ask<string>("Press enter to return");
        }
    }

    public void ChangeTime(string start, string end)
    {
        this.Duration = CalculateDuration(start, end);
    }

    private string CalculateDuration(string start, string end)
    {
        var startTime = DateTime.ParseExact(
            start,
            Globals.TIME_FORMAT,
            Globals.CULTURE_INFO);

        var endTime = DateTime.ParseExact(
            end,
            Globals.TIME_FORMAT,
            Globals.CULTURE_INFO);

        var duration = endTime.Subtract(startTime);

        return duration.ToString();
    }

    private string CalculateDuration()
    {
        var startTime = DateTime.ParseExact(
            this.StartTime,
            Globals.TIME_FORMAT,
            Globals.CULTURE_INFO);

        var endTime = DateTime.ParseExact(
            this.EndTime,
            Globals.TIME_FORMAT,
            Globals.CULTURE_INFO);

        var duration = endTime.Subtract(startTime);

        return duration.ToString();
    }
}