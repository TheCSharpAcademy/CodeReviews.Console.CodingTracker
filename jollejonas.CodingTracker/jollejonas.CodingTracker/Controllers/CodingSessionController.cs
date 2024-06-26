using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Spectre.Console;
using jollejonas.CodingTracker.Models;
using jollejonas.CodingTracker.Data;
using jollejonas.CodingTracker.Utilities;
using System.Globalization;
using System.Diagnostics;

namespace jollejonas.CodingTracker.Controllers
{
    public class CodingSessionController
    {
        private readonly IDbConnection _db;
        private readonly GoalController goalController;

        public CodingSessionController(IDbConnection db)
        {
            _db = db;
            goalController = new GoalController(_db);
        }

        public void SelectOperation(int id)
        {
            switch (id)
            {
                case 1:
                    DisplayCodingSessions(GetCodingSessions());
                    break;
                case 2:
                    CreateNewSession();
                    break;
                case 3:
                    CreateNewLiveSession();
                    break;
                case 4:
                    UpdateSession();
                    break;
                case 5:
                    DeleteSession();
                    break;
                case 6:
                    FilterAndDisplaySessions();
                    break;
                case 7:
                    goalController.SetGoal();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        static double CalculateDuration(DateTime startTime, DateTime endTime)
        {

            return (double)(endTime - startTime).TotalHours;
        }
        public List<CodingSession> GetCodingSessions(DateTime? startDate = null, DateTime? endDate = null, bool ascending = true)
        {
            var sql = "SELECT * FROM CodingSessions WHERE 1=1";

            if (startDate.HasValue)
            {
                sql += " AND StartTime >= @StartDate";
            }

            if (endDate.HasValue)
            {
                sql += " AND EndTime <= @EndDate";
            }

            sql += ascending ? " ORDER BY StartTime ASC" : " ORDER BY StartTime DESC";

            return _db.Query<CodingSession>(sql, new { StartDate = startDate, EndDate = endDate }).ToList();

        }

        public CodingSession GetCodingSession(int id)
        {
            var sql = "SELECT * FROM CodingSessions WHERE Id = @Id";
            return _db.QueryFirstOrDefault<CodingSession>(sql, new { Id = id });
        }
        public static void DisplayCodingSessions(List<CodingSession> codingSessions)
        {
            var table = new Table();
            table.AddColumn("Id");
            table.AddColumn("Start time");
            table.AddColumn("End time");
            table.AddColumn("Duration");

            foreach (var session in codingSessions)
            {

                table.AddRow(session.Id.ToString(), session.StartTime.ToString(), session.EndTime.ToString(), session.Duration.ToString("F2"));
                
            }
            if (codingSessions.Count > 0)
            {
                table.AddRow("", "", "Total:", codingSessions.Sum(x => x.Duration).ToString("F2"));
                table.AddRow("", "", "Average:", codingSessions.Average(x => x.Duration).ToString("F2"));
            }
            else
            {
                table.AddRow("No sessions found");
            }
            AnsiConsole.Write(table);
        }

        public void FilterAndDisplaySessions()
        {
            var periodSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose a period to filter by:")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Last week", "Specific week", "Last X weeks", "Last month", "Last year"
                    }));

            DateTime now = DateTime.Now;
            DateTime? startDate = null;
            DateTime? endDate = null;

