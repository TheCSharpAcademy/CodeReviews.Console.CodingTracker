using CodingTracker.Crud;
using CodingTracker.Model;
namespace CodingTracker.Operations {
    public static class ConsoleOperations {

        static DbOperations operations = new DbOperations();

        public static void GetMainInput() {

            bool isRunning = true;
            while (isRunning) {

                Console.WriteLine($@"What would you like to do:
    1 - Insert a new session
    2 - Show sessions
    3 - Use a stopwatch
    0 - Exit the program");

                string op = Console.ReadLine();
                Console.Clear();

                switch (op) {
                    case "1":
                        try {
                            InsertSession();
                        } catch (Exception ex) {
                            HelpersAndValidation.DealWithError(ex);
                        }
                        break;
                    case "2":
                        ReportOperations.VisualizeRecords(operations);
                        break;
                    case "3":
                        StopwatchOperations.TimerOperations(operations);
                        break;
                    case "4":
                        break;
                    case "0":
                        Console.WriteLine("\nGoodbye");
                        isRunning = false;
                        Environment.Exit(0);
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("Invalid input");
                        break;
                }

                Console.WriteLine("-------------------------------------------\n");
            }
        }

        static void InsertSession() {

            Console.WriteLine("\n--------------------------------\n Start Time");
            string dateStart = HelpersAndValidation.GetDateInput();

            string timeStart = HelpersAndValidation.GetTimeInput();

            Console.WriteLine("\n--------------------------------\n Finish Time");
            string dateEnd = HelpersAndValidation.GetDateInput();

            string timeEnd = HelpersAndValidation.GetTimeInput();

            Console.Clear();
            HelpersAndValidation.ValidateDates(dateStart, dateEnd);
            HelpersAndValidation.ValidateTime(timeStart, timeEnd);

            CodingSession session = new CodingSession("", $"{dateStart} {timeStart}", $"{dateEnd} {timeEnd}");

            operations.AddEntry(session);
        }
    }
}
