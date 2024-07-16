using CodingTracker.ConsoleApp.Models;
using CodingTracker.ConsoleApp.Services;
using CodingTracker.Constants;
using CodingTracker.Enums;
using CodingTracker.Models;
using CodingTracker.Services;
using Spectre.Console;

namespace CodingTracker.ConsoleApp.Views;

/// <summary>
/// Page which allows users to configure a report filter.
/// </summary>
internal class ReportFilterPage : BasePage
{
    #region Constants

    private const string PageTitle = "Filter Report";

    #endregion
    #region Properties

    internal static IEnumerable<UserChoice> ReportFilterTypeChoices
    {
        get
        {
            return
            [
                new(1, "All"),
                new(2, "By day"),
                new(3, "By week"),
                new(4, "By month"),
                new(5, "By year"),
                new(0, "Close page")
            ];
        }
    }

    internal static IEnumerable<UserChoice> ReportFilterOrderByChoices
    {
        get
        {
            return
            [
                new(1, "Ascending"),
                new(2, "Descending"),
            ];
        }
    }

    #endregion
    #region Methods - Internal

    internal static ReportFilter? Show()
    {
        AnsiConsole.Clear();

        WriteHeader(PageTitle);

        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<UserChoice>()
            .Title(PromptTitle)
            .AddChoices(ReportFilterTypeChoices)
            .UseConverter(c => c.Name!)
            );

        // Default values.
        ReportFilterType filterType = ReportFilterType.Day;
        DateTime? startDate = null;
        DateTime? endDate = null;

        switch (choice.Id)
        {
            case 1:

                // All.
                return new ReportFilter
                {
                    Type = filterType,
                    StartDate = startDate,
                    EndDate = endDate,
                    OrderBy = GetReportOrderByType()
                };

            case 2:

                // By day.
                filterType = ReportFilterType.Day;
                startDate = GetStartDate(GetDateStringFormat(filterType));
                if (startDate == null)
                {
                    return null;
                }
                endDate = GetEndDate(GetDateStringFormat(filterType), startDate.Value.Date);
                if (endDate == null)
                {
                    return null;
                }
                endDate = endDate.Value.AddDays(1).AddTicks(-1);

                return new ReportFilter
                {
                    Type = filterType,
                    StartDate = startDate,
                    EndDate = endDate,
                    OrderBy = GetReportOrderByType()
                };

            case 3:

                // By week.
                filterType = ReportFilterType.Week;
                startDate = GetStartDate(GetDateStringFormat(filterType));
                if (startDate == null)
                {
                    return null;
                }
                endDate = GetEndDate(GetDateStringFormat(filterType), startDate.Value.Date);
                if (endDate == null)
                {
                    return null;
                }
                endDate = endDate.Value.AddDays(1).AddTicks(-1);

                return new ReportFilter
                {
                    Type = filterType,
                    StartDate = startDate,
                    EndDate = endDate,
                    OrderBy = GetReportOrderByType()
                };

            case 4:

                // By month.
                filterType = ReportFilterType.Month;
                startDate = GetStartDate(GetDateStringFormat(filterType));
                if (startDate == null)
                {
                    return null;
                }
                endDate = GetEndDate(GetDateStringFormat(filterType), startDate.Value.Date);
                if (endDate == null)
                {
                    return null;
                }
                endDate = endDate.Value.AddMonths(1).AddTicks(-1);
                
                return new ReportFilter
                {
                    Type = filterType,
                    StartDate = startDate,
                    EndDate = endDate,
                    OrderBy = GetReportOrderByType()
                };

            case 5:

                // By year.
                filterType = ReportFilterType.Year;
                startDate = GetStartDate(GetDateStringFormat(filterType));
                if (startDate == null)
                {
                    return null;
                }
                endDate = GetEndDate(GetDateStringFormat(filterType), startDate.Value.Date);
                if (endDate == null)
                {
                    return null;
                }
                endDate = endDate.Value.AddYears(1).AddTicks(-1);

                return new ReportFilter
                {
                    Type = filterType,
                    StartDate = startDate,
                    EndDate = endDate,
                    OrderBy = GetReportOrderByType()
                };

            default:

                // Close page.
                return null;
        }
    }

    #endregion
    #region Methods - Private

    private static string GetDateStringFormat(ReportFilterType filterType)
    {
        return filterType switch
        {
            ReportFilterType.All => StringFormat.DateTime,
            ReportFilterType.Day => StringFormat.Date,
            ReportFilterType.Week => StringFormat.Date,
            ReportFilterType.Month => StringFormat.YearMonth,
            ReportFilterType.Year => StringFormat.Year,
            _ => StringFormat.DateTime
        };
    }

    private static DateTime? GetStartDate(string format)
    {
        DateTime? start = UserInputService.GetDateTime(
            $"Enter the start date, format [blue]{format}[/], or [blue]0[/] to return to main menu: ",
            format,
            input => UserInputValidationService.IsValidReportStartDate(input, format)
        );

        return start == null ? null : start;
    }

    private static DateTime? GetEndDate(string format, DateTime startDate)
    {
        DateTime? end = UserInputService.GetDateTime(
            $"Enter the end date, format [blue]{format}[/], or [blue]0[/] to return to main menu: ",
            format,
            input => UserInputValidationService.IsValidReportEndDate(input, format, startDate)
            );

        return end == null ? null : end;
    }

    private static ReportOrderByType GetReportOrderByType()
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<UserChoice>()
            .Title(PromptTitle)
            .AddChoices(ReportFilterOrderByChoices)
            .UseConverter(c => c.Name!)
        );

        return choice.Id switch
        {
            1 => ReportOrderByType.Ascending,
            2 => ReportOrderByType.Descending,
            _ => throw new ArgumentException("Unsupported ReportOrderByType"),
        };
    }

    #endregion
}
