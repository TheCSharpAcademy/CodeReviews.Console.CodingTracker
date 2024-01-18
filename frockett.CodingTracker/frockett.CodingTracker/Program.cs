using Microsoft.Data.Sqlite;
using Library;

namespace frockett.CodingTracker;

internal class Program
{
    static void Main(string[] args)
    {
        // selects implementation of IDbMethods

        //var dbMethods = new SqliteDbMethods();
        var dbMethods = InitializeSqliteDatabase();
        
        //TODO stopwatchService
        //TODO userInputService
        //TODO codingSessionController
        //TODO reportGenerator???
        var menuHandler = new MenuHandler();


        menuHandler.ShowMainMenu();
    }

    static IDbMethods InitializeSqliteDatabase()
    {
        var sqliteDbMethods = new SqliteDbMethods();
        sqliteDbMethods.InitDatabase();
        return sqliteDbMethods;
    }
}
