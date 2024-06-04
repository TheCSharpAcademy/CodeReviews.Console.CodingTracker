using CodingTracker.ConsoleInteraction;
using CodingTracker.IDataRepository;
using CodingTracker.Models;
using CodingTracker.Services;

namespace CodingTracker.Controllers
{
    public class CodingSessionController
    {
        private readonly IUserInteraction? _userInteraction;
        private readonly ICodingSessionService _sessionService;
        private readonly IDataConfig _dataConfig;
        readonly CodingSession codingSession = new();
        bool closeApp;

        public CodingSessionController(IUserInteraction? userInteraction, ICodingSessionService sessionService, IDataConfig dataConfig)
        {
            _userInteraction = userInteraction;
            _sessionService = sessionService;
            _dataConfig = dataConfig;
        }

        public void Run()
        {
            // Initialize Database
            _dataConfig.InitializeDatabase();

            _userInteraction.ClearConsole();
            while (!closeApp)
            {
                // Display Menu
                _userInteraction?.DisplayMenu();
                // Get User Input
                var userInput = _userInteraction?.GetUserInput();
                // Process User Input
                SelectUserInput(userInput);
            }
        }

        public void SelectUserInput(string input)
        {
            _userInteraction.ClearConsole();
            switch (input)
            {
                case "0":
                    _userInteraction?.ShowMessageTimeout("\n[Red]Goodbye![/]\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    _userInteraction?.ShowMessageTimeout("\n[Green]Getting all coding sessions...[/]\n");
                    _sessionService.GetAllSessions();
                    break;
                case "2":
                    _sessionService.InsertSession(codingSession);
                    break;
                case "3":
                    _sessionService.UpdateSession();
                    break;
                case "4":
                    _sessionService.DeleteSession();
                    break;
                case "5":
                    _sessionService.StartCodingSession();
                    break;
                case "6":
                    _sessionService.CodingSessionsByPeriod();
                    break;
                default:
                    _userInteraction?.ShowMessageTimeout("\n[Red]Invalid input. Please type a number from 0 to 4.[/]\n");
                    break;
            }
        }
    }
}
