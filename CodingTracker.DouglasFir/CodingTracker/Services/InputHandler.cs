using CodingTracker.Models;
using Spectre.Console;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CodingTracker.Services;

public class InputHandler
{
    public DateTime PromptForDate(string promptMessage, DatePrompt datePromptFormat)
    {
        var dateTimeFormat = datePromptFormat == DatePrompt.Short ? ConfigSettings.DateFormatShort : ConfigSettings.DateFormatLong;

        string dateTimeInput = AnsiConsole.Prompt(
            new TextPrompt<string>(promptMessage)
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (DateTime.TryParseExact(input.Trim(), dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        if (parsedDate <= DateTime.Now && parsedDate > DateTime.MinValue)
                        {
                            return ValidationResult.Success();
                        }
                        else
                        {
                            var errorMessage = $"[red]Date cannot be in the future.[/]";
                            return ValidationResult.Error(errorMessage.ToString());
                        }
                    }
                    else
                    {
                        var errorMessage = $"[red]Invalid date format. Please use the format {dateTimeFormat}.[/]";
                        return ValidationResult.Error(errorMessage.ToString());
                    }
                }));

        return DateTime.ParseExact(dateTimeInput, dateTimeFormat, CultureInfo.InvariantCulture);
    }

    public TimeSpan PromptForTimeSpan(string promptMessage)
    {
        string durationInput = AnsiConsole.Prompt(
            new TextPrompt<string>(promptMessage)
                .PromptStyle("yellow")
                .Validate(input =>
                {
                    var regex = new System.Text.RegularExpressions.Regex(@"^\d{2}:\d{2}$");
                    if (!regex.IsMatch(input))
                    {
                        var errorMessage = $"[red]Invalid time format. Please use the format {ConfigSettings.TimeFormatString}.[/]";
                        return ValidationResult.Error(errorMessage.ToString());
                    }
                    else if (TimeSpan.TryParseExact(input.Trim(), ConfigSettings.TimeFormatType, CultureInfo.InvariantCulture, out TimeSpan parsedTime))
                    {
                        return ValidationResult.Success();
                    }
                    else
                    {
                        var errorMessage = $"[red]Invalid time duration. Please ensure the input format is correct.[/]";
                        return ValidationResult.Error(errorMessage.ToString());
                    }
                }));

        return TimeSpan.ParseExact(durationInput, ConfigSettings.TimeFormatType, CultureInfo.InvariantCulture);
    }

    public CodingSessionModel PromptForSessionListSelection(List<CodingSessionModel> sessionLogs, string promptMessage)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<CodingSessionModel>()
                .Title(promptMessage)
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more log entries)[/]")
                .UseConverter(entry =>
                    $"[bold yellow]ID:[/] {entry.Id}, " +
                    $"[bold cyan]Session Date:[/] {entry.SessionDate}, " +
                    (entry.StartTime != null ? $"[bold green]Start Time:[/] {entry.StartTime}, " : "") +
                    (entry.EndTime != null ? $"[bold magenta]End Time:[/] {entry.EndTime}, " : "") +
                    $"[bold blue]Duration:[/] {entry.Duration}")
                .AddChoices(sessionLogs));
    }

    public List<CodingSessionModel.EditableProperties> PromptForSessionPropertiesSelection(string promptMessage)
    {
        return AnsiConsole.Prompt(
            new MultiSelectionPrompt<CodingSessionModel.EditableProperties>()
                .Title("Select properties you want to edit:")
                .NotRequired()
                .PageSize(10)
                .InstructionsText("[grey](Press [blue]<space>[/] to toggle a property, [green]<enter>[/] to accept, [yellow]<enter>[/] with no selections will cancel update)[/]")
                .AddChoices(Enum.GetValues<CodingSessionModel.EditableProperties>()));
    }

    public (TimePeriod? periodFilter, int? numOfPeriods) PromptForTimePeriodAndCount()
    {
        TimePeriod? periodFilter = null;
        int? numOfPeriods = null;

        if (AnsiConsole.Confirm("Would you like to filter by past Time Periods (days, weeks, years)?"))
        {
            periodFilter = PromptForQueryTimePeriodOptions();
            string promptMessage = $"Please enter number of {periodFilter} to retrieve:";
            numOfPeriods = PromptForPositiveInteger(promptMessage);
        }

        return (periodFilter, numOfPeriods);
    }

    public TimePeriod? PromptForQueryTimePeriodOptions()
    {
        try
        {
            return AnsiConsole.Prompt(
                new SelectionPrompt<TimePeriod>()
                    .Title("Select [blueviolet]TimePeriod[/] filter criteria:")
                    .PageSize(10)
                    .UseConverter(options => Utilities.SplitCamelCase(options.ToString()))
                    .AddChoices(Enum.GetValues<TimePeriod>()));
        }
        catch (OperationCanceledException)
        {
            return null;
        }
    }

    public List<(CodingSessionModel.EditableProperties, SortDirection, int)> PromptForOrderByFilterOptions()
    {
        AnsiConsole.Clear();

        string instructionText = "[grey](Press [blue]<space>[/] to toggle a selection, [green]<enter>[/] to accept, or [yellow]<enter> (with no options)[/] to skip ordering)[/]";
        var selectedColumns = AnsiConsole.Prompt(
            new MultiSelectionPrompt<CodingSessionModel.EditableProperties>()
                .Title("Select Order By filter criteria [yellow](optional):[/]")
                .NotRequired() // Order by sorting not required
                .PageSize(10)
                .InstructionsText(instructionText)
                .UseConverter(options => Utilities.SplitCamelCase(options.ToString()))
                .AddChoices(Enum.GetValues<CodingSessionModel.EditableProperties>()));

        if (!selectedColumns.Any())
            return new List<(CodingSessionModel.EditableProperties, SortDirection, int)>();

        var sortQueryOptionsTupleList = BuildOrderByOptionsTupleList(selectedColumns);

        if (AnsiConsole.Confirm("Run query with these properties and directions? ('n' to reselect options)"))
        {
            return sortQueryOptionsTupleList;
        }
        else
        {
            return PromptForOrderByFilterOptions(); // Recurse on modifications
        }
    }

    public List<(CodingSessionModel.EditableProperties, SortDirection, int)> BuildOrderByOptionsTupleList(List<CodingSessionModel.EditableProperties> selectedColumns)
    {
        Color[] colors = new Color[] {
            Color.Teal,
            Color.IndianRed,
            Color.RoyalBlue1,
            Color.Yellow4_1
        };

        // Initialize list of selected column options to display table view while showing ordering/rank selections
        List<(CodingSessionModel.EditableProperties property, SortDirection? direction, int? rank)> orderByTuple = selectedColumns
            .Select(property => (property, (SortDirection?)null, (int?)null))
            .ToList();

        // Set Ordering directions for selected column options
        for (int i = 0; i < orderByTuple.Count; i++)
        {
            Utilities.DisplayCurrentQuerySelections(orderByTuple, colors);
            var direction = AnsiConsole.Prompt(
                new SelectionPrompt<SortDirection>()
                    .Title($"Select the sort direction for [{colors[i]}]{Utilities.SplitCamelCase(orderByTuple[i].property.ToString())}[/]:")
                    .AddChoices(Enum.GetValues<SortDirection>()));

            orderByTuple[i] = (orderByTuple[i].property, direction, orderByTuple[i].rank);

        }

        // Set Ordering ranks for selected column options
        for (int i = 0; i < orderByTuple.Count; i++)
        {
            Utilities.DisplayCurrentQuerySelections(orderByTuple, colors);
            int rank = AnsiConsole.Prompt(
                new TextPrompt<int>($"Enter the rank for [{colors[i]}]{Utilities.SplitCamelCase(orderByTuple[i].property.ToString())}[/]:")
                    .Validate(input =>
                    {
                        if (input < 1 || input > orderByTuple.Count || orderByTuple.Any(p => p.rank == input))
                            return ValidationResult.Error("[red]Invalid rank. Ensure ranks are unique and within the correct range.[/]");
                        return ValidationResult.Success();
                    }));

            orderByTuple[i] = (orderByTuple[i].property, orderByTuple[i].direction, rank);
        }

        // Update after final selection
        AnsiConsole.Clear();
        Utilities.DisplayCurrentQuerySelections(orderByTuple, colors);

        return orderByTuple
            .Select(p => (p.property, p.direction!.Value, p.rank!.Value))
            .OrderBy(p => p.Item3)
            .ToList();
    }

    public int PromptForPositiveInteger(string promptMessage)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<int>(promptMessage)
                .Validate(input =>
                {
                    if (!int.TryParse(input.ToString().Trim(), out int parsedQuantity))
                    {
                        return ValidationResult.Error("[red]Please enter a valid integer number.[/]");
                    }

                    if (parsedQuantity <= 0)
                    {
                        return ValidationResult.Error("[red]Please enter a positive number.[/]");
                    }

                    return ValidationResult.Success();
                }));
    }

    public CodingGoalModel PromptForGoalSelection(List<CodingGoalModel> codingGoals, string promptMessage)
    {
        return AnsiConsole.Prompt(
            new SelectionPrompt<CodingGoalModel>()
                .Title(promptMessage)
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to see more goal entries)[/]")
                .UseConverter(entry =>
                $"[bold yellow]ID:[/] {entry.Id}, " +
                $"[bold cyan]Goal Description:[/] {entry.Description}, " +
                $"[bold green]Target Hours:[/] {entry.TargetDuration}, " +
                $"[bold magenta]Completed Hours:[/] {entry.CurrentProgress}%")
                .AddChoices(codingGoals));
    }

    public string PromptForGoalDescription(string promptMessage)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<string>(promptMessage)
            .PromptStyle("yellow")
                .Validate(input =>
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return ValidationResult.Error("[red]Goal description cannot be empty.[/]");
                
                    return ValidationResult.Success();
                }));
    } 

    public void PauseForContinueInput()
    {
        AnsiConsole.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public bool ConfirmAction(string actionPromptMessage) 
    {
        if (!AnsiConsole.Confirm(actionPromptMessage))
        {
            Utilities.DisplayCancellationMessage("Operation cancelled.");
            PauseForContinueInput();
            return false;
        }

        return true;
    }
}
