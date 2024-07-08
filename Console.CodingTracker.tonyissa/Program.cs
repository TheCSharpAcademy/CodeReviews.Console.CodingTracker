using CodingTracker.Database;
using CodingTracker.UserInterface;
using Spectre.Console;

DatabaseController.CreateDb();

while (true)
{
    try
    {
        UIHelper.InitMainMenu();
    }
    catch (Exception ex)
    {
        AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything | ExceptionFormats.NoStackTrace);
        AnsiConsole.MarkupLine("\n[red3_1]Please try again.[/] Press [lime]any key[/] to continue");
        Console.ReadKey();
    }
}