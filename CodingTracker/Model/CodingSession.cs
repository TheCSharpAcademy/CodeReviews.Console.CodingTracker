namespace CodingTracker;

class CodingSession
{
  internal int Id { get; private set; }
  internal string Activity { get; private set; }
  internal string StartTime { get; private set; }
  internal string EndTime { get; private set; }
  internal int Duration { get; private set; }

  internal CodingSession(Int64 id, string activity, string startTime, string endTime, Int64 duration)
  {
    Id = (int)id;
    Activity = activity;
    StartTime = startTime;
    EndTime = endTime;
    Duration = (int)duration;
  }

  internal void EditStartTime(string startTime)
  {
    StartTime = startTime;
  }

  internal void EditEndTime(string endTime)
  {
    EndTime = endTime;
  }

  internal void EditDuration(int duration)
  {
    Duration = duration;
  }

  internal void EditActivity(string activity)
  {
    Activity = activity;
  }

  internal string DisplayData()
  {
    string activity = Activity;
    string startTime = TimeFormatter.ConvertDateOutDb(StartTime);
    string endTime = TimeFormatter.ConvertHourOutDb(EndTime);
    string duration = TimeFormatter.IntToHHmm(Duration);

    string result = activity.PadRight(20, ' ') + startTime.PadRight(25, ' ') + endTime.PadRight(10, ' ') + duration;
    return result;
  }
}