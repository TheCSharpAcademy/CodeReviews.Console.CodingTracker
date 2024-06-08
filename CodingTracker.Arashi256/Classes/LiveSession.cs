using System.Diagnostics;
using Spectre.Console;

namespace CodingTracker.Arashi256.Classes
{
    public class LiveSession
    {
        private readonly Stopwatch _stopwatch;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Task _displayTask;
        private bool _stopwatchRunning;
        private DateTime _startTime;
        public event Action<DateTime, DateTime, TimeSpan> StopwatchStopped;

        public LiveSession()
        {
            _stopwatch = new Stopwatch();
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public void Start()
        {
            _startTime = DateTime.Now;
            _stopwatch.Start();
            _stopwatchRunning = true;
            _displayTask = Task.Run(UpdateDisplayAsync);
            Task.Run(HandleInputAsync); // Start a task to handle user input asynchronously
        }

        public void Stop()
        {
            _stopwatchRunning = false;
            _cancellationTokenSource.Cancel();
            _stopwatch.Stop();
            _displayTask.Wait();  // Ensure the display task has completed

            var elapsedTime = _stopwatch.Elapsed;
            var endTime = _startTime.Add(elapsedTime);

            // Trigger the event with the start time, end time, and elapsed time
            StopwatchStopped?.Invoke(_startTime, endTime, elapsedTime);
        }

        private void UpdateDisplayAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (_stopwatchRunning)
                {
                    AnsiConsole.Cursor.SetPosition(0, 0);
                    AnsiConsole.Render(new Panel($"[white]Time: {_stopwatch.Elapsed:hh\\:mm\\:ss\\.fff}[/]"));
                }
                Thread.Sleep(100);
            }
        }

        private async Task HandleInputAsync()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true); // Read the key without displaying it
                    if (key.Key != ConsoleKey.Escape) // Check if the pressed key is not the Escape key
                    {
                        Stop();
                        break;
                    }
                }

                await Task.Delay(100);
            }
        }
    }
}
