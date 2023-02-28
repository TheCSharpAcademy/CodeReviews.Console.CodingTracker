using ConsoleTableExt;
using System.Security.Cryptography;

namespace ThePortugueseMan.CodingTracker;

internal class Screens
{
    AskInput askInput = new();
    DbCommands dbCmds = new();

    public void MainMenu()
    {
        bool exitMenu = false;
        List<string> optionsString = new List<string> { 
            "1 - View Logs", 
            "2 - Insert Log", 
            "3 - Update Log", 
            "4 - Delete Log",
            "0 - Exit App"};

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
        return;
    }
    private void ViewLogs()
    {
        //View table
        dbCmds.ViewAll();
    }
    private void InsertLogs()
    {
        CodingSession codingSession = new();
        bool validEntry;

        do
        {
            codingSession.StartDateTime = askInput.DateWithHours("Insert the start date.");
            if (codingSession.StartDateTime != DateTime.MinValue)
            {
                codingSession.EndDateTime = askInput.DateWithHours("Insert the end date.");
                codingSession.Duration = 
                    codingSession.EndDateTime.Subtract(codingSession.StartDateTime);

                if (codingSession.Duration > TimeSpan.Zero)
                {
                    validEntry = true;
                    dbCmds.Insert(ref codingSession);
                }
                else
                {
                    Console.WriteLine("End date is earlier than the start date");
                    askInput.AnyAndEnterToContinue();
                    validEntry = false;
                }
            }
            else validEntry = false;
        } while (!validEntry);
        return;
    }
    private void UpdateLog()
    {
        //Insert new start date

        //Insert new end date
        throw new NotImplementedException();
    }
    private void DeleteLog()
    {
        //Show all and select by index
        throw new NotImplementedException();
    }
}
