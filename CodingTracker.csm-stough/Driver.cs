using ConsoleUtilities;

namespace CodeTracker.csm_stough
{
    internal class Driver
    {
        private static int resultsPerPage = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("resultsPerPage"));
        private static string dateFormat = System.Configuration.ConfigurationManager.AppSettings.Get("dateFormat");

        public static void Main(string[] args)
        {
            SessionDao.Init();
            GoalDao.Init();

            MainMenu();
        }

        static void MainMenu()
        {
            Console.Clear();
            GoalManager.UpdateGoals();

            new Calender();
            Console.Write("\n");
            GoalManager.DisplayCurrentGoals();
            
            Menu mainMenu = new Menu("\nMain Menu ~~~~~~~~~~~~~~~~~");

            mainMenu.AddOption("L", "Logs...", () => { RecordsMenu(); });
            mainMenu.AddOption("N", "New Log...", () => { NewRecordMenu(); });
            mainMenu.AddOption("G", "Goals...", () => { GoalMenu(); });
            mainMenu.AddOption("Q", "Quit Program", () => { Environment.Exit(0); });

            mainMenu.SelectOption(false);
        }

        static void RecordsMenu()
        {
            Menu recordsMenu = new Menu("Logs");

            recordsMenu.AddOption("A", "View All Logs", () => { AllRecordsMenu(); });
            recordsMenu.AddOption("F", "Filter Logs...", () => { FilterRecordsMenu(); });
            recordsMenu.AddOption("B", "Go Back To Main Menu...", () => { MainMenu(); });

            recordsMenu.SelectOption();
        }

        static void AllRecordsMenu()
        {
            bool ascending = true;

            Form orderForm = new Form("", (values) =>
            {
                ascending = (values[0].ToString() == "Oldest to newest") ? true : false;
            });

            orderForm.AddChoiceQuery("Please select an ordering", "Oldest to newest", "Newest to oldest");

            orderForm.Start();

            RecordsRenderer dataRenderer = new RecordsRenderer(
                (limit, offset) =>
                {
                    return new List<Object>(SessionDao.GetAll(limit, offset, ascending: ascending));
                },
                () =>
                {
                    return SessionDao.GetCount();
                },
                resultsPerPage);
            dataRenderer.DisplayTable();
            MainMenu();
        }

        static void FilterRecordsMenu()
        {
            Menu filterMenu = new Menu("Filter Logs");

            filterMenu.AddOption("Y", "By Year...", () => { FilterByTimeInterval("%Y", "Year"); });
            filterMenu.AddOption("M", "By Month...", () => { FilterByTimeInterval("%Y-%m", "Month"); });
            filterMenu.AddOption("D", "By Day...", () => { FilterByTimeInterval("%Y-%m-%d", "Day"); });
            filterMenu.AddOption("R", "Between Dates/Times...", () => { FilterBetweenDates(); });
            filterMenu.AddOption("B", "Go Back To Main Menu...", () => { MainMenu(); });

            filterMenu.SelectOption();
        }

        static void FilterByTimeInterval(string timeFormat, string unit)
        {
            ReportsRenderer reportsRenderer = new ReportsRenderer(timeFormat, unit, null, null, resultsPerPage);
            reportsRenderer.DisplayTable();
            FilterRecordsMenu();
        }

        static void FilterBetweenDates()
        {
            DateTime[] range = null;

            Form betweenDatesForm = new Form("Enter a datetime range", (values) =>
            {
                range = (DateTime[])values[0];
            });
            betweenDatesForm.AddDateTimeRangeQuery("Enter a DateTime", dateFormat);
            betweenDatesForm.Start();
            RecordsRenderer recordRenderer = new RecordsRenderer(
                (limit, offset) =>
                {
                    return new List<Object>(SessionDao.GetAll(limit, offset, "Start BETWEEN '{range[0].ToString(dateFormat)}' AND '{range[1].ToString(dateFormat)}'"));
                },
                () =>
                {
                    return SessionDao.GetCount($"Start BETWEEN '{range[0].ToString(dateFormat)}' AND '{range[1].ToString(dateFormat)}'");
                },
                resultsPerPage);
            recordRenderer.DisplayTable();
            FilterRecordsMenu();
        }

