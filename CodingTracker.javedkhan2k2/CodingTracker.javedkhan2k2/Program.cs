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