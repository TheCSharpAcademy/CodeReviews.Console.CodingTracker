using System;
using System.Configuration;

namespace CodingTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Log that we are starting the process
                Console.WriteLine("Reading connection string from app settings...");

                string connectionString = ConfigurationManager.AppSettings["connectionString"];

                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string is null or empty.");
                }
                else
                {
                    Console.WriteLine($"Connection String: {connectionString}");
                }
            }
            catch (ConfigurationErrorsException e)
            {
                Console.WriteLine($"Error reading app settings: {e.Message}");
            }

            // Keep console open
            Console.ReadLine();
        }
    }
}
