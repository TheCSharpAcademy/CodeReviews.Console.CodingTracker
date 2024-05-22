namespace CodingTracker.ConsoleInteraction
{
    public interface IUserInteraction
    {
        void DisplayMenu();
        void ShowMessage(string message);
        void ShowMessageTimeout(string message);
        string GetUserInput();
    }
}