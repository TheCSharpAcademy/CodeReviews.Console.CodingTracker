using ConsoleTableExt;
using System.Globalization;

namespace CodingTracker
{
    internal static class Viewer
    {
        public static void DisplayPromptForTime(string verb)
        {
            Console.WriteLine($"\nWhen did you {verb} coding? Please answer below in the following format: dd-MM-yy HH-mm-ss");
        }

        public static void DisplayFilterOptionsMenu()
        {
            Console.WriteLine("\nChoose which records you want to view from the following list:\n");
            Console.WriteLine("\ta - all");
            Console.WriteLine("\td - today");
            Console.WriteLine("\tw - this week");
            Console.WriteLine("\tm - this month");
            Console.WriteLine("\ty - this year");
            Console.Write("\nYour option? ");
        }

        public static void DisplayOptionsMenu()
        {
            Console.WriteLine("\nChoose an action from the following list:\n");
            Console.WriteLine("\tv - View your tracker");
            Console.WriteLine("\ta - Add a new entry");
            Console.WriteLine("\td - Delete an entry");
            Console.WriteLine("\tu - Update an entry");
            Console.WriteLine("\tsrt - Start stopwatch");
            Console.WriteLine("\tstp - Stop stopwatch");
            Console.WriteLine("\t0 - Quit this application");
            Console.Write("\nYour option? ");
        }

        public static void DisplayTitle()
        {
            Console.WriteLine("Coding Tracker\r");
            Console.WriteLine("-------------\n");
        }

        public static string GetPretifiedTime(string time)
        {
            if (DateTime.TryParseExact(time, "dd-MM-yy HH-mm-ss", new CultureInfo("en-US"), DateTimeStyles.None, out DateTime parsedDate))
            {
                return $"{parsedDate.ToLongTimeString()} - {parsedDate.ToLongDateString()}";
            }
            else
            {
                return $"{time}";
            }
        }

        public static List<CodingSession> FilterSessions(List<CodingSession> allSessions, string filterStr)
        {
            return filterStr switch
            {
                "a" => allSessions,
                "d" => allSessions.Where(s => Validator.ConvertToDate(s.Start_Time).Day == DateTime.Now.Day).ToList(),
                "w" => allSessions.Where(s => Validator.ConvertToDate(s.Start_Time).Day >= DateTime.Now.Day - 7).ToList(),
                "m" => allSessions.Where(s => Validator.ConvertToDate(s.Start_Time).Month == DateTime.Now.Month).ToList(),
                "y" => allSessions.Where(s => Validator.ConvertToDate(s.Start_Time).Year == DateTime.Now.Year).ToList(),
                _ => allSessions,
            };
        }

        public static void ViewTable(List<CodingSession> sessions)
        {
            var tableData = new List<List<object>>();
            foreach (CodingSession codingSession in sessions)
            {
                tableData.Add(new List<object>
                {   codingSession.Id,
                    codingSession.Duration,
                    GetPretifiedTime(codingSession.Start_Time),
                    GetPretifiedTime(codingSession.End_Time)
                });
            }
            ConsoleTableBuilder.From(tableData).WithTitle("Your Coding Time").WithColumn("Id", "Duration", "Start Time", "End Time").ExportAndWriteLine();
        }
    }
}