using CodingTracker.Data;
using CodingTracker.Enums;
using CodingTracker.Models;
using CodingTracker.UI;

namespace CodingTracker.Controllers
{
    internal class CodingTrackerController
    {
        internal void Run()
        {
            bool exitApp = false;
            while (!exitApp)
            {
                UserInput.ClearDisplay();
                UserInput.DisplayMainMenuFiglet();

                string mMenuOption = UserInput.GetMainMenuInput();

                switch (mMenuOption)
                {
                    case "Start Coding Session":
                        StartCodingSession();
                        break;
                    case "Manage Coding Sessions":
                        ManageSessions();
                        break;
                    case "Exit":
                        exitApp = true;
                        break;
                }
            }
        }

        private static void StartCodingSession()
        {
            DateTime startDate = DateTime.Now;
            DateTime endDate = DateTime.Now;
            TimeSpan duration = TimeSpan.Zero;
            if (!UserInput.Confirm("Are you sure you would like to start a coding session?"))
            {
                UserInput.Continue();
                return;
            }

            if (UserInput.GetTrackingType() == TrackingType.AutoTimer)
            {
                UserInput.DisplayMessage("The timer has started!");
                startDate = DateTime.Now;
                UserInput.ClearDisplay();
                UserInput.DisplayMessage("Press 'Enter' when you wish to stop the timer");
                UserInput.AwaitUserInput();
                endDate = DateTime.Now;
                duration = endDate - startDate;
            }

            else
            {
                (startDate, endDate, duration) = GetValidatedSessionTimeFrame();
            }

            CodingSession session = MapToCodingSession(startDate, endDate, duration);

            if (!UserInput.Confirm("Are you sure you would like to add this session to the tracker?"))
            {
                UserInput.Continue();
                return;
            }

            CodingSessionRepository.AddCodingSession(session);
            UserInput.ConfirmationMessage("Session has been added!");
            UserInput.Continue();
        }

        private static void ManageSessions()
        {
            bool exitToMainMenu = false;

            while (!exitToMainMenu)
            {
                string menuOption = UserInput.GetManageSessionMenuOption();

                switch (menuOption)
                {
                    case "View All Sessions":
                        ViewAllSessions(CodingSessionRepository.GetAllSessions());
                        break;
                    case "Delete Session":
                        DeleteSession();
                        break;
                    case "Update Session":
                        UpdateSession();
                        break;
                    case "Exit to Main Menu":
                        exitToMainMenu = true;
                        break;
                }
            }
        }

        private static void ViewAllSessions(List<CodingSession> sessions)
        {
            if (sessions.Count == 0)
            {
                UserInput.Error("There are no sessions to view.");
                UserInput.Continue();
                return;
            }

            TablePrinter.PrintSessionsTable(sessions);
            UserInput.Continue();
        }

        private static void DeleteSession()
        {
            List<int> sessionIds = CodingSessionRepository.GetAllSessionIds();

            if (sessionIds.Count == 0)
            {
                UserInput.Error("There are no sessions to delete.");
                UserInput.Continue();
                return;
            }

            int sessionId = UserInput.GetCodingSessionIdInput(sessionIds);
            if (!UserInput.Confirm($"Are you sure you would like to delete coding session {sessionId} from the tracker?"))
            {
                UserInput.ConfirmationMessage("Session deletetion cancelled.");
                UserInput.Continue();
                return;
            }

            ExecuteSessionDeletion(sessionId);
            UserInput.ConfirmationMessage("Session has been deleted!");
            UserInput.Continue();
        }

        private static void ExecuteSessionDeletion(int sessionId)
        {
            CodingSessionRepository.DeleteSessionFromId(sessionId);
        }

        private static void UpdateSession()
        {
            List<int> sessionIds = CodingSessionRepository.GetAllSessionIds();

            if (sessionIds.Count == 0)
            {
                UserInput.Error("There are no sessions to update.");
                UserInput.Continue();
                return;
            }

            int sessionId = UserInput.GetCodingSessionIdInput(sessionIds);
            CodingSession session = CodingSessionRepository.GetSessionFromId(sessionId);
            CodingSession updatedSession = GetCodingSessionUpdateInput(session);
            if (!UserInput.Confirm($"Are you sure you would like to update coding session {sessionId} with these details?"))
            {
                UserInput.ConfirmationMessage("Session update cancelled.");
                UserInput.Continue();
                return;
            }

            ExecuteUpdateSession(sessionId, updatedSession);
            UserInput.ConfirmationMessage("Session has been updated!");
            UserInput.Continue();
        }

        private static void ExecuteUpdateSession(int sessionId, CodingSession updatedSession)
        {
            CodingSessionRepository.UpdateSessionFromId(sessionId, updatedSession);
        }

        internal static CodingSession GetCodingSessionUpdateInput(CodingSession session)
        {
            DateTime startTime;
            DateTime endTime;
            TimeSpan duration;

            (startTime, endTime, duration) = GetValidatedSessionTimeFrame();


            return MapToCodingSession(startTime, endTime, duration, session.ID);
        }

        private static (DateTime start, DateTime end, TimeSpan duration) GetValidatedSessionTimeFrame()
        {
            while (true)
            {
                DateTime startTime = UserInput.GetDateTime("start");
                DateTime endTime = UserInput.GetDateTime("end");

                if (endTime > startTime)
                {
                    return (startTime, endTime, endTime - startTime);
                }

                UserInput.Error("Start date cannot be greater than end date! Please try again.");
                UserInput.Continue();
            }
        }

        private static CodingSession MapToCodingSession(DateTime start, DateTime end, TimeSpan duration, int Id = 0)
        {
            int totalHours = (int)duration.TotalHours;
            int minutes = duration.Minutes;

            return new CodingSession
            {
                ID = Id,
                StartTime = start.ToString("yyyy-MM-dd HH:mm"),
                EndTime = end.ToString("yyyy-MM-dd HH:mm"),
                Duration = $"{totalHours:D2}:{minutes:D2}"
            };
        }
    }
}
