namespace CodingTracker;

internal class CodingTrackerController
{
    public static void Menu()
    {
        var isTrackerOn = true;
        do
        {
            Display.DisplayMenu();
            var selection = Console.ReadLine().ToLower().Trim();
            switch(selection)
            {
                case "0":
                    HandleViewRecords();
                    break;
                case "1":
                    HandleInsertRecord();
                    break;
                case "2":
                    HandleDeleteRecord();
                    break;
                case "3":
                    HandleUpdateRecord();
                    break;
                case "4":
                    HandleLiveSession();
                    break;
                case "5":
                    Display.ExitApplicationMessage();
                    isTrackerOn = false;
                    break;
                default:
                    Display.InvalidInputMessage();
                    break;
            }

        }while(isTrackerOn);
    }

    private static void HandleInsertRecord()
    {
        IEnumerable<CodingSessionModel> results;
        CodingSessionModel userSessionInput;
        string CodingDate = Validation.GetRecordDate();
        results = Database.GetRecordsByDate(CodingDate);
        if (results.Count() == 0)
        {
            userSessionInput = Validation.GetStartEndTime(CodingDate);
            Database.InsertRecord(userSessionInput);
            Display.DisplayRecordInsertedMessage();
        }
        else
        {
            Validation.ViewRecordsSpecificDate(results);
            userSessionInput = Validation.GetStartEndTimeDatePresent(CodingDate, results);
            Database.InsertRecord(userSessionInput);
            Display.DisplayRecordInsertedMessage();
        }
    }

    private static void HandleViewRecords()
    {
        IEnumerable<CodingSessionModel> results = Database.ViewSessionRecords();
        Validation.ViewRecords(results);
    }

    private static void HandleDeleteRecord()
    {
        int sessionDeleteId = Validation.GetSessionIdToDeleteRecord();
        bool IsGivenIdPresent = Database.IsGivenSessionIdPresent(sessionDeleteId);
        Database.DeleteRecord(sessionDeleteId, IsGivenIdPresent);
        Validation.DisplayAppropriateRecordMessage(IsGivenIdPresent, sessionDeleteId);
    }

    private static void HandleUpdateRecord() {
        IEnumerable<CodingSessionModel> results;
        CodingSessionModel userSessionInput;
        string CodingDate = Validation.GetRecordDate();
        results = Database.GetRecordsByDate(CodingDate);
        if (results.Count() != 0)
        {
            Validation.ViewRecordsSpecificDate(results);
            int SessionUpdateId = Validation.GetSessionIdToUpdateRecord();
            bool IsGivenIdToUpdatePresent = Database.IsGivenSessionIdPresentForInputDate(SessionUpdateId, CodingDate);
            if (IsGivenIdToUpdatePresent)
            {
                userSessionInput = Validation.GetStartEndTimeToUpdate(SessionUpdateId, CodingDate, results);
                Database.UpdateRecord(IsGivenIdToUpdatePresent, userSessionInput, SessionUpdateId);
            }
            Validation.DisplayAppropriateUpdateRecordMessage(IsGivenIdToUpdatePresent, SessionUpdateId, CodingDate);
        }
        else
            Display.NoRecordsWithDate(CodingDate);
    }

    private static void HandleLiveSession()
    {
        bool start = Validation.LiveSessionWarning();
        if (start)
        {
            List<CodingSessionModel> SessionList = Validation.StartLiveSession();
            foreach(var session in SessionList)
            {
                Database.InsertRecord(session);
            }
            Display.DisplayRecordInsertedMessage();
        }
    }
}