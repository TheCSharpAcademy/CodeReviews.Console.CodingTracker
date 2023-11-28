using CodingTracker.Cactus;

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
                        UpdateSpecificWaterHabitRecord();
                        break;
                    default:
                        break;
                }
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue. ");
                if (Console.ReadLine() == "n") endApp = true;
                Console.WriteLine("\n");
            }
        }

        private static void PrintMenu()
        {
            Console.Clear();
            Console.WriteLine("---------------------------------------------------------");
            Console.WriteLine("= Main Menu =");
            Console.WriteLine("<0> Exit app.");
            Console.WriteLine("<1> Insert a coding record.");
            Console.WriteLine("<2> Show all coding records.");
            Console.WriteLine("<3> Update a specific coding record.");
            Console.WriteLine("<4> Delete a specific coding record.");
            Console.WriteLine("---------------------------------------------------------");
        }
        private static void InsertCodingSessionRecord()
        {
            Console.Clear();
            Console.WriteLine("= Insert Menu =");
            DateTime startTime = InputUtils.GetValidTime();
            DateTime endTime = InputUtils.GetValidTime("end");
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
            Console.WriteLine("---------------------------------Habit Records---------------------------------");
            codingSessionCache.ForEach(session => Console.WriteLine(
                                        $"id: {session.Id}\t" +
                                        $"startTime: {session.StartTime.ToString("dd-MM-yyyy")}\t" +
                                        $"endTime: {session.EndTime.ToString("dd-MM-yyyy")}\t" +
                                        $"duration: {session.Duration}\n"));
            Console.WriteLine("-------------------------------------------------------------------------------");
        }

        private static void UpdateSpecificWaterHabitRecord()
        {
            Console.Clear();
            Console.WriteLine("UPDATE MENU");
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
    }
}