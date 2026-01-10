using Spectre.Console;

internal class UserInterface
{
    private static List<FilterCondition> filterConditions = new List<FilterCondition>();

    public static void MainMenu()
    {
        CreateTitlePanel("Welcome to the Coding Tracker!");

        var menuOptions = new[] {
            "See Existing Records",
            "Create Coding Session",
            "Exit"
        };
        var menuInput = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions));

        AnsiConsole.Clear();

        switch (menuInput)
        {
            case "See Existing Records":
                ExistingRecordsInterface();
                break;
            case "Create Coding Session":
                NewRecordInterface();
                break;
            case "Exit":
                Environment.Exit(0);
                return;
        }
    }

    private static void ExistingRecordsInterface()
    {
        var title = "Existing Records!";
        CreateTitlePanel(title);

        var menuOptions = DatabaseController.GetRecords(filterConditions);

        if (filterConditions.Count > 0)
        {
            menuOptions.Add("Reset Filters");
        }

        menuOptions.Add("Add Filter");
        menuOptions.Add("Exit");

        var menuInput = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(menuOptions));

        switch (menuInput)
        {
            case "Reset Filters":
                filterConditions.Clear();
                ExistingRecordsInterface();
                break;
            case "Add Filter":
                FiltersInterface();
                break;
            case "Exit":
                filterConditions.Clear();
                MainMenu();
                break;
            default:
                var record = new CodingSession(menuInput);
                EditRecordInterface(record);
                break;
        }
    }

    private static void FiltersInterface()
    {
        var title = "Filter Options";
        CreateTitlePanel(title);

        var filterOptions = new[]
        {
            "Date",
            "Start Time",
            "End Time",
            "Duration",
            "Return"
        };

        var menuInput = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(filterOptions));
        var condition = FilterCondition.ConditionType.None;

        AnsiConsole.Clear();

        switch (menuInput)
        {
            case "Date":
                condition = FilterCondition.ConditionType.Date;
                RelationalFiltersInterface(condition);
                return;
            case "Start Time":
                condition = FilterCondition.ConditionType.StartTime;
                RelationalFiltersInterface(condition);
                break;
            case "End Time":
                condition = FilterCondition.ConditionType.EndTime;
                RelationalFiltersInterface(condition);
                break;
            case "Duration":
                condition = FilterCondition.ConditionType.Duration;
                RelationalFiltersInterface(condition);
                break;
            case "Return":
                return;
            default:
                AnsiConsole.WriteLine("Something went wrong! Could not use condition type! Try Again!");
                FiltersInterface();
                return;
        }

    }

    private static void RelationalFiltersInterface(FilterCondition.ConditionType type)
    {
        var title = "Filter Options";
        CreateTitlePanel(title);

        var additionalFilterOptions = new[]
        {
            "Greater than or Equal",
            "Less than or Equal",
            "Exactly"
        };

        var menuInput = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices(additionalFilterOptions));
        var relational = FilterCondition.RelationalCondition.None;

        AnsiConsole.Clear();

        switch (menuInput)
        {
            case "Greater than or Equal":
                relational = FilterCondition.RelationalCondition.GreaterThanOrEqual;
                break;
            case "Less than or Equal":
                relational = FilterCondition.RelationalCondition.LessThanOrEqual;
                break;
            case "Exactly":
                relational |= FilterCondition.RelationalCondition.Exactly;
                break;
            default:
                AnsiConsole.WriteLine("Something went wrong! Could not use relational! Try Again!");
                RelationalFiltersInterface(type);
                return;
        }

        ComparisonFilterInterface(type, relational);
    }

    private static void ComparisonFilterInterface(FilterCondition.ConditionType type, FilterCondition.RelationalCondition relational)
    {
        var title = "Filter Options";
        CreateTitlePanel(title);

        var endAsk = false;
        var comparison = string.Empty;
        var format = string.Empty;

        if (type == FilterCondition.ConditionType.Date) format = Globals.DATE_FORMAT;
        else format = Globals.TIME_FORMAT;

        while (!endAsk)
        {
            comparison = AnsiConsole.Ask<string>($"Compared to ({format}):");
            endAsk = Validation.CheckValidTime(comparison);

            if (endAsk == false)
            {
                AnsiConsole.Clear();
                CreateTitlePanel(title);
                AnsiConsole.WriteLine($"Enter a valid format ({format})");
            }
        }
        
        var filter = new FilterCondition(type, relational, comparison);

        filterConditions.Add(filter);
        ExistingRecordsInterface();
    }

    private static void EditRecordInterface(CodingSession record)
    {
        var title = "Edit Record";
        CreateTitlePanel(title);

        var menuOptions = new[]
        {
            "Edit",
            "Remove",
            "Return"
        };

        var menuInput = AnsiConsole.Prompt<string>(new SelectionPrompt<string>().AddChoices(menuOptions));

        switch (menuInput)
        {
            case "Edit":
                ChangeRecordInfoInterface(record);
                break;
            case "Remove":
                DatabaseController.RemoveRecord(record);
                break;
            case "Return":
                return;
            default:
                AnsiConsole.WriteLine("Something went wrong!");
                break;
        }

        ExistingRecordsInterface();
    }

    private static void ChangeRecordInfoInterface(CodingSession record)
    {
        var title = "Choose what to edit";
        CreateTitlePanel(title);

        var menuOptions = new[]
        {
            "Record Name",
            "Date",
            "Start Time",
            "End Time",
            "Return"
        };
        
        var valueToChange = string.Empty;
        var newValue = string.Empty;

        var menuInput = AnsiConsole.Prompt<string>(new SelectionPrompt<string>().AddChoices(menuOptions));

        switch (menuInput)
        {
            case "Record Name":
                valueToChange = "name";
                newValue = AnsiConsole.Ask<string>("Change To:");
                break;
            case "Date":
                valueToChange = "date";
                newValue = UserInput.GetUserDate();
                break;
            case "Start Time":
                valueToChange = "start";
                newValue = UserInput.GetUserTime();
                record.ChangeTime(newValue, record.EndTime);
                break;
            case "End Time":
                valueToChange = "end";
                newValue = UserInput.GetUserTime();
                record.ChangeTime(record.StartTime, newValue);
                break;
            case "Return":
                return;
        }

        AnsiConsole.WriteLine(record.Duration);
        DatabaseController.EditRecord(record, valueToChange, newValue);
    }

    private static void NewRecordInterface()
    {
        var title = "Log a Custom Coding Session!";
        CreateTitlePanel(title);

        var startTime = string.Empty;
        var endTime = string.Empty;

        AnsiConsole.WriteLine("Record Name:");
        var recordName = AnsiConsole.Ask<string>("Enter Here: ");

        CreateTitlePanel(title);
        var date = UserInput.GetUserDate();

        CreateTitlePanel(title);
        startTime = UserInput.GetUserTime();

        
        while (true)
        {
            CreateTitlePanel(title);
            endTime = UserInput.GetUserTime();

            if(Validation.CheckValidEndTime(startTime, endTime))
            {
                break;
            }
        }
        

        CodingSession newRecord = new CodingSession(recordName, date, startTime, endTime);
        DatabaseController.AddRecord(newRecord);

        MainMenu();
    }

    private static void CreateTitlePanel(string title)
    {
        var titlePanel = new Panel(title)
        {
            Border = BoxBorder.Double,
            Padding = new Padding(2, 0)
        };

        AnsiConsole.Clear();
        AnsiConsole.Write(titlePanel);
    }
}