using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CodingTracker
{
    public class StopwatchTimer
    {
        private Stopwatch stopwatch = new();
        private Thread _timerThread;
        private bool _stopWatchRuns;
        public TimeSpan Duration {get {return stopwatch.Elapsed;} }
        private bool _isPaused;
        public StopwatchTimer()
        {
            _timerThread = new Thread(StartTimer);
        }

        private void StartTimer()
        {

            _stopWatchRuns = true;
            string hoursElapsed;
            string minutesElapsed;
            string secondsElapsed;
            stopwatch.Start();

            while (_stopWatchRuns)
            {
                if (!_isPaused)
                {
                    hoursElapsed = stopwatch.Elapsed.Hours.ToString();
                    hoursElapsed = hoursElapsed.PadLeft(2,'0');

                    minutesElapsed = (stopwatch.Elapsed.Minutes % 60).ToString();
                    minutesElapsed = minutesElapsed.PadLeft(2,'0');

                    secondsElapsed = stopwatch.Elapsed.Seconds.ToString();
                    secondsElapsed = secondsElapsed.PadLeft(2,'0');

                    Console.ResetColor();
                    Console.SetCursorPosition(2, 2);
                    
                    Console.Write($"\r{hoursElapsed}:{minutesElapsed}:{secondsElapsed}");
                }
                Thread.Sleep(1000);
            }
        }

        public void Start()
        {
            if (!_timerThread.IsAlive)
                _timerThread.Start();
        }

        public void Stop()
        {
            _stopWatchRuns = false;
            stopwatch.Stop();
            if (_timerThread.IsAlive)
                _timerThread.Join();
        }

        public void Pause()
        {
            if (_timerThread.IsAlive && !_isPaused)
            {
                stopwatch.Stop();
                _isPaused = true;
            }
        }

        public void Resume()
        {
            if (_timerThread.IsAlive && _isPaused)
            {
                stopwatch.Start();
                _isPaused = false;
            }
        }

    }
}
