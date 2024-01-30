using Library;
using frockett.CodingTracker.Library;

namespace frockett.CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        // selects implementation of IDbMethods
        var dbMethods = InitializeSqliteDatabase();

        var stopwatchService = new StopwatchService();
        var inputValidationService = new UserInputValidationService();
        var codingSessionController = new CodingSessionController(dbMethods, inputValidationService, stopwatchService);
        var displayService = new DisplayService();
        var menuHandler = new MenuHandler(codingSessionController, displayService);

        menuHandler.ShowMainMenu();
    }

    static IDbMethods InitializeSqliteDatabase()
    {
        var sqliteDbMethods = new SqliteDbMethods();
        sqliteDbMethods.InitDatabase();
        return sqliteDbMethods;
    }
}
