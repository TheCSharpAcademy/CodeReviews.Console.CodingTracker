using System.Diagnostics;

namespace Ohshie.CodingTracker;

public class NewCodingSession
{
    private readonly string _time = DateTime.Now.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
    private readonly Stopwatch _stopwatch = new();

    public void NewSession()
    {
        string sessionDescription = AddDescriptionToSession();
        
        Session newSession = TrackSession(sessionDescription);

        Console.WriteLine("You ended your session.\n" +
                          $"Session lasted for {newSession.Length}. Nice!\n");

        newSession.Note = AddNoteToSession();

        DbOperations dbOperations = new();
        dbOperations.NewSessionEntry(newSession);
    }

    private string AddDescriptionToSession()
    {
        Console.Clear();
        Console.WriteLine("Enter some info about this session\n" +
                          "Or just press enter to skip this step");
        string? userDescription = Console.ReadLine();
        if (!string.IsNullOrEmpty(userDescription))
        {
            return userDescription;
        }

        return string.Empty;
    }

    private string AddNoteToSession()
    {
        Console.WriteLine("Any notes about this one? Type them below:");

        string? notes = Console.ReadLine();

        if (!string.IsNullOrEmpty(notes))
        {
            Console.Clear();
            return notes;
        }
        
        Console.Clear();
        return string.Empty;
    }

    private Session TrackSession(string sessionDescription)
    {
        Console.Clear();
        _stopwatch.Start();
        
        Console.WriteLine($"New session started at {_time}\n" +
                          $"------------------------------------\n" +
                          $"Press enter when you done with coding for now.");
        Session newSession = new()
        {
            Description = sessionDescription,
            Date = _time
        };
        Console.ReadLine();
        _stopwatch.Stop();

        newSession.Length = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");
        
        Console.Clear();
        return newSession;
    }
}