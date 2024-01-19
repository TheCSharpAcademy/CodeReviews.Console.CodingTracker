using System.Diagnostics;

namespace Library
{
    public class StopwatchService
    {
        private readonly Stopwatch stopwatch;

        public StopwatchService() 
        {
            stopwatch = new Stopwatch();
        }

        public Stopwatch StartStopwatch()
        {
            stopwatch.Start();
            return stopwatch;
        }

        public void StopStopwatch()
        {
            stopwatch.Stop();
        }

        public void ResetStopwatch()
        {
            stopwatch?.Reset();
        }

        public TimeSpan GetElapsed()
        {
            return stopwatch.Elapsed;
        }
    }
}
