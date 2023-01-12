namespace SinghxRaj.CodingTracker;

internal class UserInput
{
    public static int GetOption()
    {
        Console.WriteLine("Type your option:");

        int option;
        string? input = Console.ReadLine();
        while (!int.TryParse(input, out option) || !Validator.ValidateOption(option))
        {
            Console.WriteLine("Invalid option. Type option again:");
            input = Console.ReadLine();
        }
        return option;
    }

    internal static CodingSession NewCodingSessionInfo()
    {
        // Get User input for the coding session
        throw new NotImplementedException();
    }
}