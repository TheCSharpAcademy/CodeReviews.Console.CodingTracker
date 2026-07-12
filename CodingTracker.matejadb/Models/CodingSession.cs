using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.Models;

internal class CodingSession {
    private int Id { get; set; }
    private string StartTime { get; set; }
    private string EndTime { get; set; }
    private int Duration { get; set; }

    public CodingSession(int id, string startTime, string endTime, int duration) {
        Id = id;
        StartTime = startTime;
        EndTime = endTime;
        Duration = duration;
    }

    public CodingSession() { }

    public void DisplayDetails() {
        var panel = new Panel(new Markup($"[bold]Id:[/] [cyan]{Id}[/]" +
            $"\n[bold]Start Time:[/] [cyan]{StartTime}[/]" +
            $"\n[bold]End Time:[/] [cyan]{EndTime}[/]" +
            $"\n[bold]Duration:[/] [cyan]{Duration}[/]")) {
            Border = BoxBorder.Rounded
        };

        AnsiConsole.Write(panel);
    }
}
