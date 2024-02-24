using Spectre.Console;
using System.Diagnostics;

namespace CodingTracker.Dejmenek.Services
{
    public class TrackTimeService
    {
        private readonly Stopwatch _timer;

        public TrackTimeService()
        {
            _timer = new Stopwatch();
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Run(ref bool shouldStop)
        {
            Start();

            while (!shouldStop)
            {
                AnsiConsole.MarkupLine(GetElapsedTime().ToString(@"hh\:mm\:ss"));
                Thread.Sleep(1000);
                AnsiConsole.Clear();
            }

            Stop();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Reset()
        {
            _timer.Reset();
        }

        public TimeSpan GetElapsedTime()
        {
            return _timer.Elapsed;
        }

        public int GetElapsedTimeInMinutes()
        {
            return (int)GetElapsedTime().TotalMinutes;
        }
    }
}
