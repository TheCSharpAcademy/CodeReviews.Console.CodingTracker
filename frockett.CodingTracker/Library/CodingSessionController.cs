using Spectre.Console;
using frockett.CodingTracker.Library;

namespace Library
{
    public class CodingSessionController
    {
        private readonly IDbMethods dbMethods;
        private readonly UserInputValidationService userInteractionService;

        public CodingSessionController(IDbMethods dbMethods, UserInputValidationService ui)
        {
            this.dbMethods = dbMethods;
            userInteractionService = ui;
        }

        public void InsertCodingSession()
        {
            dbMethods.InsertCodingSession(userInteractionService.GetStartEndTimeInput());
        }

        public void UpdateCodingSession()
        {
            // TODO print sessions so user can see the IDs

            int sessionIdToUpdate = userInteractionService.GetSessionId("Which session would you like to update?");
            
            if(!dbMethods.ValidateSessionById(sessionIdToUpdate))
            {
                AnsiConsole.WriteLine($"[red]Record {sessionIdToUpdate} does not exist");
                sessionIdToUpdate = userInteractionService.GetSessionId("Please enter a valid session ID");
            }

            CodingSession sessionToUpdate = userInteractionService.GetStartEndTimeInput();
            sessionToUpdate.Id = sessionIdToUpdate;
            //sessionToUpdate.Duration = sessionToUpdate.EndTime - sessionToUpdate.StartTime;
            dbMethods.UpdateCodingSession(sessionToUpdate);
        }

        public void DeleteCodingSession()
        {
            // TODO print sessions so user can see the IDs
            
            int sessionIdToDelete = userInteractionService.GetSessionId("Which session would you like to delete?");

            if (!dbMethods.ValidateSessionById(sessionIdToDelete))
            {
                AnsiConsole.WriteLine($"[red]Record {sessionIdToDelete} does not exist");
                sessionIdToDelete = userInteractionService.GetSessionId("Please enter a valid session ID");
            }

            dbMethods.DeleteCodingSession(sessionIdToDelete);
        }

    }
}
