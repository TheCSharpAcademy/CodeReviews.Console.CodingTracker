using ConsoleTableExt;

namespace CodingTracker.w0lvesvvv
{
    public class CodingController
    {
        public void DisplayMenu()
        {
            #region Menu
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("================ CODING TRACKER ===============");
            Console.WriteLine("     1 - Introduce coding record");
            Console.WriteLine("     2 - View coding records");
            Console.WriteLine("     0 - Exit");
            Console.WriteLine("===============================================");

            Console.Write("Option selected: ");
            Console.ForegroundColor = ConsoleColor.White;
            int? option = UserInput.ReadNumber();
            #endregion

            if (option == null) { return; }

            switch (option)
            {
                case 0:
                    Environment.Exit(0);
                    break;
                case 1:
                    TrackCodingTime();
                    break;
                case 2:
                    ViewCodingRecords();
                    break;
            }
        }

        private void TrackCodingTime()
        {
            CodingSession codingSession = new();

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("(Time format MUST BE: dd/MM/yyyy hh:mm)");
            Console.Write("Introduce start coding time: ");
            Console.ForegroundColor = ConsoleColor.White;
            codingSession.Coding_session_start_date_time_nv = UserInput.ReadDateTimeString();

            if (string.IsNullOrEmpty(codingSession.Coding_session_start_date_time_nv)) return;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Introduce end coding time: ");
            Console.ForegroundColor = ConsoleColor.White;
            codingSession.Coding_session_end_date_time_nv = UserInput.ReadDateTimeString();

            if (string.IsNullOrEmpty(codingSession.Coding_session_end_date_time_nv)) return;

            codingSession.CalculateDuration();

            if (!Validation.ValidateCorrectDateTimes(codingSession)) return;

            DataBaseManager.InsertCodingTime(codingSession);
        }

        private void ViewCodingRecords()
        {
            List<CodingSession> codingRecords = DataBaseManager.GetCodingRecords();

            if (!codingRecords.Any()) {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There aren't coding sessions yet. Try creating one first");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            ConsoleTableBuilder.From(codingRecords).ExportAndWriteLine();
        }
    }
}

// To show the data on the console, you should use the "ConsoleTableExt" library.
// You'll need to create a configuration file that you'll contain your database path and connection strings.