            switch (periodSelection)
            {
                case "Last week":
                    startDate = now.AddDays(-7);
                    endDate = now;
                    break;
                case "Specific week":
                    Console.WriteLine("Enter the year of the week: ");
                    string? yearInput = Console.ReadLine();
                    int year = Validation.CheckYear(yearInput);

                    Console.WriteLine("Enter the week number: ");
                    string? weekNumberInput = Console.ReadLine();
                    int weekNumber = Validation.CheckWeekNumber(weekNumberInput);

                    var jan1 = new DateTime(year, 1, 1);
                    var startOfWeek = jan1.AddDays((weekNumber - 1) * 7);
                    while (startOfWeek.DayOfWeek != DayOfWeek.Monday)
                    {
                        startOfWeek = startOfWeek.AddDays(-1);
                    }
                    startDate = startOfWeek;
                    endDate = startOfWeek.AddDays(7);
                    break;
                case "Last X weeks":
                    Console.WriteLine("Enter the number of weeks: ");
                    string? weeksInput = Console.ReadLine();
                    int weeks = Validation.CheckWeeks(weeksInput);
                    startDate = now.AddDays(-7 * weeks);
                    endDate = now;
                    break;
                case "Last month":
                    startDate = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
                    endDate = new DateTime(now.Year, now.Month, 1).AddDays(-1);
                    break;
                case "Last year":
                    startDate = new DateTime(now.Year - 1, 1, 1);
                    endDate = new DateTime(now.Year - 1, 12, 31);
                    break;
            }

