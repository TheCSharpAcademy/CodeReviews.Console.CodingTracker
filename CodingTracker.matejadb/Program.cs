using CodingTracker.matejadb.Database;
using System.Runtime.CompilerServices;
using CodingTracker.matejadb.UI;

namespace CodingTracker.matejadb;

internal class Program {
    static void Main(string[] args) {
        DatabaseManager _databaseManager = new();
        UserInterface userInterface = new();
        _databaseManager.Init();
        userInterface.MainMenu();
        //databaseManager.AddSession("2026-07-12 09:30", "2026-07-12 10:00", "30");
        //databaseManager.AddSession("2026-07-13 09:30", "2026-07-13 10:00", "30");
        //databaseManager.AddSession("2026-07-14 09:30", "2026-07-14 10:00", "30");
        //databaseManager.AddSession("2026-07-15 09:30", "2026-07-15 10:00", "30");
        //databaseManager.UpdateSession(1, "2026-07-13 09:30", "2026-07-13 10:00", "30");
        //databaseManager.DeleteSession(1)
    }
}
