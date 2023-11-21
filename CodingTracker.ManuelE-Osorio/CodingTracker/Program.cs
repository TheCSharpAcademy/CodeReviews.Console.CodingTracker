using System;
using System.Configuration;
using System.Collections.Specialized;

namespace CodingTracker;

internal class CodingTracker
{
    static string tableName = "CodingTracker";

    static void Main(string[] args)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["CodingTrackerConnectionString"].ToString();


        CodingSession obj1 = new("2023/11/22","01:20","2023/11/22","02:50");
        Console.WriteLine(obj1.SessionTime());
    }
}
