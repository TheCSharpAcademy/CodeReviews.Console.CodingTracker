using CodingTracker.wkktoria.Controllers;

namespace CodingTracker.wkktoria.Ui;

public class UserInterface
{
    private readonly CodingController _codingController;

    public UserInterface(CodingController codingController)
    {
        _codingController = codingController;
    }

    public void Run()
    {
        var quitApp = false;

        while (!quitApp)
        {
            Console.Clear();
            ShowMenu();

            Console.Write("> ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "0":
                    quitApp = true;
                    break;
                case "1":
                    _codingController.ShowAll(true);
                    break;
                case "2":
                    _codingController.ShowBetweenTwoDates();
                    break;
                case "3":
                    _codingController.ShowOneById();
                    break;
                case "4":
                    _codingController.ShowReport();
                    break;
                case "5":
                    _codingController.SetGoal();
                    break;
                case "6":
                    _codingController.TrackTime();
                    break;
                case "7":
                    _codingController.Add();
                    break;
                case "8":
                    _codingController.Update();
                    break;
                case "9":
                    _codingController.Delete();
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }

    private static void ShowMenu()
    {
        Console.WriteLine("Available options:");
        Console.WriteLine("\u2022 0 - quit app");
        Console.WriteLine("\u2022 1 - show all");
        Console.WriteLine("\u2022 2 - show all between two dates");
        Console.WriteLine("\u2022 3 - show one by id");
        Console.WriteLine("\u2022 4 - show report");
        Console.WriteLine("\u2022 5 - set goal for current month");
        Console.WriteLine("\u2022 6 - track time");
        Console.WriteLine("\u2022 7 - add new");
        Console.WriteLine("\u2022 8 - update existing");
        Console.WriteLine("\u2022 9 - delete existing");
    }
}