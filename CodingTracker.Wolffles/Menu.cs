using CodingTracker.Wolffles;

namespace CodingTracker.Wolffles;
public class Menu
{
    enum Options
    {
        closeApp,
        viewRecord,
        startSession,
        deleteRecord,
        updateRecord,
        deleteAllRecord,
        maxMenuOptions
    }
    public SQLiteIO sqlDatabase;
    public SessionTableDisplay display = new SessionTableDisplay();
    public List<ISession> list;
    public Menu(SQLiteIO ioDatabase)
    {
        sqlDatabase = ioDatabase;
        list = sqlDatabase.Read();
    }
    public bool MainMenu()
    {
        Console.Clear();

        Console.WriteLine(@"Welcome to your habit tracker. Please enter a number to select an option below:
        0. Close Application
        1. View All Records
        2. Start Session
        3. Delete Record
        4. Update Record");

        int maxMenuOptions = (int)Options.maxMenuOptions;
        int selectedOption;

        do
        {
            selectedOption = InputValidation.GetUserInputAsInt();
        }
        while (selectedOption > maxMenuOptions && selectedOption < 0);

        switch ((Options)selectedOption)
        {
            case Options.closeApp:
                return false;
            case Options.viewRecord:
                ViewRecord();
                break;
            case Options.startSession:
                StartSession();
                break;
            case Options.deleteRecord:
                DeleteRecord();
                break;
            case Options.updateRecord:
                UpdateRecord();
                break;

        }
        return true;
    }
    private void ViewRecord()
    {
        Console.Clear();

        list = sqlDatabase.Read();
        display.DisplayTable(list);

        Console.WriteLine("Press any key to exit.");
        Console.ReadKey();

        this.MainMenu();
    }

    private void StartSession()
    {
        Timer timer = new Timer();
        ISession session = timer.TimedSession();

        sqlDatabase.Insert(session);



        Console.Clear();
        list = sqlDatabase.Read();
        display.DisplayTable(list);

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
        this.MainMenu();

    }
    private void UpdateRecord()
    {
        Console.Clear();

        list = sqlDatabase.Read();
        display.DisplayTable(list);
        Console.WriteLine(@"Select a record to UPDATE by typing the record DATE to select, then the DATE to replace with.
------------------------------------------------------------------------");

        sqlDatabase.Update(InputValidation.GetUserInputAsDate(), InputValidation.GetUserInputAsDate());

        Console.WriteLine("Record Updated. Press any key to continue.");

        this.MainMenu();

        Console.ReadKey();

    }
    private void DeleteRecord()
    {
        Console.Clear();
        list = sqlDatabase.Read();
        display.DisplayTable(list);
        Console.WriteLine(@"Select a record to delete by typing it's DATE.
------------------------------------------------------------------------");


        sqlDatabase.Delete(InputValidation.GetUserInputAsDate());
        this.MainMenu();
    }

}