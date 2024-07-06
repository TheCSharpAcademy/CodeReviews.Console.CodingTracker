using CodingTracker.Database;
using CodingTracker.UserInterface;

DatabaseController.CreateDb();

while (true)
{
    try
    {
        UIHelper.InitMainMenu();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("Please try again. Press any key to continue.");
        Console.ReadKey();
    }
}