using Microsoft.Data.Sqlite;
using Dapper;
using CodingTracker;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

string connectionString = config.GetConnectionString("DefaultConnection")!;
string defaultDate = config.GetSection("DateFormats")["DefaultDate"]!;
string defaultTime = config.GetSection("DateFormats")["DefaultTime"]!;
string tableName = config.GetSection("Table")["TableName"]!;

using (var connection = new SqliteConnection(connectionString))
{
    connection.Execute(@$"CREATE TABLE IF NOT EXISTS {tableName} (
    id INTEGER PRIMARY KEY AUTOINCREMENT,
    Date TEXT,
    StartTime TEXT,
    EndTime TEXT,
    Duration TEXT
    )");
}


bool wantToExit = false;
UserInput userInput = new UserInput(defaultDate, defaultTime);
DatabaseConnector dbConnector = new DatabaseConnector(connectionString, defaultDate, defaultTime, tableName);

while (!wantToExit)
{
    Console.WriteLine("\t\nMENU\n");
    Console.WriteLine("1. Show all entries.");
    Console.WriteLine("2. Start new session.");
    Console.WriteLine("3. Add session manually.");
    Console.WriteLine("4. Delete Session.");
    Console.WriteLine("5. Update session.");
    Console.WriteLine("6. Filter and show entries.");
    Console.WriteLine("7. Exit.\n");

    string? input = userInput.NumberInput();
    Console.WriteLine();

    try
    {
        switch (input)
        {
            case "1":
                dbConnector.ShowAllRecords();
                break;

            case "2":
                dbConnector.AddRecord();
                break;

            case "3":
                dbConnector.AddRecordManually();
                break;

            case "4":
                dbConnector.DeleteSession();
                break;

            case "5":
                dbConnector.UpdateSession();
                break;

            case "6":
                filterEntries(connectionString, defaultDate, defaultTime, tableName);
                break;

            case "7":
                wantToExit = true;
                break;
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
        continue;
    }
}

static void filterEntries(string connectionString, string defaultDate, string defaultTime, string tableName)
{
    bool wantToExit = false;

    UserInput userInput = new UserInput(defaultDate, defaultTime);
    DatabaseConnector dbConnector = new DatabaseConnector(connectionString, defaultDate, defaultTime, tableName);

    while (!wantToExit)
    {
        int filterOption = -1;

        Console.WriteLine("\t\nFILTER OPTIONS \n");
        Console.WriteLine("1. Filter by date");
        Console.WriteLine("2. Filter by year");
        Console.WriteLine("3. Filter by year and month.");
        Console.WriteLine("4. Go back to main menu.\n");

        string? input = userInput.NumberInput();
        Console.WriteLine();

        try
        {
            switch (input)
            {
                case "1":
                    filterOption = 1;
                    break;

                case "2":
                    filterOption = 2;
                    break;

                case "3":
                    filterOption = 3;
                    break;

                case "4":
                    wantToExit = true;
                    break;

                default:
                    Console.WriteLine("Invalid input.");
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            continue;
        }

        if (filterOption != -1)
        {
            dbConnector.FilterEntries(filterOption);
        }
    }
}