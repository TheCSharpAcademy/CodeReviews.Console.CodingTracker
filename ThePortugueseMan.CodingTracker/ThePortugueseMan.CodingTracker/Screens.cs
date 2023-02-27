using ConsoleTableExt;

namespace ThePortugueseMan.CodingTracker;

internal class Screens
{
    AskInput askInput = new();
    public void MainMenu()
    {
        bool exitMenu = false;
        List<string> optionsString = new List<string> { 
            "1 - View Logs", 
            "2 - Insert Log", 
            "3 - Update Log", 
            "4 - Delete Log" };

        ConsoleTableBuilder.From(optionsString)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .WithColumn("Coding Tracker")
            .ExportAndWriteLine();
        Console.Write("\n");

        while (!exitMenu) 
        {
            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exitMenu = true; break;
                case 1: ViewLogs(); break;
                case 2: InsertLogs(); break;
                case 3: DeleteLog(); break;
                case 4: UpdateLog(); break;
                default: break;

            }
        }
    }
    private void ViewLogs()
    {
        throw new NotImplementedException();
    }
    private void InsertLogs()
    {
        throw new NotImplementedException();
    }
    private void UpdateLog()
    {
        throw new NotImplementedException();
    }
    private void DeleteLog()
    {
        throw new NotImplementedException();
    }
}
