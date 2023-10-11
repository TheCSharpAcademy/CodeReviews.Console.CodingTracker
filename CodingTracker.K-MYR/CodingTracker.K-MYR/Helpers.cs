using CodingTracker.K_MYR.Models;
using ConsoleTableExt;

namespace CodingTracker.K_MYR
{
    internal class Helpers
    {
        internal static List<CodingSession> PrintAllRecords()
        {
            var records = SQLiteOperations.SelectAllRecords();
            PrintRecords(records);

            return records;
        }

        internal static void PrintRecords(List<CodingSession> tableData, bool reverse = false)
        {
            Console.Clear();

            if (tableData.Count > 0)
            {
                tableData.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));

                if (reverse)
                    tableData.Reverse();

                ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle("Coding Sessions", ConsoleColor.Green, ConsoleColor.Black)
                    .WithTextAlignment(new Dictionary<int, TextAligntment>
                        {
                        {1, TextAligntment.Center },
                        {2, TextAligntment.Center },
                        {3, TextAligntment.Center },
                        })
                    .WithColumn("ID", "Start", "End", "Duration")
                    .ExportAndWriteLine();
            }

            else
            {
                Console.WriteLine("Now records were found!");
            }
        }

        internal static List<CodingSession> GetRecords(string unit, int timespanNumber)
        {
            var records = SQLiteOperations.SelectAllRecords();
            TimeSpan period = new();
            List<CodingSession> tableData = new();

            try
            {
                switch (unit)
                {
                    case "d":
                        period = TimeSpan.FromDays(timespanNumber);
                        break;
                    case "w":
                        period = TimeSpan.FromDays(timespanNumber * 7);
                        break;
                    case "y":
                        period = TimeSpan.FromDays(timespanNumber * 365);
                        break;
                }

                tableData = records.Where(x => x.StartTime >= DateTime.Now.Subtract(period)).ToList();
            }

            catch (OverflowException)
            {
                tableData = records;
            }

            catch (ArgumentOutOfRangeException)
            {
                tableData = records;
            }

            return tableData;
        }

        internal static void PrintStopwatchMenu(string startDate = "", string endDate = "", string duration = "")
        {
            Console.Clear();
            Console.WriteLine("| Track your coding session via stopwatch |");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine($"Start Time: {startDate}");
            Console.WriteLine($"End Time: {endDate}");
            Console.WriteLine($"Duration: {duration}");
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("0 - Start/Stop the timer");
            Console.WriteLine("1 - Save Session");
            Console.WriteLine("2 - Return to main menu");
            Console.WriteLine("-------------------------------------------");
        }

        internal static void PrintAllGoals()
        {
            Console.Clear();

            var tableData = SQLiteOperations.SelectAllGoals();
            PrintGoals(tableData);
        }

        internal static void PrintGoals(List<CodingGoal> tableData, bool reverse = false)
        {
            Console.Clear();

            if (tableData.Count > 0)
            {
                tableData.Sort((x, y) => DateTime.Compare(x.Deadline, y.Deadline));

                if (reverse)
                    tableData.Reverse();

                ConsoleTableBuilder
                    .From(tableData)
                    .WithTitle("Coding Sessions", ConsoleColor.Green, ConsoleColor.Black)
                    .WithTextAlignment(new Dictionary<int, TextAligntment>
                        {
                        {1, TextAligntment.Center },
                        {2, TextAligntment.Center },
                        {3, TextAligntment.Center },
                        {4, TextAligntment.Center },
                        {5, TextAligntment.Center },
                        {6, TextAligntment.Center }
                        })
                    .WithColumn("ID", "Name", "Start", "End", "Goal", "Accumulated", "%", "Hours per Day")
                    .ExportAndWriteLine();
            }

            else
            {
                Console.WriteLine("Now goals were found!");
            }
        }

        internal static List<CodingGoal> GetActiveGoals()
        {
            var activeGoals = SQLiteOperations.SelectAllGoals()
                                              .Where(x => x.Deadline.Date > DateTime.Now.Date && x.StartDate.Date <= DateTime.Now.Date)
                                              .ToList();
            return activeGoals;
        }

        internal static TimeSpan CalculateElapsedTime(DateTime goalStartDate, DateTime goalEndDate)
        {
            var records = SQLiteOperations.SelectAllRecords()
                                          .Where(x => x.StartTime.Date >= goalStartDate.Date && x.StartTime < goalEndDate.Date)
                                          .ToList();

            TimeSpan elapsedTime = new(records.Sum(x => x.Duration.Ticks));
            return elapsedTime;
        }

        internal static void AdjustElapsedTime(DateTime recordStartDate)
        {
            var goals = SQLiteOperations.SelectAllGoals()
                                        .Where(x => x.StartDate.Date <= recordStartDate.Date && x.Deadline.Date > recordStartDate.Date)
                                        .ToList();
            foreach (var goal in goals)
            {
                TimeSpan elapsedTime = CalculateElapsedTime(goal.StartDate, goal.Deadline);
                SQLiteOperations.UpdateGoalElapsedTime(goal.Id, elapsedTime.ToString("dd\\:hh\\:mm\\:ss"));
            }
        }

    }
}