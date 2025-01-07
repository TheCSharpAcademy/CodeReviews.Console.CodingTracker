internal class InputDataHandler
{
    internal static (bool, DateTime, DateTime, TimeSpan) GetData()
    {
        DateTime date = SessionDate.GetDate();
        DateTime startTime = SessionStartTime.GetStartTime(date);
        DateTime endTime = DateTime.Now;
        TimeSpan duration = TimeSpan.Zero;

        var choose = DisplayInfoHelpers.GetChoiceFromSelectionPrompt(
            "Select:", [
                "Enter session end time",
                "Enter session duration"]);

        if (choose == DisplayInfoHelpers.Back)
        {
            Console.Clear();
            return (true, startTime, endTime, duration);
        }
        else if (choose == "Enter session end time")
        {
            endTime = SessionEndTime.GetEndTime(date, startTime);
            duration = endTime - startTime;
        }
        else if (choose == "Enter session duration")
        {
            duration = SessionDuration.GetDuration(startTime);
            endTime = startTime + duration;
        }
        return (false, startTime, endTime, duration);
    }
}
