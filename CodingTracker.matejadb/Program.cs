using CodingTracker.matejadb.Database;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.matejadb;

internal class Program {

    static void Main(string[] args) {
        DatabaseManager databaseManager = new();
        databaseManager.Init();
        //databaseManager.AddSession("2026-07-12 09:30", "2026-07-12 10:00", "30");
        //databaseManager.AddSession("2026-07-13 09:30", "2026-07-13 10:00", "30");
        //databaseManager.AddSession("2026-07-14 09:30", "2026-07-14 10:00", "30");
        //databaseManager.AddSession("2026-07-15 09:30", "2026-07-15 10:00", "30");
        //databaseManager.UpdateSession(1, "2026-07-13 09:30", "2026-07-13 10:00", "30");
        //databaseManager.DeleteSession(1);
        var sessions = databaseManager.ViewSessions();
        foreach(var session in sessions) {
            session.DisplayDetails();
        }
    }
}
