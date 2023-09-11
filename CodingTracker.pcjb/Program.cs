namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Coding Tracker");
        Console.WriteLine($"DatabaseFilename: {Configuration.DatabaseFilename}");
    }
}