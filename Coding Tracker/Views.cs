using Spectre.Console;

namespace CodingTracker;

public class Views
{
    // Menu selection color
    public static Style hightlightStyle = new Style().Foreground(Color.Red);

    // Session Views and general input/info
    static internal void ShowBanner()
    {
        var font = FigletFont.Load("fonts/larry3d.flf.txt");

        AnsiConsole.Write(
            new FigletText(font, "Coding Tracker")
            .Centered()
            .Color(Color.Teal));
    }

    static internal string MainMenu()
    {
        Console.Clear();
        ShowBanner();

        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(hightlightStyle)
            .Title("\nSelect an [yellow]option[/] from the menu below:")
            .PageSize(9)
            .AddChoices(new[] {
                "Start New Session", "Insert New Session", "Update Session", "Delete Session", "View Sessions", "Coding Goals", "Generate Report", "Quit",
        }));

        return menu;
    }

    static internal string DeleteMenu()
    {
        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nSelect an [yellow]option[/] from the menu below:")
            .PageSize(3)
            .HighlightStyle(hightlightStyle)
            .AddChoices(new[] {
                "Delete Session", "Clear Database", "Cancel"
            }));

        return menu;
    }

    static internal string SelectColumn()
    {
        string column = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("\nSelect the [yellow]column[/] you'd like to update:")
        .PageSize(5)
        .HighlightStyle(hightlightStyle)
        .AddChoices(new[] {
            "Id", "Date", "StartTime", "EndTime", "Back",
        }));

        return column;
    }

    static internal void PromptUser(string info, string format, string color)
    {
        AnsiConsole.MarkupInterpolated($"\nPlease input a [{color}]{info}[/]: [dim]{format}[/]\n");
    }

    static internal void ShowError(string message)
    {
        AnsiConsole.MarkupInterpolated($"\n[red]Error: {message}[/]\n");
    }

    static internal void ShowMessage(string message)
    {
        AnsiConsole.MarkupInterpolated($"\n[yellow]{message}[/]");
    }

    // Filter Views
    static internal string FilterSessionsMenu()
    {
        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .HighlightStyle(hightlightStyle)
            .Title("\nFilter sessions? Select an [yellow]option[/] below.")
            .PageSize(6)
            .AddChoices(new[] {
                "Filter by Days", "Filter by Weeks", "Filter by Months", "Filter by Ascending", "Filter by Descending", "Back"
        }));

        return menu;
    }

    static internal void ShowSpinner()
    {
        AnsiConsole.Status()
            .Start("[yellow]Sorting sessions...[/]", ctx =>
            {
                ctx.Spinner(Spinner.Known.Flip);
                ctx.SpinnerStyle(Style.Parse("yellow"));
                Thread.Sleep(4000);
            });
    }

    // Timer Views
    static internal string StartNewSessionMenu()
    {
        Console.Clear();
        ShowBanner();

        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nStart a new [yellow]session[/]?")
            .PageSize(3)
            .HighlightStyle(hightlightStyle)
            .AddChoices(new[] {
                "Start Timer", "Cancel",
        }));

        return menu;
    }

    static internal void TimerStarted(string? start)
    {
        Console.Clear();
        ShowBanner();

        AnsiConsole.MarkupInterpolated($"[yellow]Session[/] started at [red]{start}[/].\n");
        AnsiConsole.MarkupInterpolated($"Press [yellow]enter[/] to stop.\n");
    }

    static internal void PrintStopwatchDuration(string duration)
    {
        AnsiConsole.Markup($"[red]{duration}[/]");
    }

    static internal void TimerStopped(string? end)
    {
        AnsiConsole.MarkupInterpolated($"\n[yellow]Session[/] stopped at [red]{end}[/].\n");
    }

    // Goal Views
    static internal string SelectLanguage()
    {
        string language = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nSelect the [yellow]language[/] you're practicing:")
            .PageSize(9)
            .HighlightStyle(hightlightStyle)
            .AddChoices(new[] {
                "C", "C+","C#", "Python", "Java", "JavaScript", "SQL", "Go", "Rust",
            }));

        return language;
    }

    static internal string GoalsMenu()
    {
        Console.Clear();
        ShowBanner();

        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nSelect an [yellow]option[/] from the menu below:")
            .PageSize(4)
            .HighlightStyle(hightlightStyle)
            .AddChoices(new[] {
                "View Current Goals", "Create Goal", "Delete Goal", "Back",
        }));

        return menu;
    }

    static internal string DeleteGoalMenu()
    {
        string menu = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("\nSelect an [yellow]option[/] from the menu below:")
            .PageSize(3)
            .HighlightStyle(hightlightStyle)
            .AddChoices(new[] {
                "Delete Goal", "Clear Goals", "Back"
            }));

        return menu;
    }

    // Report Views
    static internal void DisplayReportLayout(BreakdownChart chart, int days, int hours, int minutes, Calendar calendar)
    {
        calendar.HeaderStyle(Style.Parse("red bold"));
        var panelBorder = BoxBorder.Double;
        var borderColor = Color.Yellow;

        var layout = new Layout("Root")
        .SplitRows(
            new Layout("Top")
                .SplitColumns(
                    new Layout("Left"),
                    new Layout("Right")),
            new Layout("Bottom"));

        layout["Right"].Update(
            new Panel(
                Align.Center(calendar))
                .Header("Days Coded")
                .Border(panelBorder)
                .BorderColor(borderColor)
                .Expand());

        layout["Left"].Update(
            new Panel(chart)
                .Header("Coding Language Breakdown")
                .Border(panelBorder)
                .BorderColor(borderColor)
                .Expand());

        layout["Bottom"].Update(
            new Panel(
                Align.Center(
                    new Markup($"All together, you've coded a grand total of [teal]{days} days,[/] [teal]{hours} hours[/] and [teal]{minutes} minutes[/]!"),
                    VerticalAlignment.Middle))
                .Header("Total Time Coded")
                .Border(panelBorder)
                .BorderColor(borderColor)
                .Expand());

        AnsiConsole.Write(layout);
    }

    static internal BreakdownChart ConfigureBreakdown(List<BreakdownItem> breakdowns)
    {
        var chart = new BreakdownChart()
            .ShowPercentage()
            .Width(60);

        foreach (BreakdownItem item in breakdowns)
        {
            chart.AddItem(new BreakdownItem(item.Label, item.Value, item.Color));
        }

        return chart;
    }
}

public class BreakdownItem : IBreakdownChartItem
{
    public string Label { get; set; }
    public double Value { get; set; }
    public Color Color { get; set; }

    public BreakdownItem(string language, double percentage, Color color)
    {
        Label = language;
        Value = percentage;
        Color = color;
    }
}