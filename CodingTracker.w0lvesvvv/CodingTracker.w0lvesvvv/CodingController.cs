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
            Console.WriteLine("     3 - Update coding records");
            Console.WriteLine("     4 - Delete coding records");
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
                case 3:
                    UpdateCodingRecord();
                    break;
                case 4:
                    DeleteCodingRecord();
                    break;
            }
        }

        private void TrackCodingTime()
        {
            CodingSession? codingSession = new();

            codingSession = UserInput.RequestCodingDates(codingSession);

            if (codingSession == null) return;

            if (!Validation.ValidateCorrectDateTimes(codingSession)) return;

            DataBaseManager.InsertCodingTime(codingSession);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Record inserted.");
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

        private void UpdateCodingRecord()
        {
            CodingSession? codingSession = new();

            ViewCodingRecords();

            int? id = UserInput.RequestCodingId();
            if (id == null) return;

            codingSession.Coding_session_id_i = id.Value;

            codingSession = UserInput.RequestCodingDates(codingSession);

            if (codingSession == null) return;

            DataBaseManager.UpdateCodingTime(codingSession);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Record updated.");
        }

        private void DeleteCodingRecord()
        {
            ViewCodingRecords();

            int? id = UserInput.RequestCodingId();
            if (id == null) return;

            DataBaseManager.DeleteCodingTime(id.Value);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Record deleted.");
        }
    }
}
