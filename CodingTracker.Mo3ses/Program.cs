using CodingTracker.Mo3ses.Data;
using CodingTracker.Mo3ses.Models;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        CodingSession codingSession = new();

        string test = "2023-09-28 23:50:55";
        codingSession.StartTime = DateTime.Now;
        codingSession.EndTime = DateTime.Parse(test);

        DbConnect dbConnect = new();

        dbConnect.Create(codingSession);

        dbConnect.GetAll();

        test = "2023-10-28 13:50:55";
        codingSession.Id = 3;
        codingSession.StartTime = DateTime.Now;
        codingSession.EndTime = DateTime.Parse(test);
        dbConnect.Update(codingSession);
      
       dbConnect.Delete(3);
       dbConnect.GetAll();
    }
}