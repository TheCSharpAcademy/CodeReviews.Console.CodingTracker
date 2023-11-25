using System.Diagnostics;
using System.Globalization;

class CodingSession
{
    public DateTime StartDateTime;
    public DateTime EndDateTime;

    public TimeSpan ElapsedTime;

    public int ID;

    public CodingSession(string id, string startDate, string startTime, string endDate, string endTime)
    {
        DateTime.TryParseExact(startDate+" "+startTime, "yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture,
        DateTimeStyles.None,out StartDateTime);
        DateTime.TryParseExact(endDate+" "+endTime, "yyyy/MM/dd HH:mm",CultureInfo.InvariantCulture,
        DateTimeStyles.None,out DateTime TempDateTime);
        EndDateTime = TempDateTime;
        ID = Convert.ToInt32(id);
        ElapsedTime = EndDateTime - StartDateTime;
    }

    public CodingSession(int id, DateTime startDateTime, DateTime endDateTime, TimeSpan elapsedTime)
    {
        StartDateTime = startDateTime;
        EndDateTime = endDateTime;
        ID = id;
        ElapsedTime = elapsedTime;
    }
}