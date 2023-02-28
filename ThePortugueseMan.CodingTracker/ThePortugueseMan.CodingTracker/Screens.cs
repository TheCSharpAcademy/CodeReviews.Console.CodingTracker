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

        while (!exitMenu) 
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Coding Tracker")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exitMenu = true; break;
                case 1: ViewLogs(); break;
                case 2: InsertLogs(); break;
                case 3: UpdateLog(); break;
                case 4: DeleteLog(); break;
                default: break;

            }
        }
        return;
    }
    private void ViewLogs()
    {
        Console.Clear();
        DisplaySessions(dbCmds.ReturnAll());
        askInput.AnyAndEnterToContinue();
    }

    private void DisplaySessions(List<CodingSession> listToDisplay)
    {
        var tableDataDisplay = new List<List<object>>();
        if (listToDisplay is not null)
        {
            

            foreach (CodingSession session in listToDisplay)
            {
                tableDataDisplay.Add(
                    new List<object>
                    {
                    session.Id,
                    session.StartDateTime.ToString("dd-MM-yy"), session.StartDateTime.ToString("HH:mm"),
                    session.EndDateTime.ToString("dd-MM-yy"), session.EndDateTime.ToString("HH:mm"),
                    session.Duration.ToString("hh\\:mm")
                    });
            }
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle("VIEW")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        else 
        {
            tableDataDisplay.Add(new List<object> { "", "", "", "", "", "" });
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle("EMPTY")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        return;
    }
    private void InsertLogs()
    {
        CodingSession codingSession = new();
        bool validEntry;

        do
        {
            codingSession.StartDateTime = askInput.AskForDateWithHours("Insert the start date.");
            if (codingSession.StartDateTime != DateTime.MinValue)
            {
                codingSession.EndDateTime = askInput.AskForDateWithHours("Insert the end date.");
                if (codingSession.EndDateTime == DateTime.MinValue) return;
                
                codingSession.Duration = 
                    codingSession.EndDateTime.Subtract(codingSession.StartDateTime);

                if (codingSession.Duration > TimeSpan.Zero)
                {
                    validEntry = true;
                    dbCmds.Insert(codingSession);
                }
                else
                {
                    Console.WriteLine("End date is earlier than the start date.");
                    askInput.AnyAndEnterToContinue();
                    validEntry = false;
                }
            }
            else return;
        } while (!validEntry);
        Console.WriteLine("Entry was logged successfully");
        askInput.AnyAndEnterToContinue();
        Console.Clear();
        return;
    }
    private void UpdateLog()
    {
        int index;
        bool showError = false, exit = false, validUpdate = false;
        CodingSession codingSession = new();

        while (!exit)
        {
            Console.Clear();
            DisplaySessions(dbCmds.ReturnAll());
            do
            {
                if (!showError) index = askInput.PositiveNumber("Select the index you want to update. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExists(index) && exit == false);

            if (!exit)
            {
                validUpdate = false;
                codingSession.StartDateTime = askInput.AskForDateWithHours("Insert the updated start date.");
                if (codingSession.StartDateTime != DateTime.MinValue)
                {
                    codingSession.EndDateTime = askInput.AskForDateWithHours("Insert the updated end date.");
                    if (codingSession.EndDateTime == DateTime.MinValue) break; ;

                    codingSession.Duration =
                        codingSession.EndDateTime.Subtract(codingSession.StartDateTime);

                    if (codingSession.Duration > TimeSpan.Zero)
                    {
                        validUpdate = dbCmds.Update(index, codingSession);
                    }
                    else
                    {
                        Console.WriteLine("End date is earlier than the start date.");
                        askInput.AnyAndEnterToContinue();
                    }
                }
            }
        }
        return;
    }
    private void DeleteLog()
    {
        int index;
        bool showError = false, exit = false;

        while(!exit)
        {
            Console.Clear();
            DisplaySessions(dbCmds.ReturnAll());
            do
            {
                if (!showError) index = askInput.PositiveNumber("Select the index you want to delete. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExists(index) || exit == false);

            if (!exit) 
            {
                if (dbCmds.DeleteByIndex(index)) Console.WriteLine("Log successfully deleted");
                else Console.WriteLine("Couldn't delete log...");
                askInput.AnyAndEnterToContinue();
            }
        }
        return;
    }
}
