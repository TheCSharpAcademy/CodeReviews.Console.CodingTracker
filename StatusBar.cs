using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace CodingTracker
{
    public class StatusBar
    {
        private int _maximumValue;
        private TimeSpan _currentTime;
        private TimeSpan _goalTime;
        public StatusBar(int maximumValue, TimeSpan currentTime, TimeSpan goalTime)
        {
            _maximumValue = maximumValue;
            _currentTime = currentTime;
            _goalTime = goalTime;

        }
        private int CalculateValue()
        {
            decimal currentTicks = _currentTime.Ticks;
            decimal goalTicks = _goalTime.Ticks;

            return (int)Math.Round(_maximumValue * (currentTicks / goalTicks));
        }
        public string Display()
        {
            var statusBarString = new StringBuilder();
            int currentValue = CalculateValue();
            if (currentValue == _maximumValue)
                currentValue -= 1; // so the bar is never full, if the goal time isn't really passed

            statusBarString.Append("[yellow]");
            for (int i = 0; i < currentValue; i++)
            {
                statusBarString.Append('█');
            }
            statusBarString.Append("[/]");

            return statusBarString.ToString();
        }
    }
}
