namespace Ohshie.CodingTracker;

public class Errors
{
    public static void DoesNotExist(string errorMessage)
    {
        Console.Clear();
        Console.WriteLine($"Whoops, looks like nothing like that exist {errorMessage}\n" +
                          "Press enter to go back");
        Console.ReadLine();
    }

    public static void DefaultMenuError()
    {
        Console.Clear();
        Console.WriteLine("Looks like you pressed something that you shouldn't. \n" +
                          "Press any key to try again");
        Console.ReadKey(true);
    }
}