        static void NewRecordMenu()
        {
            Menu newRecordMenu = new Menu("Create A New Log");

            newRecordMenu.AddOption("M", "Manual Entry", () => { ManualRecordEntry(); });
            newRecordMenu.AddOption("S", "Stopwatch Entry", () => { StopwatchMenu(); });
            newRecordMenu.AddOption("B", "Go Back To Main Menu", () => { MainMenu(); });

            newRecordMenu.SelectOption();
        }

        static void ManualRecordEntry()
        {
            Form manualForm = new Form(
                "Manual Record Entry Form.\n" +
                $"Please enter dates/times in the format ({dateFormat})\n" +
                "Example: 2015-05-29 05:50:00 AM", 
                (values) =>
            {
                SessionDao.Insert((DateTime)values[0], (DateTime)values[1]);
            });

            manualForm.AddDateTimeQuery("Please enter the starting date/time", dateFormat);
            manualForm.AddDateTimeQuery("Please enter the ending date/time", dateFormat);

            manualForm.Start();

            NewRecordMenu();
        }

        static async void StopwatchMenu()
        {
            DateTime start = DateTime.Now;
            StartClock(start);
            DateTime end = DateTime.Now;

            Console.WriteLine("Stopwatch ended...");

            Form stopwatchForm = new Form($"You spent {(end - start).ToString(@"hh\:mm")} coding!", (values) =>
            {
                if (values[0].ToString().ToLower() == "yes")
                {
                    SessionDao.Insert(start, end);
                }
            });

            stopwatchForm.AddChoiceQuery("Would you like to log this time?", "Yes", "No");

            stopwatchForm.Start();

            NewRecordMenu();
        }

        static void StartClock(DateTime start)
        {
            string startTimeLabel = $"Starting Date/Time: {start.ToString("MM/dd/yyyy hh:mm:ss tt")}";
            bool completed = false;

            Task.Run(() =>
            {
                ConsoleKeyInfo cki = new ConsoleKeyInfo();

                do
                {
                    cki = Console.ReadKey(true);
                } while (cki.Key != ConsoleKey.Enter);

                completed = true;
            });

            while (!completed)
            {
                Console.Clear();
                Console.WriteLine(startTimeLabel);
                Console.WriteLine($"Current Date/Time: {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")}");
                Console.WriteLine("\nPress 'Enter' to end the stopwatch...");
                Thread.Sleep(500);
            }
        }

        static void GoalMenu()
        {
            Menu goalsMenu = new Menu("Goals");

            goalsMenu.AddOption("A", "All Goals", () => { AllGoals(); });
            goalsMenu.AddOption("N", "New Goal", () => { NewGoal(); });

            goalsMenu.SelectOption();
        }

        static void AllGoals()
        {
            Console.Clear();

            Console.WriteLine("Active Goals ~~~~~~~~~~~~~~");
            GoalManager.CurrentGoals.ForEach(goal => GoalManager.DisplayGoal(goal));

            Console.WriteLine("\nUpcoming Goals ~~~~~~~~~~~~~~");
            GoalManager.UpcomingGoals.ForEach(goal => GoalManager.DisplayGoal(goal));

            Console.WriteLine("\nExpired Goals ~~~~~~~~~~~~~~");
            GoalManager.PastGoals.ForEach(goal => GoalManager.DisplayGoal(goal));

            Console.WriteLine("\n\nPress any key to return to the Main Menu...");
            Console.ReadLine();
            MainMenu();
        }

        static void NewGoal()
        {
            Form goalForm = new Form("New Goal", (values) =>
            {
                DateTime[] range = (DateTime[])values[0];
                GoalDao.InsertGoal(range[0], range[1], TimeSpan.FromHours(int.Parse(values[1].ToString())));
            });

            goalForm.AddDateTimeRangeQuery("Enter a Start and End date for this goal", dateFormat);
            goalForm.AddFloatQuery("Please enter a target amount of hours for this goal", true);

            goalForm.Start();

            MainMenu();
        }
    }
}
