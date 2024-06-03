using CodingTracker;

string? connectionString;
connectionString = System.Configuration.ConfigurationManager.AppSettings.Get("ConnectionString");

if (connectionString == null)
{
    Console.WriteLine("ConnectionString not found.");
    System.Environment.Exit(0);
}
CodingController app = new CodingController(connectionString);
app.RunApp();