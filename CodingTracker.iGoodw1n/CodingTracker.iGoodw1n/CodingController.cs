using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CodingTracker.iGoodw1n;

public class CodingController
{
    private readonly CodingApp _app;

    public CodingController(string connectionString)
    {
        _app = new CodingApp(new DataContext(connectionString), CodingOutput.ShowOnConsole);
    }

    internal void Start()
    {
        var closeApp = false;
        while (!closeApp)
        {
            _app.Info = new InfoForOutput(StringsFor.Menu, InfoType.Text);

            var input = Console.ReadLine();

            switch (input)
            {
                case "0":
                    _app.Info = new InfoForOutput("\nGoodbye!\n", InfoType.Text);
                    closeApp = true;
                    break;
                case "1":
                    ShowAllRecords();
                    break;
                case "2":
                    CreateRecord();
                    break;
                case "3":
                    DeleteRecord();
                    break;
                case "4":
                    UpdateRecord();
                    break;
                case "5":
                    ShowReport();
                    break;
                default:
                    _app.Info = new InfoForOutput("\nInvalid Command. Please type a number from 0 to 4.\n", InfoType.Text);
                    break;
            }
        }
    }

    private void UpdateRecord()
    {
        _app.ShowAllRecords();

        var id = GetParsedUserInput<int>("\nPlease type the id of row that you want to update or type 0 to return to Main Menu: ");

        if (id == 0) return;

        _app.ShowRecord(id);

        var record = (CodingSession?)_app.Info.Information;

        if (record is null)
        {
            return;
        }

        record.Language = GetUpdatedValue("Language", record.Language);
        record.Start = GetUpdatedDateTime("Start time", record.Start);
        record.End = GetUpdatedDateTime("End time", record.End);

        if (!ValidateCodingSession(record))
        {
            return;
        }

        _app.UpdateRecord(record);
        _app.Info = new InfoForOutput("Record was successfully updated", InfoType.Text);
        _app.ShowRecord(record.Id);

        string GetUpdatedValue(string field, string currentValue)
        {
            _app.Info = new InfoForOutput($"Change {field} or just press Enter\nCurrent value: {currentValue}", InfoType.Text);
            var newValue = Console.ReadLine();
            return string.IsNullOrEmpty(newValue) ? currentValue : newValue;
        }

        DateTime GetUpdatedDateTime(string field, DateTime currentValue)
        {
            string input;
            DateTime date;
            do
            {
                _app.Info = new InfoForOutput($"Change {field} or just press Enter\nCurrent value: {currentValue:dd-MM-yy_HH:mm}", InfoType.Text);
                input = Console.ReadLine()!;

                if (input == "") return currentValue;
            } while (!DateTime.TryParseExact(input, "dd-MM-yy_HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out date));

            return date;
        }
    }

    private void DeleteRecord()
    {
        ShowAllRecords();

        var id = GetParsedUserInput<int>("\nPlease type the id of row that you want to delete or type 0 to return to Main Menu: ");

        if (id == 0) return;

        var input = GetUserInput("This record will be deleted. Are you sure? y/n");

        if (!input.Equals("y", StringComparison.OrdinalIgnoreCase)) return;

        _app.DeleteRecord(id);
    }

    private void CreateRecord()
    {
        var language = GetUserInput("\nPlease enter Programming Language: ");
        var start = GetDateTimeInput(StringsFor.StartTimeQuery);
        if (start is null) return;

        var end = GetDateTimeInput(StringsFor.EndTimeQuery);
        if (end is null) return;

        var session = new CodingSession
        {
            Language = language,
            Start = start.Value,
            End = end.Value
        };

        if (ValidateCodingSession(session))
        {
            _app.InsertSession(session);
        }
    }

    private bool ValidateCodingSession(CodingSession session)
    {
        var validationContext = new ValidationContext(session);
        var results = new List<ValidationResult>();
        if (Validator.TryValidateObject(session, validationContext, results, true))
        {
            return true;
        }

        foreach (var item in results)
        {
            _app.Info = new InfoForOutput(item.ErrorMessage, InfoType.Text);
        }
        return false;
    }

    private void ShowAllRecords()
    {
        _app.ShowAllRecords();
    }

    private void ShowReport()
    {
        _app.ShowAllYears();

        if (_app.Info is null) return;

        var year = GetParsedUserInput<int>($"\nEnter Year to generate annual report: ");

        _app.ShowReport(year);
    }

    public T GetParsedUserInput<T>(string textToShow) where T : IParsable<T>
    {
        _app.Info = new InfoForOutput(textToShow, InfoType.Text);

        var input = Console.ReadLine();
        T? result;

        while (!T.TryParse(input, CultureInfo.InvariantCulture, out result) && result is not null)
        {
            _app.Info = new InfoForOutput("Something goes wrong!", InfoType.Text);
            _app.Info = new InfoForOutput(textToShow, InfoType.Text);
            input = Console.ReadLine();
        }

        return result!;
    }

    public string GetUserInput(string textToShow)
    {
        _app.Info = new InfoForOutput(textToShow, InfoType.Text);

        return Console.ReadLine()!;
    }

    public DateTime? GetDateTimeInput(string textToShow)
    {
        string input;
        DateTime date;
        do
        {
            _app.Info = new InfoForOutput(textToShow, InfoType.Text);
            input = Console.ReadLine()!;

            if (input == "0") return null;
            if (input == "") return DateTime.Now;
        } while (!DateTime.TryParseExact(input, "dd-MM-yy_HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out date));

        return date;
    }
}
