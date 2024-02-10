using CodingTracker.services;

namespace CodingTracker;

class Program
{
    static void Main(string[] args)
    {
        DatabaseManager databaseManager = new();
        
        databaseManager.CreateDatabase();
    }
}