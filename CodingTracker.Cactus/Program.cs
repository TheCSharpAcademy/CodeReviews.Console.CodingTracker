using CodingTracker.Cactus;
using ConsoleTableExt;

namespace ConConfig
{
    class Program
    {
        // Store records in the memory to avoid frequent database queries.
        private static List<CodingSession>? codingSessionCache;

        static void Main(string[] args)
        {
            CodingSessionDBHelper.CreateCodingTrackerTableIfNotExist();

            codingSessionCache = CodingSessionDBHelper.SeleteAll(); // init cache
            if (codingSessionCache == null) codingSessionCache = new List<CodingSession>();
            bool endApp = false;

            while (!endApp)
            {
                PrintMenu();
                string? op = Console.ReadLine();
                switch (op)
                {
                    case "0":
                        Environment.Exit(0);
                        break;
                    case "1":
                        InsertCodingSessionRecord();
                        break;
                    case "2":
                        ShowAllCodingSessionRecords();
                        break;
                    case "3":
                        UpdateSpecificCodingSessionRecord();
                        break;
                    case "4":
                        DeleteSpecificCodingSessionRecord();
                        break;
                    default:
                        break;
                }
                Console.Write("\nPress 'q' and Enter to close the app, or press any other key and Enter to return MAIN MENU. ");
                if (Console.ReadLine() == "q") endApp = true;
                Console.WriteLine("\n");
            }
        }

        private static void PrintMenu()
        {
            Console.Clear();
            List<string> menuData = new List<string>
            {
                "<0> Exit app.",
                "<1> Insert a coding record.",
                "<2> Show all coding records.",
                "<3> Update a specific coding record.",
                "<4> Delete a specific coding record."
            };
            ConsoleTableBuilder
            .From(menuData)
            .WithTitle("MAIN MENU", ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithFormat(ConsoleTableBuilderFormat.Alternative)
            .ExportAndWriteLine(TableAligntment.Center);
        }

        private static void InsertCodingSessionRecord()
        {
            Console.Clear();
            Console.WriteLine("=> INSERT");
            bool isValidSession = false;
            DateTime startTime = InputUtils.GetValidTime();
            DateTime endTime = InputUtils.GetValidTime("end");
            while (!isValidSession)
            {
                if (startTime.CompareTo(endTime) <= 0)
                {
                    isValidSession = true;
                    continue;
                }
                Console.WriteLine("[!] Start time should not later than end time.\n");
                startTime = InputUtils.GetValidTime();
                endTime = InputUtils.GetValidTime("end");
            }
            CodingSession codingSession = new CodingSession(startTime, endTime);
            codingSession.Id = CodingSessionDBHelper.Insert(codingSession);
            if (codingSession.Id != -1)
            {
                if (codingSessionCache == null) codingSessionCache = new List<CodingSession>();
                codingSessionCache.Add(codingSession);
            }
        }

        private static void ShowAllCodingSessionRecords()
        {
            Console.Clear();
            if (codingSessionCache == null || codingSessionCache.Count == 0)
            {
                Console.WriteLine("There is no coding session record.");
                return;
            }
            Console.WriteLine("CODE SESSION RECORDS TABLE:");
            var records = new List<List<object>>();
            codingSessionCache.ForEach(session =>
            {
                var data = new List<object>
                {
                    session.Id.ToString(),
                    session.StartTime.ToString("HH:mm dd-MM-yyyy"),
                    session.EndTime.ToString("HH:mm dd-MM-yyyy"),
                    $"{session.Duration}min"
                };
                records.Add(data);
            });
            ConsoleTableBuilder
                .From(records)
                .ExportAndWriteLine();
        }

        private static void UpdateSpecificCodingSessionRecord()
        {
            Console.Clear();
            Console.WriteLine("=> UPDATE");
            ShowAllCodingSessionRecords();
            if (codingSessionCache == null || codingSessionCache.Count == 0) return;
            HashSet<int> ids = codingSessionCache.Select(x => x.Id).ToHashSet<int>();
            int id = InputUtils.GetInValidInputId(ids);
            DateTime startTime = InputUtils.GetValidTime();
            DateTime endTime = InputUtils.GetValidTime("end");
            CodingSession codingSession = codingSessionCache.Where(session => session.Id == id).ToList()[0];
            codingSession.StartTime = startTime;
            codingSession.EndTime = endTime;
            CodingSessionDBHelper.Update(codingSession);
        }

        private static void DeleteSpecificCodingSessionRecord()
        {
            Console.Clear();
            Console.WriteLine("=> DELETE");
            ShowAllCodingSessionRecords();
            if (codingSessionCache == null || codingSessionCache.Count == 0) return;
            HashSet<int> ids = codingSessionCache.Select(x => x.Id).ToHashSet<int>();
            int id = InputUtils.GetInValidInputId(ids);
            CodingSessionDBHelper.Delete(id);
            codingSessionCache = CodingSessionDBHelper.SeleteAll(); // Update the cache after deleting a record
        }
    }
}