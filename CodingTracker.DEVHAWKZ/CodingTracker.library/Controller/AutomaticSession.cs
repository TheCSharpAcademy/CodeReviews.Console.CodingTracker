using CodingTracker.library.View;
using System.Diagnostics;

namespace CodingTracker.library.Controller;

internal static class AutomaticSession
{
    internal static void NewAutomaticSession()
    {
        Console.Clear();

        bool getBack = Helpers.GetBackToMainMenu("start","to start new coding session");

        if (getBack) Menu.MainMenu();

        else
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            DateTime start = DateTime.Now;

            Console.WriteLine("Coding session has started.\nType 'stop' to terminate coding session.");
            Validations.TerminateAutomaticSession(Console.ReadLine());

            stopwatch.Stop();
            
            TimeSpan durationSpan = stopwatch.Elapsed;
            DateTime end = start.Add(durationSpan);

            double duration = Math.Round(durationSpan.TotalMinutes, 0);

            Console.WriteLine($"\nCoding session has been stopped.\nDuration of your coding session: {duration} minutes");
            
            Console.WriteLine("\nDo you wish to insert this session?\nType 'yes' or 'no'");
            string choice = Validations.InsertAutomaticSessionChoice(Console.ReadLine());
               
            if(choice.ToLower().Trim() == "yes")
            {
                string startTime = start.ToString("dd-MM-yyyy HH:mm");
                string endTime = end.ToString("dd-MM-yyyy HH:mm");
                CrudController.InsertNewAutomaticSession(startTime, endTime, duration);
            }
            
            else
            {
                Console.WriteLine("\n\nPress any key to get back to main menu...");
                Console.ReadKey();
            }
                
        }
    }
}
