using CodingTracker.matejadb.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTracker.matejadb.Controllers;

internal class BaseController {
    protected bool ConfirmDeletion(CodingSession session) {
        var confirm = AnsiConsole.Confirm($"Are you sure you want to delete [red]{session.Id} {session.StartTime} {session.EndTime} {session.Duration}[/]");

        return confirm;
    }
}
