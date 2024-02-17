using System.Globalization;

namespace CodingTracker.Operations {
    internal class HelpersAndValidation {

        internal static void ValidateDates( string start, string end ) {
            var dateStart = DateTime.ParseExact(start, "dd-MM-yyyy", null);
            var dateEnd = DateTime.ParseExact(end, "dd-MM-yyyy", null);

            int difference = DateTime.Compare(dateStart, dateEnd);

            if (difference > 0) {
                Console.WriteLine("\n-----------\n");
                Console.WriteLine(@$"You inserted an invalid date pair. 
The end time ({dateEnd}) is earlier than the start time ({dateStart}), try again");
                Console.WriteLine("\n-----------\n");

               ConsoleOperations.GetMainInput();
            }
        }

        internal static void ValidateTime( string start, string end ) {
            var timeStart = DateTime.ParseExact(start, "HH:mm", null);
            var timeEnd = DateTime.ParseExact(end, "HH:mm", null);

            int difference = DateTime.Compare(timeStart, timeEnd);

            if (difference > 0) {
                Console.WriteLine("\n-----------\n");
                Console.WriteLine(@$"You inserted an invalid time pair. 
The end time ({timeEnd}) is earlier than the start time ({timeStart}), try again");
                Console.WriteLine("\n-----------\n");

                ConsoleOperations.GetMainInput();
            }
        }
        internal static string GetTimeInput() {
            Console.WriteLine("\nPlease insert the time in the format HH:mm. Type 0 to return to the main menu");

            string timeInput = Console.ReadLine();

            if (timeInput == "0") {
                ConsoleOperations.GetMainInput();
            }

            while (!DateTime.TryParseExact(timeInput, "HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _)) {
                Console.WriteLine("\nInvalid time. Type 0 to return to the main menu or try again:\n");
                timeInput = Console.ReadLine();
            }

            return timeInput;
        }



        internal static string GetDateInput() {
            Console.WriteLine("\nPlease insert the date in the format dd-mm-yyyy. Type 0 to return to the main menu");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") {
                ConsoleOperations.GetMainInput();
            }

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yyyy", new CultureInfo("en-US"), DateTimeStyles.None, out _)) {
                Console.WriteLine("\nInvalid date. Type 0 to return to the main menu or try again:\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }

        internal static void DealWithError( Exception ex ) {
            Console.WriteLine(ex);
            Console.WriteLine("\n------------\n");
            Console.WriteLine("Something Went wrong! Check what you typed, you might have typed something incorrectly, or check the error log!");
        }
    }
}
