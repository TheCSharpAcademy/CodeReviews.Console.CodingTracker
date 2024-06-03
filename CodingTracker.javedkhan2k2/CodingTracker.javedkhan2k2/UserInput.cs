using System.Text.RegularExpressions;
using CodingTracker.Models;
using Spectre.Console;

namespace CodingTracker;

internal static class UserInput
{
    internal static string GetDateTimeInput(string message)
    {
        string? userInput = AnsiConsole.Ask<string?>($"{message} Or Enter 0 to cancel:");
        while (!Validation.IsValidDateTimeInput(userInput))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold]Invalid input [red]({userInput})[/][/].\n{message} Or Enter 0 to cancel:");
        }
        return userInput;
    }

    internal static int GetIntegerValue(string message)
    {
        string? userInput = AnsiConsole.Ask<string?>($"{message} Or Enter 0 to cancel:");
        while (!Validation.IsValidIntegerInput(userInput))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold red]Invalid input {userInput}[/]\n{message} Or Enter 0 to cancel:");
        }
        return Convert.ToInt32(userInput);
    }

    internal static CodingSessionDto NewCodingSessionDialog()
    {
        CodingSessionDto codingSessionDto = new CodingSessionDto();
        while (true)
        {
            AnsiConsole.Clear();
            VisualisationEngine.DisplaySessionDto(codingSessionDto);
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Add new Coding Session Dialog")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Start Date Time",
                        "End Date Time",
                        "Submit",
                        "Cancel"
                }));
            switch (choice)
            {
                case "Start Date Time":
                    codingSessionDto.StartTime = GetDateInput("Enter Start date and time in [bold green](yyyy-MM-dd HH:mm:ss)[/] format.", "yyyy-MM-dd HH:mm:ss");
                    codingSessionDto.Duration = Helpers.CalculateDuration(codingSessionDto.StartTime, codingSessionDto.EndTime);
                    break;
                case "End Date Time":
                    codingSessionDto.EndTime = GetDateInput("Enter End date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format.", "yyyy-MM-dd HH:mm:ss");
                    codingSessionDto.Duration = Helpers.CalculateDuration(codingSessionDto.StartTime, codingSessionDto.EndTime);
                    break;
                case "Submit":
                    if (Validation.IsValidDateTimeInputs(codingSessionDto.StartTime, codingSessionDto.EndTime))
                    {
                        return codingSessionDto;
                    }
                    else
                    {
                        VisualisationEngine.DisplayInvalidDateInputError(codingSessionDto.StartTime, codingSessionDto.EndTime);
                    }
                    break;
                case "Cancel":
                    return null;
                default:
                    break;
            }
        }
    }

    internal static CodingSessionDto UpdateCodingSessionDialog(CodingSession codingSession)
    {
        CodingSessionDto codingSessionDto = new CodingSessionDto
        {
            StartTime = codingSession.StartTime.ToString("yyyy-MM-dd HH:mm:ss"),
            EndTime = codingSession.EndTime.ToString("yyyy-MM-dd HH:mm:ss"),
            Duration = codingSession.DurationInSeconds
        };
        while (true)
        {
            AnsiConsole.Clear();

            VisualisationEngine.DisplaySessionDto(codingSessionDto);
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Update Coding Session Dialog")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "Start Date Time",
                        "End Date Time",
                        "Submit",
                        "Cancel"
                }));
            switch (choice)
            {
                case "Start Date Time":
                    codingSessionDto.StartTime = GetDateTimeInput("Enter Start date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format ");
                    codingSessionDto.Duration = Helpers.CalculateDuration(codingSessionDto.StartTime, codingSessionDto.EndTime);
                    break;
                case "End Date Time":
                    codingSessionDto.EndTime = GetDateTimeInput("Enter End date and time in [bold green](yyyy-MM-dd hh:mm:ss)[/] format ");
                    codingSessionDto.Duration = Helpers.CalculateDuration(codingSessionDto.StartTime, codingSessionDto.EndTime);
                    break;
                case "Submit":
                    if (Validation.IsValidDateTimeInputs(codingSessionDto.StartTime, codingSessionDto.EndTime))
                    {
                        return codingSessionDto;
                    }
                    else
                    {
                        VisualisationEngine.DisplayInvalidDateInputError(codingSessionDto.StartTime, codingSessionDto.EndTime);
                    }
                    break;
                case "Cancel":
                    return null;
                default:
                    break;
            }
        }
    }

    internal static string GetDateInput(string message, string format)
    {
        string? userInput = AnsiConsole.Ask<string?>($"{message} Or Enter 0 to cancel:");
        while (!Validation.IsValidDateInput(userInput, format))
        {
            userInput = AnsiConsole.Ask<string?>($"[bold]Invalid input [red]({userInput})[/][/].\n{message} Or Enter 0 to cancel:");
        }
        return userInput == "0" ? "" : userInput;
    }

    internal static ReportDto? GetDailyReportInput()
    {
        ReportDto reportInput = new ReportDto();
        while (true)
        {
            AnsiConsole.Clear();
            VisualisationEngine.DisplayReportDto(reportInput);
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Enter the input below to generate Report")
                    .AddChoices(new[]
                    {
                        "Start Date",
                        "End Date",
                        "Sort",
                        "Generate Report",
                        "Cancel"
                    })
            );
            switch (choice)
            {
                case "Start Date":
                    reportInput.StartDate = GetDateInput("Enter Start Date in format: (yyyy-MM-dd)", "yyyy-MM-dd");
                    break;
                case "End Date":
                    reportInput.EndDate = GetDateInput("Enter End Date in format: (yyyy-MM-dd)", "yyyy-MM-dd");
                    break;
                case "Sort":
                    reportInput.Sort = GetSortInput("Enter 'a' for ascending or 'd' descending to sort the report");
                    break;
                case "Generate Report":
                    if (Validation.IsValidDateTimeInputs(reportInput.StartDate, reportInput.EndDate))
                    {
                        return reportInput;
                    }
                    else
                    {
                        VisualisationEngine.DisplayInvalidDateInputError(reportInput.StartDate, reportInput.EndDate);
                        break;
                    }
                case "Cancel":
                    return null;
            }
        }
    }

    private static string GetSortInput(string message)
    {
        Console.WriteLine(message);
        string? userInput = Console.ReadLine();
        while (userInput == null || !Regex.IsMatch(userInput.ToLower(), "[a|d]"))
        {
            Console.WriteLine($"Invalid Input! {message}");
            userInput = Console.ReadLine();
        }
        return userInput.ToLower() == "a" ? "asc" : "desc";
    }

    internal static ReportDto GetYearlyReportInput()
    {
        ReportDto reportInput = new ReportDto();
        while (true)
        {
            AnsiConsole.Clear();
            VisualisationEngine.DisplayReportDto(reportInput);
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Enter the input below to generate Report")
                    .AddChoices(new[]
                    {
                        "Start Date",
                        "End Date",
                        "Sort",
                        "Generate Report",
                        "Cancel"
                    })
            );
            switch (choice)
            {
                case "Start Date":
                    reportInput.StartDate = GetDateInput("Enter Start Date in format: (yyyy)", "yyyy");
                    break;
                case "End Date":
                    reportInput.EndDate = GetDateInput("Enter End Date in format: (yyyy)", "yyyy");
                    break;
                case "Sort":
                    reportInput.Sort = GetSortInput("Enter 'a' for ascending or 'd' descending to sort the report");
                    break;
                case "Generate Report":
                    if (Validation.IsValidYearInputs(reportInput.StartDate, reportInput.EndDate))
                    {
                        return reportInput;
                    }
                    else
                    {
                        VisualisationEngine.DisplayInvalidDateInputError(reportInput.StartDate, reportInput.EndDate);
                        break;
                    }
                case "Cancel":
                    return null;
            }
        }
    }

    internal static bool GetStopWatchInput(string message)
    {
        AnsiConsole.Markup(message);
        var input = Console.ReadLine();
        if (input != null && input.ToLower() == "0")
        {
            return false;
        }
        while (input == null || input.ToLower() != "s")
        {
            AnsiConsole.Markup($"Invalid input! {message}");
            input = Console.ReadLine();
            if (input != null && input.ToLower() == "0")
            {
                return false;
            }
        }
        return true;
    }

}