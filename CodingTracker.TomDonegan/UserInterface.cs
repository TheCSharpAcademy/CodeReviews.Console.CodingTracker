using ConsoleTableExt;

namespace CodingTracker.TomDonegan
{
    internal static class UserInterface
    {
        internal static void WelcomeScreen()
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

            Helpers.TableBuilder(tableData, "CODING TRACKER", TextAligntment.Center, "menuTable");

            Console.ReadLine();
            Console.Clear();
            MainMenu();
        }

        internal static void MainMenu()
        {
            Console.Clear();
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

            Helpers.TableBuilder(tableData, "Main Menu", TextAligntment.Left, "menuTable");

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
                    UpdateSessionInterface("update");
                    break;
                case "3":
                    UpdateSessionInterface("delete");
                    break;
                case "4":
                    ViewAllSessions();
                    break;
            }
        }

        private static List<CodingSession> ViewAllSessions(string viewReason = "overview")
        {
            Console.Clear();
            try
            {
                List<CodingSession> sessions = Database.ViewAllSQLiteDatabase();

                List<List<object>> sessionTable = new List<List<object>>();

                TimeSpan totalCodingTime = TimeSpan.Zero;

                foreach (CodingSession session in sessions)
                {
                    List<object> newRow = new List<object>
                    {
                        session.Id,
                        session.Date,
                        session.StartTime,
                        session.EndTime,
                        session.Duration
                    };

                    if (TimeSpan.TryParse(session.Duration, out TimeSpan sessionDuration))
                    {
                        totalCodingTime = totalCodingTime.Add(sessionDuration);
                    }

                    sessionTable.Add(newRow);
                }

                string formattedTotalCodingTime =
                    $"{(int)totalCodingTime.TotalHours:D2}:{totalCodingTime.Minutes:D2}";

                List<object> totalDurationRow = new List<object>
                {
                    "",
                    "",
                    "",
                    "Total Duration:",
                    formattedTotalCodingTime,
                };

                sessionTable.Add(totalDurationRow);

                List<string> columnHeaders = new List<string>
                {
                    "ID",
                    "Date",
                    "Start Time",
                    "End Time",
                    "Duration",
                };

                Helpers.TableBuilder(
                    sessionTable,
                    "All Coding Sessions",
                    TextAligntment.Center,
                    "dataTable",
                    columnHeaders
                );

                if (viewReason == "overview")
                {
                    Helpers.WaitForUserInput("Press Enter to return to the Main Menu.");
                }
                return sessions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"All session data could not be read from the database. Reason: {ex}"
                );
                return null;
            }
        }

        private static void DisplayEditSessionMenu(string title, string editSelection)
        {
            var tableData = new List<List<object>>
            {
                new List<object> { "" },
                new List<object> { },
                new List<object> { $"Here you can {editSelection} a previous session." },
                new List<object> { },
                new List<object>
                {
                    "Press 'Enter' to view sessions or '0' to return to the Main Menu."
                },
                new List<object> { },
            };

            Helpers.TableBuilder(tableData, title, TextAligntment.Center, "menuTable");
        }

        private static void UpdateSessionInterface(string updateOrDelete)
        {
            Console.Clear();

            switch (updateOrDelete)
            {
                case "update":
                    DisplayEditSessionMenu("UPDATE SESSION", "update");
                    break;
                case "delete":
                    DisplayEditSessionMenu("DELETE SESSION", "delete");
                    break;
            }

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            if (keyInfo.KeyChar == '0')
            {
                MainMenu();
                return;
            }

            if (keyInfo.KeyChar == '\r')
            {
                ViewAllSessions("nonoverview");
                string selectedSessionId = Helpers.GetUserInput(
                    $"Please select a session ID to {updateOrDelete}: "
                );

                while (!Validation.DoesIdExistInTable(selectedSessionId))
                {
                    Console.WriteLine(
                        $"Record No.{selectedSessionId} does not exist in the table. Please try again."
                    );
                    selectedSessionId = Helpers.GetUserInput(
                        $"Please select a session ID to {updateOrDelete}: "
                    );
                }

                Console.WriteLine(
                    $"Confirm {updateOrDelete} on session: {selectedSessionId}? (Y/N)"
                );

                string editConfirmation = Console.ReadLine();

                while (!Validation.YesNoConfirm(editConfirmation.ToLower()))
                {
                    Console.WriteLine(
                        $"{editConfirmation} is not a valid selection, please try again."
                    );
                    editConfirmation = Console.ReadLine();
                }

                if (editConfirmation.ToLower() == "y")
                {
                    if (updateOrDelete == "delete")
                    {
                        try
                        {
                            Database.DeleteEntrySQLiteDatabase(selectedSessionId);
                            Helpers.WaitForUserInput(
                                $"Session {selectedSessionId} successfully removed."
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            CodingSession updatedSession = CreateSession();
                            Database.UpdateEntrySQLiteDatabase(updatedSession, selectedSessionId);
                            Helpers.WaitForUserInput(
                                $"Session {selectedSessionId} successfully updated."
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    UpdateSessionInterface(updateOrDelete);
                }
            }
        }

        private static CodingSession CreateSession()
        {
            string sessionDate;
            string[] sessionTimes;

            try
            {
                sessionDate = GetDateInput();
                sessionTimes = GetTimeInput();

                if (sessionDate == null || sessionTimes == null)
                    throw new ArgumentNullException("sessionDate and sessionTimes cannot be null.");
            }
            catch (OperationCanceledException)
            {
                throw new Exception("Operation was canceled.");
            }

            CodingSession session =
                new()
                {
                    Date = sessionDate,
                    StartTime = sessionTimes[0],
                    EndTime = sessionTimes[1]
                };

            if (
                DateTime.TryParse(session.StartTime, out DateTime startTime)
                && DateTime.TryParse(session.EndTime, out DateTime endTime)
            )

                session.Duration = Helpers.CalculateDuration(startTime, endTime);

            return session;
        }

        private static void AddSessionInterface()
        {
            CodingSession session = CreateSession();
            try
            {
                Database.AddEntrySQLiteDatabase(session);
                Console.WriteLine(
                    $"\nSession: {session.Date} | Start time: {session.StartTime} | End time: {session.EndTime} | Total duration: {session.Duration}"
                );
                Helpers.WaitForUserInput("Session data added to the database.");
            }
            catch (Exception ex)
            {
                Helpers.WaitForUserInput(
                    $"Session data could not be written to the database. Reason: {ex}"
                );
            }
        }

        private static string GetDateInput()
        {
            Console.WriteLine(
                "Please enter the date for the session (DD-MM-YY). '0' key to return to the Main Menu."
            );

            string sessionDate;

            do
            {
                sessionDate = Console.ReadLine();

                if (sessionDate == "0")
                {
                    Console.Clear();
                    MainMenu();
                    throw new OperationCanceledException();
                }

                if (!Validation.DateEntryValidation(sessionDate))
                {
                    Console.WriteLine(
                        "Invalid input. Please enter the date in DD-MM-YY format and within the valid range (DD: 01-31, MM: 01-12). Try again."
                    );
                }
            } while (!Validation.DateEntryValidation(sessionDate));

            return sessionDate;
        }

        private static string[] GetTimeInput()
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
                    if (!Validation.IsSecondTimeBeforeFirstTime(startAndEndTimes))
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
    }
}
