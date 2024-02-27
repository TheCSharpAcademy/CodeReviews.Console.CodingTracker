using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CodingTracker.Enums;

namespace CodingTracker;

internal class UInterface
{

    public static void MainMenu()
    {
        var dataAcess = new DataAccess();
        
        bool isRunning = true;
        while (isRunning)
        {
            var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuChoices>().Title("What would you like to do?").AddChoices(MenuChoices.AddRecord, MenuChoices.ViewRecords, MenuChoices.UpdateRecord, MenuChoices.DeleteRecord, MenuChoices.Quit));

            switch (choice)
            {
                case MenuChoices.AddRecord:
                    AddRecord();
                    break;
                case MenuChoices.ViewRecords:
                    dataAcess.ResetIds();
                    dataAcess = new DataAccess();
                    var records = dataAcess.GetRecords();
                    ViewRecords(records);
                    break;
                case MenuChoices.UpdateRecord:
                    UpdateRecords();
                    break;
                case MenuChoices.DeleteRecord:
                    DeleteRecord();
                    break;
                case MenuChoices.Quit:
                    Console.Write("Farewell!");
                    isRunning = false;
                    break;
            }
        }
    }


    public static void DeleteRecord()
    {
        var dataAcess = new DataAccess();
        var records = dataAcess.GetRecords();
        ViewRecords(records);

        var id = GetNumber("Enter the id you want to delete");

        if(!AnsiConsole.Confirm("Are you sure?"))
        {
            return;
        }

        var response = dataAcess.DeleteRecord(id);

        var responseMessage = response < 1 ? $"{id} doesn' exist. Press any key to return..." : "Id deleted successfully. Press any key to return...";


        Console.WriteLine(responseMessage);
        Console.ReadKey();
    }

    public static void ViewRecords(IEnumerable<CodingRecord> records)
    {
        Console.Clear();
        
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Start Date");
        table.AddColumn("End Date");
        table.AddColumn("Duration");
    
        foreach (var record in records)
        {
            table.AddRow(record.Id.ToString(), record.DateStart.ToString(), record.DateEnd.ToString(), $"{record.Duration.TotalHours} hours {record.Duration.TotalMicroseconds%60} minutes");
        }

        AnsiConsole.Write(table);
    
    }

    public static void UpdateRecords()
    {
        var dataAcess = new DataAccess();
        var records = dataAcess.GetRecords();
        ViewRecords(records);

        var id = GetNumber("Please type the id of the habit... or type 0 to return");

        var record = records.Where(x => x.Id == id).Single();
        var dates = GetDateInputs();

        record.DateStart = dates[0];
        record.DateEnd = dates[1];

        dataAcess.UpdateRecord(record);
    }

    public static int GetNumber(string message)
    {
        string numberInput = AnsiConsole.Ask<string>(message);
        if (numberInput == "0")
        {
            MainMenu();
        }

        var output = Validation.ValidateInt(numberInput, message);
        return output;
    }

    public static void AddRecord()
    {
        CodingRecord record = new CodingRecord();

        var dateInputs = GetDateInputs();
        record.DateStart = dateInputs[0]; // isso é, a posição 0 do array retornado
        record.DateEnd = dateInputs[1];

        var dataAcess = new DataAccess();
        dataAcess.InsertRecord(record);
    }

    private static DateTime[] GetDateInputs()
    {
        var startDateInput = AnsiConsole.Ask<string>("Input Start Date With the format: dd-mm-yy hh:mm, or type 0 to return");

        if (startDateInput == "0")
        {
            MainMenu();
        }

        var startDate = Validation.ValidateStartDate(startDateInput);


        var endDateInput = AnsiConsole.Ask<string>("Input End Date with the format: dd-mm-yy hh:mm, or type 0 to return");

        if (endDateInput == "0")
        {
            MainMenu();
        }
        var endDate = Validation.ValidateEndDate(startDate, endDateInput);
        return [startDate, endDate];
    }
}
