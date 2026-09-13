using Microsoft.Data.Sqlite;
using Dapper;
using Spectre.Console;

class Program1
{
    static string connectionString = @"Data Source=habit-Tracker.db";
    
    static void Main(string[] args)
    {
        {
            CodingController.CreateTable();
            Input.GetUserInput();
            Console.ReadLine();
        }
    }

}
