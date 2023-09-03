using CodingTracker.library.View;

namespace CodingTracker.library.Controller;

internal static class CrudController
{
    internal static void CreateTable() => QueriesCrud.CreateTableQuery();

    internal static void InsertNewSession()
    {
        Console.Clear();

        bool getBackToMenu = Helpers.GetBackToMainMenu("insert", "insert new session");

        if(getBackToMenu)
        {
            Menu.MainMenu();
        }

        else
        {
            string startTime = Helpers.GetDateTime("start");
            string endTime = Helpers.GetDateTime("end", startTime, "endTime");
            double duration = Helpers.Duration(startTime, endTime);

            Console.WriteLine();
            QueriesCrud.InsertNewSessionQuery(startTime, endTime, duration);

            Console.Clear();
            Console.WriteLine("Session inserted successfully!\n\nPress any key to get back to main menu...");
            Console.ReadKey();
        }
    }

    internal static void InsertNewAutomaticSession(string startTime, string endTime, double duration)
    {
            Console.WriteLine();
            QueriesCrud.InsertNewSessionQuery(startTime, endTime, duration);

            Console.Clear();
            Console.WriteLine("Session inserted successfully!\n\nPress any key to get back to main menu...");
            Console.ReadKey();
        
    }

    internal static int ViewAllSessions(string operation = "")
    {
        int rows = 0;

        if (operation == "delete" || operation == "update") rows = QueriesCrud.ViewAllSessionsQuery("crud");

        else if (operation == "query") rows = QueriesCrud.ViewAllSessionsQuery("query");

        else
        {
            Console.Clear();

            bool getBackToMenu = Helpers.GetBackToMainMenu("view", "view all coding sessions");

            if (getBackToMenu)
            {
                Menu.MainMenu();
            }

            else
            {
                rows = QueriesCrud.ViewAllSessionsQuery();
            }
        }
        return rows;
    }

    internal static void DeleteSession()
    {
        Console.Clear();

        int rowCount = ViewAllSessions("update");

        if(rowCount > 0) 
        {
            int recordId = Helpers.GetSessionId("delete");
            Console.WriteLine();
            bool getBackToMenu = Helpers.GetBackToMainMenu("delete", "delete coding session");

            if (getBackToMenu)
            {
                Menu.MainMenu();
            }

            else
            {
                QueriesCrud.DeleteSessionQuery(recordId);
                Console.WriteLine("\n\nPress any key to get back to main menu...");
                Console.ReadKey();
            }
        }
    }
    
    internal static void UpdateSession() 
    {
        Console.Clear();

        int rowCount = ViewAllSessions("update");

        if (rowCount > 0)
        {
            bool getBackToMenu = Helpers.GetBackToMainMenu("update", "update coding session");

            if (getBackToMenu) Menu.MainMenu();

            else
            {
                int recordId = Helpers.GetSessionId("update");
                QueriesCrud.UpdateSessionQuery(recordId);
            }
        }
    }
}
