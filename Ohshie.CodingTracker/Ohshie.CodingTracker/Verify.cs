namespace Ohshie.CodingTracker;

public class Verify
{
    public static bool GoBack(string? userInput)
    {
        if (string.IsNullOrEmpty(userInput) || userInput == "no")
        {
            return true;
        }

        return false;
    }

    public static bool Confirm(string? userInput)
    {
        if (userInput == "yes")
        {
            return true;
        }

        return false;
    }
}