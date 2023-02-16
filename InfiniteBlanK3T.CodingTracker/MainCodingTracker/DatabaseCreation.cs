using Microsoft.Data.Sqlite;
using System.Configuration;

namespace CodingTracker;

public class DatabaseCreation
{        
    private string _name;
    string connectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
    
    public DatabaseCreation(string name) 
    {
        _name = name;                                             
        CheckMainTableExist();            
    }

    public DatabaseCreation() : this("Thomas_Default") { }

    public string Name
    {
        set { _name = value; }
        get { return _name; }
    }

    public void CheckMainTableExist()
    {
        using var connection = new SqliteConnection(connectionString);
        
        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
                @$"CREATE TABLE IF NOT EXISTS {Name} (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Date TEXT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration INTEGER
                    );";
        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);
        }
    }

    public void CheckGoalTableExist()
    {
        using var connection = new SqliteConnection(connectionString);

        connection.Open();
        var tableCmd = connection.CreateCommand();
        tableCmd.CommandText =
            $@"CREATE TABLE IF NOT EXISTS Goals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT,
                Date TEXT,
                TimePerDay Interger,
                Goal Interger
                );";
        try
        {
            tableCmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Oh no! An error occured.\n - Details: " + ex.Message);
        }
    }

    public string CreateNewRecord()
    {
        Console.Clear();
        Console.Write("Your name for the record: ");

        string? newRecordName = Console.ReadLine();
        newRecordName = newRecordName.Replace(" ", "_");

        while (newRecordName == null || newRecordName == "")
        {
            Console.Write("Invalid name please try again: ");
            newRecordName = Console.ReadLine();
        }

        DatabaseCreation newrecord = new(newRecordName);
        Console.WriteLine($"New record <<{newrecord.Name}>> created!");
        Thread.Sleep(1000);

        return newRecordName;
    }
}
