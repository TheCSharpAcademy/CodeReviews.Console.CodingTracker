using CodingTracker.matejadb.Database;
using CodingTracker.matejadb.UI;

namespace CodingTracker.matejadb;

internal class Program {
    static void Main(string[] args) {
        DatabaseManager _databaseManager = new();
        UserInterface userInterface = new();

        _databaseManager.Init();
        userInterface.MainMenu();
    }
}
