using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker
{
    internal class Controller
    {
        private Database db = new();
        internal void ViewAllSession()
        {
            AnsiConsole.Clear();
            List<CodingSession> sessions = db.GetAllSession();
            if (!sessions.Any())
            {
                AnsiConsole.MarkupLine("[yellow]No sessions found.[/]");
                return;
            }
            Table table = new();
            table.AddColumns("Id", "Start Time", "End Time", "Duration");
            foreach (CodingSession session in sessions)
            {
                table.AddRow(session.Id.ToString(), session.StartTime.ToString(), session.EndTime.ToString(), session.Duration.ToString());
            }
            AnsiConsole.Write(table);
        }

        internal void AddSession()
        {
            CodingSession session = new();
            DateTime startTime = Helpers.GetDateTimeInput("Please input your session starting date and time: [green](Format: dd-mm-yy HH:mm:ss)[/].");
            DateTime endTime = Helpers.GetDateTimeInput("Please input your session ending date and time: [green](Format: dd-mm-yy HH:mm:ss)[/].");

            if (!Validation.ValidateTimeSpan(startTime, endTime))
                return;

            if (!Validation.ValidateOverlappingTime(db.GetOverlappedSession(startTime, endTime)))
                return;

            TimeSpan duration = Helpers.CalculateDuration(startTime, endTime);
            session.StartTime = startTime;
            session.EndTime = endTime;
            session.Duration = duration;

            db.Insert(session);
        }

        internal void UpdateSession()
        {
            AnsiConsole.Clear();
            ViewAllSession();
            int sessionId = Helpers.GetNumberInput("Please the session id you want to update: ");
            CodingSession session = db.GetSessionById(sessionId);
            if (session == null)
            {
                AnsiConsole.MarkupLine($"[red]Session with id {sessionId} not found. Returning to main menu[/]");
                return;
            }
            DateTime startTime = Helpers.GetDateTimeInput("Please input your session starting date and time: [green](Format: dd-mm-yy HH:mm:ss)[/].");
            DateTime endTime = Helpers.GetDateTimeInput("Please input your session ending date and time: [green](Format: dd-mm-yy HH:mm:ss)[/].");
            TimeSpan duration = Helpers.CalculateDuration(startTime, endTime);

            if (!Validation.ValidateTimeSpan(startTime, endTime))
                return;

            if (!Validation.ValidateOverlappingTime(db.GetOverlappedSession(startTime, endTime)))
                return;

            session.StartTime = startTime;
            session.EndTime = endTime;
            session.Duration = duration;
            db.Update(session);
            AnsiConsole.Clear();
        }

        internal void DeleteSession()
        {
            AnsiConsole.Clear();
            ViewAllSession();
            int sessionId = Helpers.GetNumberInput("Please the session id you want to delete: ");
            CodingSession session = db.GetSessionById(sessionId);
            if (session == null)
            {
                AnsiConsole.MarkupLine($"[red]Session with id {sessionId} not found. Returning to main menu[/]");
                return;
            }
            db.Delete(session);
            AnsiConsole.Clear();
        }
    }
}
