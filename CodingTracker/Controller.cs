namespace CodingTracker;

public static class Controller
{
    public static async Task Start()
    {
        Console.WriteLine("Hello, World!");
    }


    public static int Duration(DateTime start, DateTime end)
    {
        if (start.CompareTo(end) >= 0) throw new Exception("Start must be earlier than end");
        
        return (end - start).Seconds;
    }
    
}