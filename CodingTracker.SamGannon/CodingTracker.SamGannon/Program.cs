using System.Data.Common;
using System.Configuration;

namespace CodingTracker.SamGannon
{
    internal class Program
    {
        
        static string connectionString = ConfigurationManager.ConnectionStrings["DataConnection"].ConnectionString;

        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new();
            databaseManager.CreateTable(connectionString);

            UserInput u
        }
    }
}