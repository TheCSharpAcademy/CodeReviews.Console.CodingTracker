using CodingTracker.urasylmaz1.Controllers;
using CodingTracker.urasylmaz1.Data;
using Microsoft.Extensions.Configuration;

namespace CodingTracker.urasylmaz1
{
    class Program
    {

        static void Main(string[] args)
        {
         
            var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            string? connectionString = config.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine("Connection string missing.");
                return;
            }

            Database db = new(connectionString);

            db.Initialize();

            CodingController controller = new(connectionString);

            controller.Run();
        }
    }
}
