using CodingTracker.Crud;

namespace CodingTracker.Operations {
    internal class ReportOperations {

        internal static void VisualizeRecords( DbOperations operations ) {

            Console.WriteLine("Would you like to see all reports (a), filter them (f) or see a condensed (c) form?. Press 0 to cancel and return.");
            string input = Console.ReadLine();
            Console.Clear();

            switch (input) {
                case "a":
                    operations.GetAllSessions();
                    break;
                case "f":
                    FilterReport(operations, "f");
                    break;
                case "c":
                    FilterReport(operations, "c");
                    break;
                case "0":
                    ConsoleOperations.GetMainInput();
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }

        static void FilterReport( DbOperations operations, string choice ) {

            Console.WriteLine("What's the start date?");
            string start = HelpersAndValidation.GetDateInput();
            Console.Clear();

            Console.WriteLine("What's the end date?");
            string end = HelpersAndValidation.GetDateInput();
            Console.Clear();

            HelpersAndValidation.ValidateDates(start, end);

            if (choice == "f") {
                operations.GetFilteredSessions(start, end);
            } else {
                operations.GetShortFormReport(start, end);
            }

        }
    }
}
