using CodingTracker.wkktoria.Models;
using CodingTracker.wkktoria.Services;

namespace CodingTracker.wkktoria.Controllers;

public class CodingController
{
    private readonly CodingService _codingService;

    public CodingController(CodingService codingService)
    {
        _codingService = codingService;
    }

    public void ShowAll(bool stop)
    {
        Console.Clear();

        var tableData = _codingService.ReadAll();

        if (tableData.Any())
            TableVisualisationEngine.PrintAllRecordTable(tableData);
        else
            Console.WriteLine("No records found.");

        if (!stop) return;

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void ShowBetweenTwoDates()
    {
        Console.Clear();

        var startDate = UserInput.GetDateTimeInput("start date");
        var endDate = UserInput.GetDateTimeInput("end date");

        var tableData = _codingService.ReadAllBetweenDates(Helpers.PareDateToDbFormat(startDate),
            Helpers.PareDateToDbFormat(endDate));

        if (tableData.Any())
            TableVisualisationEngine.PrintAllRecordTable(tableData);
        else
            Console.WriteLine($"No records between {startDate} and {endDate} found.");

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void ShowOneById()
    {
        Console.Clear();

        var id = UserInput.GetNumberInput("id");
        var record = _codingService.ReadById(id);

        if (record.Id != 0)
            TableVisualisationEngine.PrintOneRecordTable(record);
        else
            Console.WriteLine($"Record with id '{id}' doesn't exists.");

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void Add()
    {
        Console.Clear();

        var startTime = UserInput.GetDateTimeInput("start time");
        var endTime = UserInput.GetDateTimeInput("end time");
        var duration = Helpers.CalculateDuration(startTime, endTime);

        if (Validation.ValidateTwoDates(startTime, endTime))
        {
            var recordToAdd = new CodingSession
            {
                StartTime = Helpers.PareDateToDbFormat(startTime),
                EndTime = Helpers.PareDateToDbFormat(endTime),
                Duration = duration
            };

            _codingService.Create(recordToAdd);

            Console.WriteLine("Record has been added.");
        }
        else
        {
            Console.WriteLine("End time cannot be before start time.");
        }

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void Update()
    {
        Console.Clear();
        ShowAll(false);

        var id = UserInput.GetNumberInput("id");
        var record = _codingService.ReadById(id);

        if (record.Id != 0)
        {
            var startTime = UserInput.GetDateTimeInput("start time");
            var endTime = UserInput.GetDateTimeInput("end time");
            var duration = Helpers.CalculateDuration(startTime, endTime);

            if (Validation.ValidateTwoDates(startTime, endTime))
            {
                var updatedRecord = new CodingSession
                {
                    Id = record.Id,
                    StartTime = Helpers.PareDateToDbFormat(startTime),
                    EndTime = Helpers.PareDateToDbFormat(endTime),
                    Duration = duration
                };

                _codingService.Update(updatedRecord);

                Console.WriteLine("Record has been updated.");
            }
            else
            {
                Console.WriteLine("End time cannot be before start time.");
            }
        }
        else
        {
            Console.WriteLine($"Record with id '{id}' doesn't exists.");
        }

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void Delete()
    {
        Console.Clear();
        ShowAll(false);

        var id = UserInput.GetNumberInput("id");
        var record = _codingService.ReadById(id);

        if (record.Id != 0)
        {
            _codingService.Delete(id);

            Console.WriteLine("Record has been deleted.");
        }
        else
        {
            Console.WriteLine($"Record with id '{id}' doesn't exists.");
        }

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }

    public void TrackTime()
    {
        Console.Clear();

        var startTime = DateTime.UtcNow;

        Console.WriteLine("Tracking coding time has been started.\nPress any key to stop...");
        Console.ReadKey();

        var endTime = DateTime.UtcNow;
        var duration = Helpers.CalculateDuration(startTime, endTime);

        var record = new CodingSession
        {
            StartTime = Helpers.PareDateToDbFormat(startTime),
            EndTime = Helpers.PareDateToDbFormat(endTime),
            Duration = duration
        };

        _codingService.Create(record);
        Console.WriteLine("Record has been added.");

        Console.Write("Press any key to continue...");
        Console.ReadKey();
    }
}