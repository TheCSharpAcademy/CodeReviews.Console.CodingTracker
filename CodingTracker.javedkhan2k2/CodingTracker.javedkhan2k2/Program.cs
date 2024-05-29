using System.Configuration;
using System.Collections.Specialized;
using CodingTracker.Models;
using CodingTracker;

string? connectionString;
connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

if (connectionString == null)
{
    Console.WriteLine("ConnectionString not found.");
    System.Environment.Exit(0);
}
CodingController app = new CodingController(connectionString);
app.runApp();


// CodingTrackerDbContext dbContext = new CodingTrackerDbContext(connectionString);
// var sessions = dbContext.GetAllCodingSessions();
// foreach (var cs in sessions)
// {
//     Console.WriteLine($"Id: {cs.Id} - Start Time: {cs.StartTime} - End Time: {cs.EndTime} - Duration: {cs.Duration} ");
// }

// Console.ReadLine();