            var orderSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose the sorting order:")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "Ascending", "Descending"
                    }));

            bool ascending = orderSelection == "Ascending";

            var sessions = GetCodingSessions(startDate, endDate, ascending);
            DisplayCodingSessions(sessions);
        }
        public void CreateNewSession()
        {
            string startTimeInput;
            string endTimeInput;

            while (true)
            {
                Console.WriteLine("Enter start time (dd-MM-yyyy HH:mm): ");
                startTimeInput = Console.ReadLine();
                string validationMessage = Validation.CheckStartTime(startTimeInput);
                if (validationMessage == null)
                {
                    break;
                }
                Console.WriteLine(validationMessage);
            }

            while (true)
            {
                Console.WriteLine("Enter end time (dd-MM-yyyy HH:mm): ");
                endTimeInput = Console.ReadLine();
                string validationMessage = Validation.CheckEndTime(endTimeInput, startTimeInput);
                if (validationMessage == null)
                {
                    break;
                }
                Console.WriteLine(validationMessage);
            }

            DateTime startTime = DateTime.ParseExact(startTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(endTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            double duration = CalculateDuration(startTime, endTime);

            string insertDataQuery = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Duration)
                VALUES (@StartTime, @EndTime, @Duration)";

            _db.Execute(insertDataQuery, new
            {
                StartTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndTime = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                Duration = duration
            });
        }

        public void CreateNewLiveSession()
        {
            Stopwatch stopwatch = new Stopwatch();
            Console.WriteLine("Press any key to start the session");
            Console.ReadKey();
            Console.Clear();

            stopwatch.Start();
            DateTime startTime = DateTime.Now;

            Console.CursorVisible = false;

            Console.WriteLine("Press any key to end the session");

            int originalRow = Console.CursorTop;
            while (!Console.KeyAvailable)
            {
                TimeSpan elapsed = stopwatch.Elapsed; 
                
                Console.SetCursorPosition(0, originalRow + 1);

                Console.WriteLine("Coding time: {0:hh\\:mm\\:ss\\.fff}", elapsed);
                Thread.Sleep(100);
            }

            stopwatch.Stop();
            DateTime endTime = DateTime.Now;

            TimeSpan ts = stopwatch.Elapsed;
            string elapsedTime = String.Format("[green]{0:00}:{1:00}:{2:00}",
                ts.Hours, ts.Minutes, ts.Seconds);

            Console.Clear();
            Console.WriteLine($"Session runtime {elapsedTime}");
            Console.ReadKey();

            Console.CursorVisible = true;

            double duration = CalculateDuration(startTime, endTime);

            string insertDataQuery = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Duration)
                VALUES (@StartTime, @EndTime, @Duration)";

            _db.Execute(insertDataQuery, new
            {
                StartTime = startTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                EndTime = endTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                Duration = duration
            });
        }

        public void UpdateSession()
        {
            List<CodingSession> sessions = GetCodingSessions();
            DisplayCodingSessions(sessions);
            Console.WriteLine("All sessions: ");
            Console.WriteLine("Enter the id of the session you want to update: ");
            string idInput = Console.ReadLine();
            int id = Validation.CheckId(idInput);
            while(sessions.All(x => x.Id != id))
            {
                Console.WriteLine("Invalid id. Please enter a valid id.");
                idInput = Console.ReadLine();
                id = Validation.CheckId(idInput);
            }
            Console.Clear();

            CodingSession session = GetCodingSession(id);

            Console.WriteLine("Selected session:");
            Console.WriteLine($"Start time: {session.StartTime}");
            Console.Write("Select new start time or press enter to keep the current time: ");
            string newStartTimeInput = Console.ReadLine();
            
            while (true)
            {
                if (newStartTimeInput == "")
                {
                    newStartTimeInput = session.StartTime.ToString("dd-MM-yyyy HH:mm");
                    break;
                }
                string validationMessage = Validation.CheckStartTime(newStartTimeInput);
                if (validationMessage == null)
                {
                    break;
                }
                Console.WriteLine(validationMessage);

                Console.WriteLine("Enter start time (dd-MM-yyyy HH:mm): ");
                newStartTimeInput = Console.ReadLine();
            }
            DateTime newStartTime = DateTime.ParseExact(newStartTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            Console.WriteLine($"End time: {session.EndTime}");
            Console.Write("Select new end time or press enter to keep the current time: ");
            string newEndTimeInput = Console.ReadLine();
            while (true)
            {
                if(newEndTimeInput == "")
                {
                    newEndTimeInput = session.EndTime.ToString("dd-MM-yyyy HH:mm");
                    break;
                }
                string validationMessage = Validation.CheckEndTime(newEndTimeInput, newStartTimeInput);
                if (validationMessage == null)
                {
                    break;
                }
                Console.WriteLine(validationMessage);

                Console.WriteLine("Enter end time (dd-MM-yyyy HH:mm): ");
                newEndTimeInput = Console.ReadLine();
            }

            DateTime newEndTime = DateTime.ParseExact(newEndTimeInput, "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
                        
            string updateDataQuery = @"
                UPDATE CodingSessions
                SET StartTime = @StartTime, EndTime = @EndTime, Duration = @Duration
                WHERE Id = @Id";

            _db.Execute(updateDataQuery, new CodingSession
            {
                Id = id,
                StartTime = newStartTime,
                EndTime = newEndTime,
                Duration = CalculateDuration(newStartTime, newEndTime)
            });

            Console.WriteLine("Session updated");
        }
        public void DeleteSession()
        {
            List<CodingSession> sessions = GetCodingSessions();
            Console.WriteLine("All sessions: ");
            DisplayCodingSessions(GetCodingSessions());
            Console.WriteLine("Enter the id of the session you want to delete: ");
            string idInput = Console.ReadLine();
            int id = Validation.CheckId(idInput);
            while (sessions.All(x => x.Id != id))
            {
                Console.WriteLine("Invalid id. Please enter a valid id.");
                idInput = Console.ReadLine();
                id = Validation.CheckId(idInput);
            }
            string deleteDataQuery = @"
                DELETE FROM CodingSessions
                WHERE Id = @Id";

            _db.Execute(deleteDataQuery, new { Id = id });

            Console.WriteLine("Sessions deleted.");
        }


        public void SeedData(IDbConnection db)
        {
            var sql = "SELECT * FROM CodingSessions";
            var codingSessions = db.Query<CodingSession>(sql).ToList();
            if (codingSessions.Count <= 0)
            {
                string insertDataQuery = @"
                INSERT INTO CodingSessions (StartTime, EndTime, Duration)
                VALUES (@StartTime, @EndTime, @Duration)";

                var seedData = new List<CodingSession>
            {
            new CodingSession
            {
                StartTime = DateTime.Now.AddHours(-1),
                EndTime = DateTime.Now,
                Duration = 1
            },
            new CodingSession
            {
                StartTime = DateTime.Now.AddHours(-2),
                EndTime = DateTime.Now.AddHours(-1),
                Duration = 1
            },
            new CodingSession
            {
                StartTime = DateTime.Now.AddHours(-3),
                EndTime = DateTime.Now.AddHours(-2),
                Duration = 1
            }
        };

                db.Execute(insertDataQuery, seedData);
            }

        }
    }
}
