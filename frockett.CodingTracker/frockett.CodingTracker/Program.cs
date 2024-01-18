using Microsoft.Data.Sqlite;
using Library;
using frockett.CodingTracker.Library;

namespace frockett.CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        // selects implementation of IDbMethods
        var dbMethods = InitializeSqliteDatabase();

        //TODO stopwatchService
        var userInteractionService = new UserInputValidationService();
        var codingSessionController = new CodingSessionController(dbMethods, userInteractionService);
        //TODO reportGenerator???
        var menuHandler = new MenuHandler(codingSessionController);


        menuHandler.ShowMainMenu();
    }

    static IDbMethods InitializeSqliteDatabase()
    {
        var sqliteDbMethods = new SqliteDbMethods();
        sqliteDbMethods.InitDatabase();
        return sqliteDbMethods;
    }
}
