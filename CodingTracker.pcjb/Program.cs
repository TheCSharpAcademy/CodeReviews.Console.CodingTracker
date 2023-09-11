using System.ComponentModel.DataAnnotations.Schema;

namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {   
        var database = new Database(Configuration.DatabaseFilename);
        database.CreateDatabaseIfNotPresent();
        
        Console.WriteLine("Coding Tracker");
        Console.WriteLine($"DatabaseFilename: {Configuration.DatabaseFilename}");
    }
}