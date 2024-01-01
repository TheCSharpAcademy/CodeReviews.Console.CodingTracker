using System.Configuration;
using System.Collections.Specialized;
using codingTracker.Ibrahim.data;
using codingTracker.Ibrahim.Helpers;
using codingTracker.Ibrahim.UI;

namespace codingTracker.Ibrahim;

class Program
{
    static void Main(string[] args)
    {
        DatabaseManager databaseManager = new DatabaseManager();

        bool endApp = false;

        while(!endApp)
        {
            UserMenu.showMenu();

        }
        
    }
}
