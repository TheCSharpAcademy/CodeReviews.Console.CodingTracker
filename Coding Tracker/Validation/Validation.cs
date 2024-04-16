using System;
using System.Diagnostics;

namespace CodingTracker;

internal class Validation
{
    private static Stopwatch s_stopWatch = new Stopwatch();
    private static int s_userInputSessionId;
    private static TimeOnly s_startSessionTime, s_endSessionTime;
    private static TimeSpan s_codingSessionLimitOnDateChanged;

    public static bool TryParseTime(string input, out TimeOnly time)
    {
        time = default;

        if (DateTime.TryParseExact(input, "HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime parsedTime))
        {
            time = TimeOnly.FromDateTime(parsedTime);
            return true;
        }
        return false;
    }

    public static bool TryParseDate(string input, out DateOnly date)
    {
        date = default;

        if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
        {
            if (parsedDate <= DateTime.Now.Date) // Check if parsed date is not greater than current date
            {
                date = DateOnly.FromDateTime(parsedDate);
                return true;
            }
            else
                Display.DisplayFutureDateWarning();
        }
        return false;
    }

    public static TimeSpan CalculateTimeSpan(TimeOnly startSessionTime, TimeOnly endSessionTime)
    {
        DateTime startDateTime = DateTime.Today.Add(startSessionTime.ToTimeSpan());
        DateTime endDateTime = DateTime.Today.Add(endSessionTime.ToTimeSpan());
        TimeSpan duration = endDateTime - startDateTime;
        duration = new TimeSpan(duration.Days, duration.Hours, duration.Minutes, duration.Seconds);
        return duration;
    }

    public static bool ValidateStartSessionTime(string time)
    {
        if (TryParseTime(time, out TimeOnly Time))
        {
            s_startSessionTime = Time;
            return true;
        }
        return false;
    }

    public static bool ValidateEndSessionTime(string time)
    {
        if (TryParseTime(time, out TimeOnly Time))
        {
            if (Time > s_startSessionTime)
            {
                s_endSessionTime = Time;
                TimeSpan duration = CalculateTimeSpan(s_startSessionTime, s_endSessionTime);
                if (duration.TotalHours <= 9)
                {
                    return true;
                }
                Display.DisplayMaximumSessionTimeExceeded();
                return false;
            }
            else
            {
                Display.DisplayEndTimeWarning();
            }
        }
        return false;
    }

    public static string SessionTotalTime()
    {
        return CalculateTimeSpan(s_startSessionTime, s_endSessionTime).ToString();
    }

    public static bool ValidateInputDate(string date)
    {
        if (TryParseDate(date, out DateOnly Date))
            return true;
        return false;
    }

    public static bool ValidateInputInteger(string sessionId)
    {
        if (int.TryParse(sessionId, out s_userInputSessionId)) 
            return true;
        return false;
    }

    public static int ReturnSessionID()
    {
        return s_userInputSessionId;
    }

    public static bool IsTimeBetween(string inputTime, string startTime, string endTime)
    {
        return string.Compare(startTime, inputTime) <= 0 && string.Compare(inputTime, endTime) < 0;
    }

    public static bool IsTimeBetweenEndTime(string inputTime, string startTime, string endTime)
    {
        return string.Compare(startTime, inputTime) < 0 && string.Compare(inputTime, endTime) <= 0;
    }

    public static bool ValidateStartSessionTimeOnDatePresent(string time, IEnumerable<CodingSessionModel> records)
    {
        foreach (var session in records)
        {
            if (IsTimeBetween(time, session.SessionStartTime, session.SessionEndTime))
            {
                Display.StartTimeRejected();
                return false;
            }
        }
        return true;
    }

    public static bool ValidateStartSessionTimeOnUpdateRecord(string time, IEnumerable<CodingSessionModel> records, int sessionId)
    {
        foreach (var session in records)
        {
            if(session.SessionId != sessionId)
            {
                if (IsTimeBetween(time, session.SessionStartTime, session.SessionEndTime))
                {
                    Display.StartTimeRejected();
                    return false;
                }
            }
        }
        return true;
    }

    public static bool ValidateEndSessionTimeOnDatePresent(string endTime, IEnumerable<CodingSessionModel> records, string startTime)
    {
        if (string.Compare(startTime, endTime) >= 0)
        {
            Display.DisplayEndTimeWarning();
            return false;
        }
        foreach (var record in records)
        {
            if (
                IsTimeBetweenEndTime(endTime, record.SessionStartTime, record.SessionEndTime))
            {
                Display.EndTimeRejected();
                return false;
            }
        }
        return true;
    }

    public static bool ValidateEndSessionTimeOnUpdateRecord(string endTime, IEnumerable<CodingSessionModel> records, string startTime, int sessionId)
    {
        if (string.Compare(startTime, endTime) >= 0)
        {
            Display.DisplayEndTimeWarning();
            return false;
        }
        foreach (var record in records)
        {
            if (record.SessionId != sessionId)
            {
                if (IsTimeBetweenEndTime(endTime, record.SessionStartTime, record.SessionEndTime))
                {
                    Display.EndTimeRejected();
                    return false;
                }
            }
        }
        return true;
    }

    public static void ControlStopwatch(string action)
    {
        switch (action.ToLower().Trim())
        {
            case "start":
                s_stopWatch.Start();
                break;
            case "stop":
                if (s_stopWatch.IsRunning)
                {
                    s_stopWatch.Stop();
                }
                s_stopWatch.Reset();
                break;
            default:
                break;
        }
    }

    public static string GetCurrentDate()
    {
        DateOnly currentDate = DateOnly.FromDateTime(DateTime.Now);
        string formattedDate = currentDate.ToString("yyyy-MM-dd");
        return formattedDate;
    }

    public static string GetCurrentTime()
    {
        TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
        s_startSessionTime = currentTime;
        return currentTime.ToString("HH:mm");
    }

    public static TimeOnly GetCurrentTimeTimeOnly()
    {
        TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
        s_endSessionTime = currentTime;
        return currentTime;
    }

    internal static bool QuitLiveSessionPressed()
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.Q)
            {
                return true; // 'q' pressed
            }
        }
        return false;
    }

    internal static bool CheckSessionLimitCompletion()
    {
        TimeSpan duration = CalculateTimeSpan(s_startSessionTime, GetCurrentTimeTimeOnly());
        if (duration.TotalHours >= 9)
        {
            s_endSessionTime = s_startSessionTime.AddHours(9);
            return true;
        }
        return false;
    }

    internal static bool CheckNewSessionLimitCompletion()
    {
        TimeSpan duration = CalculateTimeSpan(s_startSessionTime, GetCurrentTimeTimeOnly());
        if (duration <= s_codingSessionLimitOnDateChanged)
        {
            return true;
        }
        return false;
    }

    internal static string GetEndTime()
    {
        return s_endSessionTime.ToString();
    }

    internal static void ViewRecords(IEnumerable<CodingSessionModel> records)
    {
        if (records.Count() == 0)
        {
            Display.NoRecordsFoundConsoleMessage();
            Console.ReadLine();
            return;
        }
        foreach (var session in records)
        {
            Display.DisplayIndividualRecord(session);
        }
        Display.DisplayPressKeyToContinue();
        Console.ReadLine();
    }

    internal static string GetRecordDate()
    {
        string timeDateInput;
        do
        {
            Display.GetCodingSessionDateConsoleMessage();
            timeDateInput = Console.ReadLine();
        } while (!ValidateInputDate(timeDateInput));
        return timeDateInput;
    }

    internal static int GetSessionIdToDeleteRecord()
    {
        Display.DisplayGetSessionId();
        var input = Console.ReadLine();
        bool intVal = ValidateInputInteger(input);
        while (!intVal)
        {
            Display.DisplayInvalidInput();
            input = Console.ReadLine();
            intVal = ValidateInputInteger(input);
        }
        return int.Parse(input.ToString());
    }

    internal static int GetSessionIdToUpdateRecord()
    {
        Display.DisplayUpdateGetSessionId();
        var input = Console.ReadLine();
        bool intVal = ValidateInputInteger(input);
        while (!intVal)
        {
            Display.DisplayInvalidInput();
            input = Console.ReadLine();
            intVal = ValidateInputInteger(input);
        }
        int SessionId = ReturnSessionID();
        return SessionId;

    }

    internal static CodingSessionModel GetStartEndTime(string codingDate)
    {
        string timeDateInput;
        CodingSessionModel model = new CodingSessionModel();
        model.SessionCodingDate = codingDate;
        do
        {
            Display.GetCodingSessionStartTimeConsoleMessage();
            timeDateInput = Console.ReadLine();
        } while (!ValidateStartSessionTime(timeDateInput));
        model.SessionStartTime = timeDateInput;
        do
        {
            Display.GetCodingSessionEndTimeConsoleMessage();
            timeDateInput = Console.ReadLine();
        } while (!ValidateEndSessionTime(timeDateInput));
        model.SessionEndTime = timeDateInput;
        model.SessionDuration = SessionTotalTime();
        return model;
    }

    internal static CodingSessionModel GetStartEndTimeDatePresent(string codingDate, IEnumerable<CodingSessionModel> records)
    {
        string timeDateInput;
        CodingSessionModel model = new CodingSessionModel();
        model.SessionCodingDate = codingDate;
        do
        {
            Display.GetCodingSessionStartTimeConsoleMessage();
            timeDateInput = Console.ReadLine();
        } while (!ValidateStartSessionTime(timeDateInput) || !ValidateStartSessionTimeOnDatePresent(timeDateInput, records));
        model.SessionStartTime = timeDateInput;

        do
        {
            Display.GetCodingSessionEndTimeConsoleMessage();
            timeDateInput = Console.ReadLine();
        } while (!ValidateEndSessionTime(timeDateInput) || !ValidateEndSessionTimeOnDatePresent(timeDateInput, records, model.SessionStartTime));
        model.SessionEndTime = timeDateInput;
        model.SessionDuration = SessionTotalTime();
        return model;
    }

    internal static CodingSessionModel GetStartEndTimeToUpdate(int userInputSessionId, string CodingDate, IEnumerable<CodingSessionModel> records)
    {
        string timeDateInput;
        CodingSessionModel model = new CodingSessionModel();
        model.SessionCodingDate = CodingDate;
        do
        {
            Display.GetCodingSessionStartTimeConsoleMessage();
            timeDateInput = Console.ReadLine();
        }while (!ValidateStartSessionTime(timeDateInput) || !ValidateStartSessionTimeOnUpdateRecord(timeDateInput, records, userInputSessionId));
        model.SessionStartTime = timeDateInput;
        do
        {
            Display.GetCodingSessionEndTimeConsoleMessage();
            timeDateInput = Console.ReadLine();
        }while (!ValidateEndSessionTime(timeDateInput) || !ValidateEndSessionTimeOnUpdateRecord(timeDateInput, records, model.SessionStartTime, userInputSessionId));
        model.SessionEndTime = timeDateInput;
        model.SessionDuration = SessionTotalTime();
        return model;
    }

    internal static void ViewRecordsSpecificDate(IEnumerable<CodingSessionModel> records)
    {
        Display.DisplayRecordAlreadyPresent();
        foreach (var session in records)
        {
            Display.DisplayIndividualRecord(session);
        }
        Display.DisplayPressKeyToContinue();
        Console.ReadLine();
    }

    internal static bool LiveSessionWarning()
    {
        Display.DisplayLiveSessionWarning();
        var input = Console.ReadLine().ToLower().Trim();
        if (input == "y")
            return true;
        return false;
    }

    internal static List<CodingSessionModel> StartLiveSession()
    {
        List<CodingSessionModel> sessionList = new List<CodingSessionModel>();
        CodingSessionModel model = new CodingSessionModel();
        model.SessionCodingDate = GetCurrentDate();
        model.SessionStartTime = GetCurrentTime();
        ControlStopwatch("start");
        Display.DisplayLiveSessionStarted();
        bool SessionLive = true;
        bool DateChanged = false;
        while (SessionLive)
        {
            SessionLive = !QuitLiveSessionPressed() && !CheckSessionLimitCompletion();
            if (model.SessionCodingDate != GetCurrentDate())
            {
                DateChanged = true;
                //Save the previous session in the list
                model.SessionEndTime = "23:59";
                ValidateStartSessionTime(model.SessionStartTime);
                ValidateEndSessionTime(model.SessionEndTime);
                model.SessionDuration = SessionTotalTime();
                sessionList.Add(model);
                //Start a new session with limit = 9hrs- duration of prev session
                s_codingSessionLimitOnDateChanged = TimeSpan.FromHours(9) - CalculateTimeSpan(s_startSessionTime, s_endSessionTime);
                model.SessionStartTime = "00:00";
                ValidateStartSessionTime(model.SessionStartTime);
                break;
            }
        }
        if (DateChanged)
        {
            while (SessionLive)
            {
                SessionLive = !QuitLiveSessionPressed() && !CheckNewSessionLimitCompletion();
            }
            model.SessionCodingDate = GetCurrentDate();
        }
        model.SessionEndTime = GetEndTime();
        model.SessionDuration = SessionTotalTime();
        ControlStopwatch("stop");
        Display.DisplayLiveSessionStopped();
        model.SessionDuration = SessionTotalTime();
        sessionList.Add(model);
        return sessionList;
    }

    internal static void DisplayAppropriateRecordMessage(bool iSGivenSessionIdPresent, int userInputSessionId)
    {
        if (!iSGivenSessionIdPresent)
        {
            Display.UserInputSessionIdNotPresentToDelete(userInputSessionId);
            Console.ReadLine();
        }
        else
        {
            Display.RecordDeletedSuccessMessage(userInputSessionId);
            Console.ReadLine();
        }
    }

    internal static void DisplayAppropriateUpdateRecordMessage(bool iSGivenSessionIdPresent, int userInputSessionId, string userInputSessionDate)
    {
        if (!iSGivenSessionIdPresent)
        {
            Display.UserInputSessionIdNotPresent(userInputSessionId, userInputSessionDate);
            Console.ReadLine();
        }
        else
        {   
            Display.UserInputSessionIdUpdatedSuccessMessage(userInputSessionId);
            Console.ReadLine();
        }
    }
}