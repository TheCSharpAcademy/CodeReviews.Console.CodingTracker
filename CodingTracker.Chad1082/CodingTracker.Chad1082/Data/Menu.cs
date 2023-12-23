using CodingTracker.Chad1082.Models;

namespace CodingTracker.Chad1082.Data
{
    internal class Menu
    {
        internal void ShowMainMenu()
        {
            Console.WriteLine("Welcome to the Coding Tracker app!");
            do
            {
                MainMenu();
            } while (true);
        }

        private void MainMenu()
        {

            Console.WriteLine("******************************");
            Console.WriteLine("Please select an option below:");
            Console.WriteLine("A - Log new Session");
            Console.WriteLine("U - Update an existing session");
            Console.WriteLine("D - Delete a previous session");
            Console.WriteLine("V - View logged sessions");
            Console.WriteLine("0 - Exit the application");
            Console.WriteLine("******************************");

            string menuOption = Console.ReadLine().Trim().ToUpper();
            switch (menuOption)
            {
                case "A":
                    AddSession();
                    Console.Clear();
                    break;
                case "D":
                    DeleteSession();
                    Console.Clear();
                    break;
                case "U":
                    UpdateSession();
                    Console.Clear();
                    break;
                case "V":
                    ViewLoggedSessions();
                    Console.Clear();
                    break;
                case "0":
                    Environment.Exit(0);
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Option not recognised");
                    break;
            }
        }

        private void ViewLoggedSessions()
        {
            TableVisualisationEngine.DisplayAllSessions();

            Console.WriteLine("Press enter to continue....");
            Console.Read();
        }

        private void DeleteSession()
        {
            TableVisualisationEngine.DisplayAllSessions();
            string selection;
            int sessionID;
            UserInput.SessionSelect(out selection, out sessionID, Operation.Delete);

            if (selection.ToLower() == "q")
            {
                Console.WriteLine("Returning to main menu.");
                Console.WriteLine("Press enter to continue....");
                Console.Read();
            }
            else
            {
                if (CodingController.DeleteSession(sessionID))
                {
                    Console.WriteLine("Session deleted");
                    Console.WriteLine("Press enter to continue....");
                    Console.Read();
                }
                else
                {
                    Console.WriteLine("Session Not Found!");
                    DeleteSession();
                }
            }
        }

        private void AddSession()
        {
            Console.WriteLine("Log a coding session:\n\n");

            string startDateEntry, endDateEntry;
            UserInput.SessionInput(out startDateEntry, out endDateEntry);

            CodingController.AddSession(startDateEntry, endDateEntry);
            Console.WriteLine($"Recorded new session starting on {startDateEntry}. Press enter to continue....");
            Console.Read();
        }

        private void UpdateSession()
        {
            TableVisualisationEngine.DisplayAllSessions();

            string selection;
            int sessionID;
            UserInput.SessionSelect(out selection, out sessionID, Operation.Update);

            if (selection.ToLower() == "q")
            {
                Console.WriteLine("Returning to main menu.");
                Console.WriteLine("Press enter to continue....");
                Console.Read();
            }
            else
            {
                CodingSession session = CodingController.GetSingleSession(sessionID);

                if (session == null)
                {
                    Console.Clear();
                    Console.WriteLine("Entry Not Found!");
                    UpdateSession();
                }
                else
                {
                    TableVisualisationEngine.ShowTable(session);

                    string startDateEntry, endDateEntry;
                    UserInput.SessionInput(out startDateEntry, out endDateEntry);

                    CodingController.UpdateSession(sessionID, startDateEntry, endDateEntry);

                    Console.WriteLine("Entry Updated");
                    Console.WriteLine("Press enter to continue....");
                    Console.Read();
                }
            }
        }
    }

    internal enum Operation
    {
        Update,
        Delete
    }

}
