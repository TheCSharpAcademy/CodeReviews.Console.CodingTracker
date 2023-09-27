using System;
using System.Configuration;

namespace coding_tracker
{
    class Program
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        static void Main(string[] args)
        {

        }
    }
}
