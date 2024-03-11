namespace CodingTracker.Views;

public class SessionLogView : BaseView
{
    public SessionLog AskSessionTimes(SessionLog? toUpdateLog = null)
    {
        var errorMessage = "[red]Invalid time format. Use 24-hour format (HH:mm).[/]";
        var startTime = AskInput($"At what time did you start? [grey]({Validator.TimeFormat})[/]",
            Validator.ValidateStringAsTime, errorMessage, toUpdateLog?.StartTime.ToString(Validator.TimeFormat));
        var endTime = AskInput($"At what time did you end? [grey]({Validator.TimeFormat})[/]",
            Validator.ValidateStringAsTime, errorMessage, toUpdateLog?.EndTime.ToString(Validator.TimeFormat));

        return new SessionLog
        {
            StartTime = TimeOnly.ParseExact(startTime, Validator.TimeFormat),
            EndTime = TimeOnly.ParseExact(endTime, Validator.TimeFormat)
        };
    }
}