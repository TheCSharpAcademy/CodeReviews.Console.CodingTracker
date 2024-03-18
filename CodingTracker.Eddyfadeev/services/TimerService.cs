using System.Diagnostics;
using System.Text;
using CodingTracker.enums;
using CodingTracker.models;
using Spectre.Console;
using static CodingTracker.utils.Utilities;

namespace CodingTracker.services;

/// <summary>
/// The TimerService class handles the timer functionality and manages coding sessions.
/// </summary>
internal class TimerService
{
    private readonly DatabaseService _databaseService;
    private CodingSession? _codingSession;
    private Panel _summaryPanel;
    
    internal TimerService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        _summaryPanel = new Panel("");
        
        _codingSession = _databaseService.GetLastRecord();
        GenerateSummary();
    }
    
    internal void StartTimer()
    {
        DateTime startTime, endTime;
        var isRunning = true;
        var sw = Stopwatch.StartNew();

        startTime = DateTime.Now;
        
        Task.Run(() =>
        {
            while (isRunning)
            {
                Console.Clear();
                AnsiConsole.WriteLine("Timer started. Press enter to stop.");
                
                var timeElapsed = sw.Elapsed;
                
                AnsiConsole.MarkupLine($"[yellow]{timeElapsed:g}[/]");
                Thread.Sleep(1000);
            }
        });

        while (Console.ReadKey().Key != ConsoleKey.Enter)
        {
            // do nothing, wait for Enter keypress
        }
        
        isRunning = false;
        sw.Stop();
        endTime = DateTime.Now;
        
        _codingSession = new CodingSession
        {
            StartTime = startTime,
            EndTime = endTime
        };
        
        GenerateSummary();
        
        ShowSummary();
        
        ContinueMessage();
    }

    internal void ShowLastSession()
    {
        AnsiConsole.Write(_summaryPanel);
        ContinueMessage();
    }

    private void ShowSummary()
    {
        AnsiConsole.Write(_summaryPanel);
        SavePrompt();
    }
    
    private void GenerateSummary()
    {
        var summaryPanel = new Panel(GenerateSummaryText())
        {
            Header = new PanelHeader("Summary of the session"),
            Padding = new Padding(2, 2, 2, 2),
            Expand = true,
            BorderStyle = new Style(foreground: Color.Green3_1),
        };
        summaryPanel.NoBorder();
        
        _summaryPanel = summaryPanel;
    }

    private string GenerateSummaryText()
    {
        var stringBuilder = new StringBuilder();

        if (_codingSession is null)
        {
            stringBuilder.Append("No sessions found. Please start a new session.");

            return stringBuilder.ToString();
        }

        stringBuilder.Append($"Date of the session: {_codingSession.StartTime:dd-MM-yyyy}\n");
        stringBuilder.Append($"This session was going from {_codingSession.StartTime:HH:mm:ss} to {_codingSession.EndTime:HH:mm:ss}\n");
        stringBuilder.Append("Total time spent: ");
        
        if (_codingSession.Duration.Hours > 0)
        {
            stringBuilder.Append($"{_codingSession.Duration.Hours} " 
                                 + (_codingSession.Duration.Hours == 1 ? "hour " : "hours "));
        }

        stringBuilder.Append($"{_codingSession.Duration.Minutes} " 
                             + (_codingSession.Duration.Minutes == 1 ? "minute " : "minutes"));
        stringBuilder.Append("\nGreat job! Keep up the good work!");
        
        return stringBuilder.ToString();
    }

    private void SavePrompt()
    {
        var wantToSave = AnsiConsole.Confirm("Would you like to save this summary?");

        if (!wantToSave)
        {
            return;
        }
        
        var result = _databaseService.UpdateData(
            action:DatabaseUpdateActions.Insert,
            session:_codingSession
            );

        AnsiConsole.MarkupLine(result > 0
            ? "[green]Summary saved successfully![/]"
            : "[red]There was a problem saving the summary.[/]");
    }
}