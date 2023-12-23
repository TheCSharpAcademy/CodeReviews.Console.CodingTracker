using CodingTracker.Crud;
using CodingTracker.Model;
using System.Globalization;

namespace CodingTracker.Operations {
    internal class StopwatchOperations {

        public static void TimerOperations(DbOperations operation ) {

            Console.Clear();
            Console.WriteLine("\n---------------\n");
            Console.WriteLine("Timer selected, starting now. Press 's' when you want to stop.\n Press 0 to cancel and return");

            string startingDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm", DateTimeFormatInfo.InvariantInfo);

            string input = Console.ReadLine();

            while (input != "s") {

                if (input == "0") {
                    ConsoleOperations.GetMainInput();
                }

                Console.WriteLine("Invalid input. Press s to stop or 0 to go back and reset the clock");
                input = Console.ReadLine();
            }

            var stopDate = DateTime.Now.ToString("dd-MM-yyyy HH:mm", DateTimeFormatInfo.InvariantInfo);
            CodingSession session = new CodingSession("", startingDate, stopDate);

            try {
                operation.AddEntry(session);
                Console.WriteLine("Stopwatch stopped and session added to the database!");
            } catch (Exception ex) {
                HelpersAndValidation.DealWithError(ex);
            }
        }
    }
}
