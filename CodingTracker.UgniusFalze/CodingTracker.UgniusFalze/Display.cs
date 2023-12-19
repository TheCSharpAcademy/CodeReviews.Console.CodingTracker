using ConsoleTableExt;

namespace CodingTracker
{
    internal class Display
    {
        public static void DisplayIntroMessage()
        {
            Console.WriteLine("Hello, welcome to coding tracker application. Here you'll be able to log how much time you've spent coding.");
        }

        public static void DisplayMenu()
        {
            Console.WriteLine("Please choose what kind of operation you would like to do:");
            Console.WriteLine("1. View all the current logged codding times.");
            Console.WriteLine("2. Insert a new daily log.");
            Console.WriteLine("3. Delete a single log");
            Console.WriteLine("4. Update a selected logged codding time.");
            Console.WriteLine("0. Exit the application");
        }

        public static void DisplaySeperator()
        {
            Console.WriteLine("----------------------------------");
        }

        public static void DisplayAllSessions(List<CoddingSession> habbits)
        {
            ConsoleTableBuilder
                .From(habbits)
                .WithTitle("Codding Tracker Log")
                .WithColumn("Id", "Start Time", "End Time", "Duration")
                .ExportAndWriteLine(TableAligntment.Center);
        }

        public static void DisplayIncorrectFormat()
        {
            Console.WriteLine("Incorrect date format, please enter the correct date format: " + SqliteOperations.GetDateFormat() + " .");
        }

        public static void DisplayIncorrectNumber()
        {
            Console.WriteLine("Please enter non negative and correct number.");
        }

        public static void DisplayIncorrectMenuOption()
        {
            Console.WriteLine("Please enter correct menu option.");
        }

        public static void DisplayUpdateMenu()
        {
            Console.WriteLine("Please choose which field you would like to update:");
            Console.WriteLine("1. Start Time");
            Console.WriteLine("2. End Time");
            Console.WriteLine("0. Go back to main menu.");
        }

        public static void DisplayDateInput()
        {
            Console.WriteLine("Using correct date format: " + SqliteOperations.GetDateFormat() + ".");
        }

        public static void DisplayDeleteLog()
        {
            Console.WriteLine("Please choose which log you want to delete");
        }

        public static void IOException()
        {
            Console.WriteLine("Console input error, try enter your input again.");
        }

        public static void SqliteException()
        {
            Console.WriteLine("SQLlite error, possibly duplicated dates, please try again.");
        }

        public static void IncorrectId()
        {
            Console.WriteLine("Check if your id is correct and try again.");
        }

        public static void DisplayWhichRecordToUpdate()
        {
            Console.WriteLine("Please choose which record you want to update.");
        }

        public static void DisplayEnterStartDate() {
            Console.WriteLine("Please enter when you started the codding session.");
        }

        public static void DisplayEnterEndDate()
        {
            Console.WriteLine("Please enter when you ended your codding session.");
        }
    }
}
