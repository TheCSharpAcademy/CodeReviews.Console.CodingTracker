using System.Configuration;

namespace SinghxRaj.CodingTracker;

internal class DatebaseManager
{
    private readonly string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString")!;

    public static void CreateTable()
    {
        // TODO: Fill this in
        throw new NotImplementedException();
    }

    public static bool AddNewCodingSession(CodingSession session)
    {
        // TODO: Fill this in
        throw new NotImplementedException();
    }

    public static List<CodingSession> GetCodingSessions()
    {
        // TODO: Fill this in
        throw new NotImplementedException();
    }
}
