using CodingTracker.CSharpAcademy_Learner.Controllers;
using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.CSharpAcademy_Learner
{
    internal class StopwatchService
    {
        private readonly CodingController _codingController;
        public StopwatchService(CodingController codingController)
        {
            _codingController = codingController;
        }
        internal void TrackSession()
        {

            if (!UserInterfaceHelpers.WaitForEnterOrEscape_CheckForEnter("Tracking live session"))
            {
                return;
            }

            UserInterfaceHelpers.WaitForSpecificKey(ConsoleKey.Enter, "Press [green]ENTER[/] to start tracking.");

            var stopwatch = Stopwatch.StartNew();
            var startTime = DateTime.Now;

            AnsiConsole.Status()
                .Start("Tracking session... Press [green]ENTER[/] to stop...", ctx =>
                {
                    while (!Console.KeyAvailable || Console.ReadKey(true).Key != ConsoleKey.Enter)
                    {
                        ctx.Status($"Tracking session... Elapsed time: {stopwatch.Elapsed:hh\\:mm\\:ss}... Press [green]ENTER[/] to stop...");
                        Thread.Sleep(500);
                    }
                });

            stopwatch.Stop();
            var endTime = DateTime.Now;

            var formattedStart = startTime.ToString(Validation.DatabaseDateFormat);
            var formattedEnd = endTime.ToString(Validation.DatabaseDateFormat);
            var duration = Validation.CalculateDurationInSeconds(startTime, endTime);

            AnsiConsole.MarkupLine($"STOPPED. Start time: {formattedStart}. End time: {formattedEnd}. Duration: {duration} seconds");

            if (AnsiConsole.Confirm("Do you want to save this session?"))
            {
                _codingController.InsertSession(formattedStart, formattedEnd, duration);
            }
            else
            {
                return;
            }

            Console.ReadKey();
        }
    }
}
