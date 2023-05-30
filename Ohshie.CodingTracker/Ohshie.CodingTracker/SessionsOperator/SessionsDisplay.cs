using ConsoleTableExt;

namespace Ohshie.CodingTracker;

public class SessionsDisplay
{
    public void ShowEntries()
    {
        DbOperations dbOperations = new();
        List<Session> allSessions = dbOperations.FetchAllSessions();
        
        ConsoleTableBuilder.From(allSessions).ExportAndWriteLine();
    }
}