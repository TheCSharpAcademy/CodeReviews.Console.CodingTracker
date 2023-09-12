using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CodingTracker
{
    partial class Tracker
    {

        public string InputDate()
        {
            string correctDate = "";
            bool wrongDate = true;

            Regex regex = new Regex("^[0-9][0-9]-[0-9][0-9]-[0-9][0-9][0-9][0-9]$", RegexOptions.IgnoreCase);

            while (wrongDate)
            {
                while (!regex.IsMatch(correctDate))
                {
                    Console.WriteLine("Please enter date in format dd-mm-yyyy: ");
                    correctDate = Console.ReadLine()!;
                }
                string[] dateList = correctDate.Split('-');
                int day = int.Parse(dateList[0]);
                int month = int.Parse(dateList[1]);
                int year = int.Parse(dateList[2]);

                if ((day<=31) &&(month <=12) &&(year > 2000 && year < 2024))
                {
                    wrongDate = false;
                }
                else
                {
                    correctDate = "";
                }
            }

            return correctDate;
        }

        public string InputStartTime()
        {
            string startTime = "";
            bool wrongDate = true;

            Regex regex = new Regex("^[0-9][0-9]:[0-9][0-9]$", RegexOptions.IgnoreCase);

            while (wrongDate)
            {
                while (!regex.IsMatch(startTime))
                {
                    Console.WriteLine("Start time (hh:mm): ");
                    startTime = Console.ReadLine()!;
                }
                string[] dateList = startTime.Split(':');
                int hours = int.Parse(dateList[0]);
                int minutes = int.Parse(dateList[1]);

                if ((hours <= 24) && (minutes <= 60))
                {
                    wrongDate = false;
                }
                else
                {
                    startTime = "";
                }
            }

            return startTime;
        }

        public string InputEndTime()
        {
            string endTime = "";
            bool wrongDate = true;

            Regex regex = new Regex("^[0-9][0-9]:[0-9][0-9]$", RegexOptions.IgnoreCase);

            while (wrongDate)
            {
                while (!regex.IsMatch(endTime))
                {
                    Console.WriteLine("End time (hh:mm): ");
                    endTime = Console.ReadLine()!;
                }
                string[] dateList = endTime.Split(':');
                int hours = int.Parse(dateList[0]);
                int minutes = int.Parse(dateList[1]);

                if ((hours <= 24) && (minutes <= 60))
                {
                    wrongDate = false;
                }
                else
                {
                    endTime = "";
                }
            }

            return endTime;
        }

        public string[] InputCorrectStartAndEndTime()
        {
            bool wrongTime = true;
            string startTime = "";
            string endTime = "";
            while (wrongTime)
            {
                startTime = InputStartTime();
                endTime = InputEndTime();
                int minutes = TimeToMinutes(startTime, endTime);

                if (minutes > 0)
                {
                    wrongTime = false;
                }
                else
                {
                    Console.WriteLine("Coding time cannot be smaller than 1 minute.");
                }
            }

            string[] output = { startTime, endTime };

            return output;
        }

        public int TimeToMinutes(string startTime, string endTime)
        {
            int minutes = 0;

            string[] startList = startTime.Split(':');
            int startH = int.Parse(startList[0]);
            int startM = int.Parse(startList[1]);

            string[] endList = endTime.Split(':');
            int endH = int.Parse(endList[0]);
            int endM = int.Parse(endList[1]);

            minutes = ((endH - startH) * 60) + (endM - startM);

            return minutes;
        }

        public int InputNumber()
        {
            int number = 0;
            bool wrongNumber = true;

            while (wrongNumber)
            {
                try
                {
                    number = int.Parse(Console.ReadLine()!);
                    wrongNumber = false;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Please input numbers only.");
                }
            }

            return number;
        }
    }
}
