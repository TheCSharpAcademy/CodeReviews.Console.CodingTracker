using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker.jkjones98
{
    internal class ReportInputs
    {
        static string connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        internal void GetReport(string dbQuery, string message )
        {
            List<string> dbQueryReturned = new();
            using(var connection = new SqliteConnection(connectionString))
            {
                using(var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = dbQuery;

                    using(var reader = tableCmd.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                dbQueryReturned.Add(reader.GetString(0));
                            }    
                        }
                        else
                        {
                            Console.WriteLine("\nNo data to return from report query");
                        }

                        Console.WriteLine(message);
                        Console.WriteLine(dbQueryReturned[0]);
                    }
                }
            }
        }
    }
}