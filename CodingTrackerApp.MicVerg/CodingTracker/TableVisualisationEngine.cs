using ConsoleTableExt;

namespace CodingTracker
{
    internal class TableVisualisationEngine
    {
        internal static void DrawTable(List<List<object>> tableData)
        {
            // https://github.com/minhhungit/ConsoleTableExt
            ConsoleTableBuilder
                .From(tableData)
                .ExportAndWriteLine();
        }
        internal static void MainMenu()
        {
            bool appIsRunning = true;
            while (appIsRunning)
            {
                Console.Clear();
                Console.WriteLine("\t++++++++++++++++++++");
                Console.WriteLine("\t++ Coding tracker ++");
                Console.WriteLine("\t++++++++++++++++++++");
                var optionMenu = new List<List<object>>
        {
            new List<object>{ "Press 0 to view all records." },
            new List<object>{ "Press 1 to add a new record." },
            new List<object>{ "Press 2 to delete a record." },
            new List<object>{ "Press 3 to update a record." },
            new List<object>{ "Press Q to quit the application." }
        };
                TableVisualisationEngine.DrawTable(optionMenu);

                string userInput = Console.ReadLine();

                switch (userInput.ToLower())
                {
                    case "0":
                        CrudController.ViewRecords();
                        break;
                    case "1":
                        CrudController.InsertRecord();
                        break;
                    case "2":
                        CrudController.DeleteRecord();
                        break;
                    case "3":
                        CrudController.UpdateRecord();
                        break;
                    case "q":
                        Console.WriteLine();
                        Console.WriteLine("Buh-bye!");
                        Environment.Exit(0);
                        appIsRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
        }
    }
}
