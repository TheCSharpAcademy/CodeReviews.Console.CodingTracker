using System.ComponentModel.DataAnnotations;
using Spectre.Console;
using static CodingTracker.Golvi1124.Helpers.Sorting;
using static CodingTracker.Golvi1124.UI.Enums;

namespace CodingTracker.Golvi1124.Helpers;

public static class Extras
{
    public static string GetEnumDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        if (field == null) return value.ToString();

        var attribute = Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) as DisplayAttribute;
        return attribute?.Name ?? value.ToString();
    }

    internal static bool AskSortDirection()
    {
        AnsiConsole.Markup("[blue]Sort records descending? (y/n): [/]");
        string? input = Console.ReadLine()?.Trim().ToLower();

        return input == "y" || input == "yes";
    }

    internal static PeriodType AskPeriodType()
    {
        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose [blue]period type[/] for the report:")
                .AddChoices(["Year", "Month", "Week", "Day", "[grey]Back[/]"])
        );

        return selected switch
        {
            "Year" => PeriodType.Year,
            "Month" => PeriodType.Month,
            "Week" => PeriodType.Week,
            "Day" => PeriodType.Day,
            "[grey]Back[/]" => PeriodType.Back,
            _ => throw new Exception("[red]Invalid selection[/]")
        };
    }

    internal static DateRange AskDateRangeForPeriod(PeriodType period)
    {
        switch (period)
        {
            case PeriodType.Year:
                return AskForYearRange();

            case PeriodType.Month:
                return AskForMonthRange();

            case PeriodType.Week:
                return AskForWeekRange();

            case PeriodType.Day:
                return AskForDayRange();

            case PeriodType.Back:

            default:
                throw new Exception("Unsupported period");
        }
    }
}