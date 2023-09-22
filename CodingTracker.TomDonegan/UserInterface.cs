using ConsoleTableExt;
using Microsoft.SqlServer.Server;
using Microsoft.VisualBasic.FileIO;

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

            TableBuilder(tableData, "CODING TRACKER", TextAligntment.Center);

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

            TableBuilder(tableData, "Main Menu", TextAligntment.Left);

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
            // No options in here. Will just show historic session data.
            throw new NotImplementedException();
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


            Console.WriteLine("Please enter a start and end time: (HH:MM)");
            string sessionStartTime;
            string sessionEndTime;

            do
            {
                Console.Write("Start time: ");
                sessionStartTime = Console.ReadLine();
                Console.Write("End time: ");
                sessionEndTime = Console.ReadLine();

                if (!Validation.TimeEntryValidation(sessionStartTime) || !Validation.TimeEntryValidation(sessionEndTime))
                {
                    Console.WriteLine(
                    "Invalid input. Please enter the time in HH:MM format and within the valid range (HH: 00-23, MM: 00-59). Try again."
                );
                }
            } while (!Validation.TimeEntryValidation(sessionStartTime) || !Validation.TimeEntryValidation(sessionEndTime));

            /*while (!Validation.TimeEntryValidation(sessionStartTime))
            {
                Console.WriteLine("Invalid input. Please enter the time in HH:MM format and within the valid range (HH: 00-23, MM: 00-59). Try again.");
                sessionStartTime = Console.ReadLine();
            }

            Console.WriteLine("Please enter a start time: (HH:MM)");
            string sessionEndTime = Console.ReadLine();

            while (!Validation.TimeEntryValidation(sessionEndTime))
            {
                Console.WriteLine("Invalid input. Please enter the time in HH:MM format and within the valid range (HH: 00-23, MM: 00-59). Try again.");
                sessionEndTime = Console.ReadLine();
            }*/

            CodingSession session =
                new()
                {
                    date = sessionDate,
                    startTime = sessionStartTime,
                    endTime = sessionEndTime
                };

            if (
                DateTime.TryParse(session.startTime, out DateTime startTime)
                && DateTime.TryParse(session.endTime, out DateTime endTime)
            )

                session.duration = Helpers.CalculateDuration(startTime, endTime);

            Database.AddEntrySQLiteDatabase(session);
        }

        static void TableBuilder(
            List<List<object>> tableData,
            string title,
            TextAligntment alignment
        )
        {
            ConsoleTableBuilder
                .From(tableData)
                .WithTitle(title)
                .WithTextAlignment(new Dictionary<int, TextAligntment> { { 0, alignment } })
                .WithFormat(ConsoleTableBuilderFormat.Minimal)
                .ExportAndWriteLine(TableAligntment.Center);
        }
    }
}
