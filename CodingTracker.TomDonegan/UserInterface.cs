using ConsoleTableExt;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;

namespace CodingTracker.TomDonegan
{
    public static class UserInterface
    {
        public static void WelcomeScreen()
        {
            var tableData = new List<List<object>>
            {
                new List<object> { "Welcome to your Coding Tracker!" },
                new List<object> { },
                new List<object>
                {
                    "This app will help you to track the time you spend working on projects."
                },
                new List<object> { },
                new List<object>
                {
                    "Currently the app will only provide you with a coding duration."
                },
                new List<object> { },
                new List<object>
                {
                    "Future updates will allow you to assign your time to different projects"
                }
            };

            TableBuilder(tableData, "CODING TRACKER", TextAligntment.Center, "menuTable");

            Console.ReadLine();
            Console.Clear();
            MainMenu();
        }

        public static void MainMenu()
        {
            var tableData = new List<List<object>>
            {
                new List<object> { },
                new List<object> { "Please select one of the following options:" },
                new List<object> { },
                new List<object> { "1 - Add a coding session." },
                new List<object> { "2 - Update a session." },
                new List<object> { "3 - Delete a session." },
                new List<object> { "4 - View all session data." },
            };

            TableBuilder(tableData, "Main Menu", TextAligntment.Left, "menuTable");

            string[] selectionOptions = { "1", "2", "3", "4" };

            string menuSelection;
            bool isValidSelection;

            do
            {
                menuSelection = Console.ReadLine();
                isValidSelection = Validation.MenuValidation(selectionOptions, menuSelection);

                if (!isValidSelection)
                {
                    Console.WriteLine(
                        "Invalid input. Please enter a valid option (1, 2, 3, or 4)."
                    );
                }
            } while (!isValidSelection);

            switch (menuSelection)
            {
                case "1":
                    AddSessionInterface();
                    break;
                case "2":
                    UpdateSessionInterface();
                    break;
                case "3":
                    DeleteSessionInterface();
                    break;
                case "4":
                    ViewAllSessions();
                    break;
            }
        }

        private static void ViewAllSessions()
        {
            Console.Clear();

            List<CodingSession> sessions = Database.ViewAllSQLiteDatabase();

            List <List<object>> sessionTable = new List<List<object>>();
            

            foreach (CodingSession session in sessions)
            {
                List<object> newRow = new List<object>
                {
                    session.Id,
                    session.date,
                    session.startTime,
                    session.endTime,
                    session.duration
                };

                sessionTable.Add(newRow);
            }
            TableBuilder(sessionTable, "All Sessions", TextAligntment.Center, "dataTable");
            Console.ReadLine();
        }

        private static void DeleteSessionInterface()
        {
            // Select a session from its ID to delete.
            // Filtering woudl be required if db entries are high. By date input?
            throw new NotImplementedException();
        }

        private static void UpdateSessionInterface()
        {
            throw new NotImplementedException();
        }

        internal static void AddSessionInterface()
        {
            string sessionDate = GetDateInput();
            string[] sessionTimes = GetTimeInput();

            CodingSession session =
                new()
                {
                    date = sessionDate,
                    startTime = sessionTimes[0],
                    endTime = sessionTimes[1]
                };

            if (
                DateTime.TryParse(session.startTime, out DateTime startTime)
                && DateTime.TryParse(session.endTime, out DateTime endTime)
            )

                session.duration = Helpers.CalculateDuration(startTime, endTime);

            Database.AddEntrySQLiteDatabase(session);
        }

        internal static string GetDateInput()
        {
            Console.WriteLine("Please enter the date for the session (DD-MM-YY)");
            string sessionDate;

            do
            {
                sessionDate = Console.ReadLine();

                if (!Validation.DateEntryValidation(sessionDate))
                {
                    Console.WriteLine(
                        "Invalid input. Please enter the date in DD-MM-YY format and within the valid range (DD: 01-31, MM: 01-12). Try again."
                    );
                }
            } while (!Validation.DateEntryValidation(sessionDate));

            return sessionDate;
        }

        internal static string[] GetTimeInput()
        {
            Console.WriteLine("Please enter a start and end time: (HH:MM)");
            string[] startAndEndTimes = { "", "" };

            while (true)
            {
                Console.Write("Start time: ");
                startAndEndTimes[0] = Console.ReadLine();
                Console.Write("End time: ");
                startAndEndTimes[1] = Console.ReadLine();

                if (
                    Validation.TimeEntryValidation(startAndEndTimes[0])
                    && Validation.TimeEntryValidation(startAndEndTimes[1])
                )
                {
                    if (!Validation.isSecondTimeBeforeFirstTime(startAndEndTimes))
                    {
                        break;
                    }

                    Console.WriteLine("Your end time is a time before your start time.");
                    Console.WriteLine("Did you code past midnight? (Y/N)");

                    string endTimeConfirmation = Console.ReadLine();

                    while (endTimeConfirmation.Length < 1)
                    {
                        Console.WriteLine("Please make a selection. (Y/N)");
                        endTimeConfirmation = Console.ReadLine();
                    }

                    if (endTimeConfirmation.ToLower() == "y")
                    {
                        break;
                    }
                }

                Console.WriteLine(
                    "Invalid time input. Please enter the time in HH:MM format and within the valid range (HH: 00-23, MM: 00-59). Try again."
                );
            }

            return startAndEndTimes;
        }

        static void TableBuilder(
            List<List<object>> tableData,
            string title,
            TextAligntment alignment, string tableType
        )
        {
            if (tableType == "menuTable")
            {
                ConsoleTableBuilder
                .From(tableData)
                .WithTitle(title)
                .WithTextAlignment(new Dictionary<int, TextAligntment> { { 0, alignment } })
                .WithFormat(ConsoleTableBuilderFormat.Minimal)
                .ExportAndWriteLine(TableAligntment.Center);
            }
            else
            {
                ConsoleTableBuilder
                .From(tableData)
                .WithTitle(title)
                .WithTextAlignment(new Dictionary<int, TextAligntment> { { 0, alignment } })
                .WithFormat(ConsoleTableBuilderFormat.MarkDown)
                .ExportAndWriteLine(TableAligntment.Center);
            }           
        }
    }
}
