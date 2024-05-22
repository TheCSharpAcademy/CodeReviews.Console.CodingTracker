using CodingTracker.ConsoleInteraction;

namespace CodingTracker.Controllers
{
    public class CodingSessionController
    {
        private readonly IUserInteraction? _userInteraction;
        bool closeApp;

        public CodingSessionController(IUserInteraction? userInteraction)
        {
            _userInteraction = userInteraction;
        }

        public void Run()
        {
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
            switch (input)
            {
                case "0":
                    _userInteraction?.ShowMessageTimeout("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    _userInteraction?.ShowMessageTimeout("Getting all coding sessions...");
                    _codingSessionService.GetAllHabits();
                    break;
                // case "2":
                //     _habitRepository.InsertHabit(habit);
                //     break;
                // case "3":
                //     _habitRepository.UpdateHabit();
                //     break;
                // case "4":
                //     _habitRepository.DeleteHabit();
                //     break;
                default:
                    _userInteraction?.ShowMessageTimeout("\nInvalid input. Please type a number from 0 to 4.\n");
                    break;
            }
        }
    }
}
