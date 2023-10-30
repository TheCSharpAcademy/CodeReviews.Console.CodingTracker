
using System.Globalization;

namespace CodingTracker.vilvee
{
    public class Program
    {

        static void Main(string[] args)
        {

            PrintMenu();

        }

        private static void PrintMenu()
        {
            do
            {
                const string MAIN_MENU = """
                                           -------------------------------------------
                                           MENU
                                           -------------------------------------------


                                           [1] START NEW SESSION

                                           [2] END OPEN SESSION

                                           [3] INPUT A COMPLETE SESSION

                                           [4] RETRIEVE ALL SESSIONS

                                           [5] UPDATE SESSION
                                           
                                           [6] DELETE SESSION

                                           [0] EXIT

                                           """;

                Console.WriteLine(MAIN_MENU);

                switch (Console.ReadLine())
                {
                    case "0":
                        Exit();
                        break;
                    case "1":
                        Console.Clear();
                        StartNewSession();
                        break;
                    case "2":
                        Console.Clear();
                        EndOpenSession();
                        break;
                    case "3":
                        Console.Clear();
                        InputACompleteSession();
                        break;
                    case "4":
                        Console.Clear();
                        Database.RetrieveAllSessions();
                        break;
                    case "5":
                        Console.Clear();
                        UpdateSession();
                        break;
                    case "6":
                        DeleteSession();
                        break;
                }

            } while (true);
        }

        private static void DeleteSession()
        {
            Database.RetrieveAllSessions();
            int userInput = GetNumberInput("\n\nWHICH SESSION DO YOU WANT TO DELETE? SELECT ID\n\n");
            Database.DeleteRecordInDb(userInput);
        }


        /// <summary>
        /// OPTION 0
        /// </summary>
        private static void Exit()
        {
            Console.WriteLine("GOODBYE");
            Thread.Sleep(1000);
            Environment.Exit(0);
        }

        /// <summary>
        /// OPTION 1
        /// </summary>
        private static void StartNewSession()
        {
            DateTime? date = GetDateInputDt("\nENTER DATE IN 'dd/MM/yy HH:mm:ss' FORMAT OR PRESS ENTER TO USE TODAY'S DATE: ");
            CodingSession cs = new CodingSession(date.Value);
            
            Database.AddNewSessionToDatabase(cs);
            Console.WriteLine("NEW SESSION CREATED");

            Console.WriteLine();
            Database.RetrieveAllSessions();
        }



        /// <summary>
        /// OPTION 2
        /// </summary>
        private static void EndOpenSession(int userInput = 0)
        {

            if (userInput == 0)
            {
                Database.RetrieveAllSessions();
                userInput = GetNumberInput("\n\nWHICH SESSION DO YOU WANT TO END? SELECT ID\n\n");

            }

            DateTime date = GetDateInputDt("ENTER DATE AND TIME ('dd/MM/yy HH:mm:ss')");

            Database.EndSessionInDatabase(userInput, date);
            Database.RetrieveAllSessions();

        }

        /// <summary>
        /// OPTION 3
        /// </summary>
        private static void InputACompleteSession()
        {
            CodingSession cs = new CodingSession();
            cs.StartTime = GetDateInputDt("\n\nSTART DATE (dd/MM/yy HH:mm:ss): ");
            cs.EndTime = GetDateInputDt("\nEND TIME (dd/MM/yy HH:mm:ss): ");
            Database.AddNewSessionToDatabase(cs);
            Database.RetrieveAllSessions();

        }

        /// <summary>
        /// OPTION 5
        /// </summary>
        private static void UpdateSession()
        {
            Database.RetrieveAllSessions();

            var recordId = GetNumberInput("\n\nENTER THE ID OF THE SESSION YOU WANT TO UPDATE\n\n");

            if (Database.GetEndTimeFromDb(recordId) != null)
            {
                Console.WriteLine("UPDATE START TIME:\nINPUT NEW START TIME OR ENTER 0 TO SKIP OR ENTER TO CONTINUE: ");
                DateTime starTime = Console.ReadLine() != "0" ? GetDateInputDt("START TIME: ") : Database.GetStartTimeFromDb(recordId);

                Console.WriteLine("UPDATE END TIME:\nINPUT NEW END TIME OR ENTER 0 TO SKIP OR ENTER TO CONTINUE: ");
                DateTime? endTime = Console.ReadLine() != "0" ? GetDateInputDt("END TIME: ") : Database.GetStartTimeFromDb(recordId);

                Database.UpdateSession(recordId, starTime, endTime);

            }
            else
            {
                EndOpenSession(recordId);
            }
        }


        internal static int GetNumberInput(string message = "")
        {
            if (!string.IsNullOrEmpty(message))
                Console.WriteLine(message);
            Int32 parsedInput;

            while (!Int32.TryParse(Console.ReadLine(), out parsedInput))
            {
                Console.WriteLine("WRONG INPUT");
            }
            if (parsedInput == 0) PrintMenu();
            return parsedInput;
        }

        internal static DateTime GetDateInputDt(string message)
        {
            Console.WriteLine(message);
            string input = Console.ReadLine();
            return string.IsNullOrWhiteSpace(input) ? DateTime.Now : ParseToDateTime(input);
        }

        internal static DateTime GetDateInputDt(string message = "", int recordId = 0)
        {
            DateTime parsedDate;

            if (!string.IsNullOrEmpty(message))
            {
                Console.WriteLine(message);
            }

            var dateInput = Console.ReadLine();
            if (dateInput == "0") Database.GetStartTimeFromDb(recordId);
            if(string.IsNullOrEmpty(dateInput)) parsedDate = DateTime.Now;

            while (!DateTime.TryParseExact(dateInput,
                       "dd/MM/yy HH:mm:ss",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None,
                       out parsedDate))
            {

                Console.WriteLine("\n\nInvalid format.");
                dateInput = Console.ReadLine();
            }

            return parsedDate;
        }

        internal static DateTime ParseToDateTime(string input)
        {
            DateTime parsedDate;
            while (!DateTime.TryParseExact(input,"dd/MM/yy HH:mm:ss",CultureInfo.InvariantCulture,DateTimeStyles.None, out parsedDate))
            {
                Console.WriteLine("WRONG INPUT");
                input = Console.ReadLine();
            };
            return parsedDate;
        }
    }
}
