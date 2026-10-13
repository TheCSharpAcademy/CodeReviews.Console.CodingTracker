using Microsoft.Extensions.Configuration;

namespace CodingTracker;

class Program
{
  static void Main(string[] args)
  {
    var configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();

    var connectionString = configuration.GetConnectionString("CodingTracker")
        ?? throw new InvalidOperationException(
            "Connection string 'CodingTracker' is missing.");

    Database.Configure(connectionString);
    Database.CreateDB();

    Displayer.PrintTitle();
    UserInterface.MainMenu();
  }
}
