using Spectre.Console;

namespace Lonchanick.CodingTracker;

internal class Controller
{
    internal static void CreateNewRecord()
    {
        CodingSession newSession = new();

        newSession.DateTimeSessionInit = DateTime.Now;

        Console.Write(">> New Coding Session has been initialized, Press ENTER When finished.. ");
        Console.ReadLine();

        newSession.DateTimeSessionEnd = DateTime.Now;

        newSession.Duration = TimeSpanCalculator(newSession.DateTimeSessionInit, newSession.DateTimeSessionEnd);

        PrintTimeSpan(newSession.Duration);

        bool response = Repository.Insert(newSession);

        if(response)
            Console.WriteLine("Done!");
        else
            Console.WriteLine("Something Went wrong!");

        Console.ReadLine();

    }
    internal static void ReadAllRecords()
    {
        var sessionsRecords = Repository.Select();

        if(sessionsRecords is not null)
            PrettyTable(sessionsRecords); 
        else
            Console.WriteLine("There is no records yet");
    }
    internal static void UpdateRecord()
    {
        int id = GetValidInteger("Type id of the record to update");
        DateTime init = GetValidDateTime("Init dateTime, use the right format (yyy-MM-dddd HH:DD:SS) ");
        DateTime end = GetValidDateTime("End dateTime, use the right format (yyy-MM-dddd HH:DD:SS) ");
        TimeSpan duration = TimeSpanCalculator(init, end);

        bool response = Repository.Update(new CodingSession 
        {
            Id=id,
            DateTimeSessionInit=init,
            DateTimeSessionEnd=end,
            Duration=duration
        });

        if(response)
            Console.WriteLine("Done!");
        else
            Console.WriteLine("Something went wrong!");

    }
    internal static void DeleteRecord()
    {
        int id = GetValidInteger("Type id of the record to delete");

        bool response = Repository.Delete(id);

        if (response)
            Console.WriteLine("Done!");
        else
            Console.WriteLine("Something went wrong!");
    }

    public static TimeSpan TimeSpanCalculator(DateTime init, DateTime final) => (final - init);

    public static void PrintTimeSpan(TimeSpan timeSpan)
    {
        var panel = new Panel
        ($"Hours: {timeSpan.Hours}\nMinutes: {timeSpan.Minutes}\nSeconds: {timeSpan.Seconds}")
        {
            Header = new PanelHeader("Total Span"),
            Padding = new Padding(1, 1, 1, 1)
        };

        AnsiConsole.Write(panel);
    }

    public static void PrettyTable(List<CodingSession> sessions)
    {
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Init");
        table.AddColumn("End");
        table.AddColumn("Duration");

        foreach (var s in sessions)
        {
            TimeSpan aux = s.Duration;
            table.AddRow
                (s.Id.ToString(),
                s.DateTimeSessionInit.ToString("yyyy-MM-dddd"),
                s.DateTimeSessionEnd.ToString("yyyy-MM-dddd"),
                $"Hours: {aux.Hours} - Minutes: {aux.Minutes} - Seconds: {aux.Seconds}");
        }

        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    static int GetValidInteger(string Lable)
    {
        string userInput = string.Empty;
        int result;

        do
        {
            Console.Write($"{Lable}: ");
            userInput = Console.ReadLine();

        } while (!int.TryParse(userInput, out result));

        return result;
    }

    static DateTime GetValidDateTime(string param)
    {
        string userInput = string.Empty;
        DateTime result;

        do
        {
            Console.Write($"{param}: ");
            userInput = Console.ReadLine();
        } while (!DateTime.TryParse(userInput, out result));

        return result;
    }
}


