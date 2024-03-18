using CodingTracker.enums;
using static CodingTracker.utils.Utilities;
using static CodingTracker.utils.Validation;

using CodingTracker.utils;
using CodingTracker.views;
using Spectre.Console;

namespace CodingTracker.services;

/// <summary>
/// This handles the user's input for reporting. Methods are invoked with
/// reflection, based on the method name passed as custom attribute above corresponding enum entry.
/// </summary>
internal class ReportService
{
    private readonly DatabaseService _databaseService;
    private Table _report;
    private Table _reportForSaving;
    private string _formattedDuration;
    
    internal ReportService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        _formattedDuration = string.Empty;
        _report = new Table();
        _reportForSaving = new Table();
    }
    
    internal void HandleUserChoice(Enum userChoice)
    {
        HandleUserAction(() =>
            {
                if (ProcessUserChoice(userChoice))
                {
                    DisplayAndSaveReport(_report);
                }
                ContinueMessage();
            });
    }
    
    private void HandleUserAction(Action userAction)
    {
        try
        {
            userAction();
        }
        catch (ReturnBackException)
        {
            // Do nothing
        }
        catch (InvalidOperationException)
        {
            // Do nothing
        }
    }

    private bool ProcessUserChoice(Enum userChoice)
    {
        switch (userChoice)
        {
            case ReportTypes.DateToToday:
                return CreateReport(singleDate: true);
            case ReportTypes.DateRange:
                return CreateReport(singleDate:false);
            case ReportTypes.TotalForMonth:
                return TotalsForMonthAndYear(ReportTypes.TotalForMonth);
            case ReportTypes.TotalForYear:
                return TotalsForMonthAndYear(ReportTypes.TotalForYear);
            case ReportTypes.Total:
                return Total();
            case ReportTypes.BackToMainMenu:
                return false;
            default:
                AnsiConsole.WriteLine("Invalid choice.");
                break;
        }

        return false;
    }

    private bool TotalsForMonthAndYear(ReportTypes userChoice)
    {
        var month = userChoice == ReportTypes.TotalForMonth
            ? (int)ValidateNumber("Enter the month number (1-12).", 12)
            : 01;
        
        var year = GetCorrectYear();
        
        const int startDateDay = 1;
        var endDateDay = userChoice == ReportTypes.TotalForMonth
            ? DateTime.DaysInMonth(year, month)
            : DateTime.DaysInMonth(year, 12);

        return CreateReport(
            singleDate: false,
            startDate: new DateTime(year: year, month: month, day: startDateDay),
            endDate: new DateTime(year: year, month: month, day: endDateDay)
        );
    }
    
    private bool Total()
    {
        var controller = new CodingController(_databaseService);
        
        try
        {
            var report = controller.PrepareRecords();
            
            _report = report.summaryForRender;
            _reportForSaving = report.summaryForSave;
            _formattedDuration = report.formattedDuration;
            
            return true;
        }
        catch (ArgumentNullException)
        {
            return false;
        }
    }

    /// <summary>
    /// Creates a report based on the provided parameters.
    /// </summary>
    /// <param name="singleDate">A boolean value indicating whether the report is for a single date or a date range.</param>
    /// <param name="startDate">The start date of the report. Nullable if singleDate is true.</param>
    /// <param name="endDate">The end date of the report. Nullable if singleDate is true.</param>
    /// <returns>A boolean value indicating whether the report was successfully created.</returns>
    private bool CreateReport(bool singleDate, DateTime? startDate = null, DateTime? endDate = null)
    {
        var tableConstructor = new SummaryConstructor();

        var dates = ProcessDates(singleDate, startDate, endDate);
        
        var records = _databaseService.RetrieveCodingSessions(dates[0], dates[1]);

        var codingSessions = CheckForAnyRecord(records);
        
        if (codingSessions.Count == 0)
        {
            return false;
        }
        
        tableConstructor.PopulateWithRecords(codingSessions);
        
        _formattedDuration = tableConstructor.FormattedDuration;
        _reportForSaving = tableConstructor.SummaryTableForSaving;
        _report = tableConstructor.SummaryTable;
        
        return true;
    }

    private void SavePrompt()
    {
        var wantsToSave = AnsiConsole.Confirm("\nWould you like to save this report?");

        if (wantsToSave)
        {
            SaveReport();
        }
    }

    /// <summary>
    /// Saves the report generated by the ReportService.
    /// The report is saved as a CSV file with a timestamp in the filename.
    /// </summary>
    private void SaveReport()
    {
        var actualTime = DateTime.Now
            .ToString("HH:mm:ss")
            .Split(":")
            .Aggregate((x, y) => x + "-" + y);
        var textWriter = new StringWriter();

        var console = AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(textWriter)
        });
        
        console.Write(_reportForSaving);
        console.Write($"Coding Tracker Report. Generated on {DateTime.Now:f}\n");
        console.Write(_formattedDuration);
        
        File.WriteAllText($"report-{DateTime.Now.Date:dd-MM-yyyy}-{actualTime}.csv", textWriter.ToString());

        AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings());
        
        AnsiConsole.MarkupLine("\n[green]Save compete[/]");
    }

    private int GetCorrectYear()
    {
        return (int)ValidateNumber(
            message:$"Enter the year (yyyy). Minimum year is {DateTime.Now.AddYears(-10):yyyy}.", 
            topLimit:uint.Parse(DateTime.Today.Year.ToString()), 
            bottomLimit:uint.Parse(DateTime.Today.Year.ToString()) - 10
        );
    }

    /// <summary>
    /// Process the dates based on the given parameters.
    /// </summary>
    /// <param name="singleDate">True if a single date is selected, false otherwise.</param>
    /// <param name="startDate">The start date of the date range, null if not applicable.</param>
    /// <param name="endDate">The end date of the date range, null if not applicable.</param>
    /// <returns>An array of DateTime containing the processed dates.</returns>
    private DateTime[] ProcessDates(bool singleDate, DateTime? startDate, DateTime? endDate)
    {
        UserInput userInput = new();
        DateTime[] processedDates;

        switch (singleDate)
        {
            case true when startDate is null:
                processedDates =
                [
                    userInput.GetDateInputs(singleDate: true)[0],
                    DateTime.Now, 
                ];
                break;
            case false when startDate is null && endDate is null:
                processedDates = userInput.GetDateInputs();
                break;
            case false when startDate is null || endDate is null:
                processedDates =
                [
                    startDate ?? userInput.GetDateInputs(singleDate:true)[0],
                    endDate ?? userInput.GetDateInputs(singleDate:true)[0]
                ];
                break;
            default:
                endDate ??= DateTime.Now; 
                processedDates = 
                [
                    startDate.Value,
                    endDate.Value
                ];
                break;
        }
        
        return processedDates;
    }

    private void DisplayAndSaveReport(Table report)
    {
        AnsiConsole.Write(report);
        SavePrompt();
    }
}