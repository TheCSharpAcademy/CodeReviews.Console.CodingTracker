
using System;
using System.Data;

namespace CodingTracker.Paul_W_Saltzman
{
    internal static class Menu
    {
        internal static void TopMenu()
        {
            Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
            Console.Clear();
            bool running = true;
            while (running)
            {
                Settings settings = Data.GetSettings();
                List<string> mainMenu = new List<string>();
                mainMenu.Add("X: Exit Program");
                mainMenu.Add("1: Log Coding Session");
                mainMenu.Add("2: Timed Coding Session");
                mainMenu.Add("3: View Coding Logs");
                mainMenu.Add("4: Goals");
                mainMenu.Add("5: View Daily Totals");
                mainMenu.Add("6: View Weekly Totals");
                mainMenu.Add("7: Settings");
                if (settings.TestMode)
                {
                    mainMenu.Add("8: Generate Logs (TEST MODE)");
                }

                DataTable mainMenuTable = MenuTable(mainMenu);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Helpers.ShowTable(mainMenuTable, "Main Menu");
                Helpers.CenterCursor();
                String userInput = Console.ReadLine();
                userInput = userInput.Trim().ToLower();

                switch (userInput)
                {
                    case "x":
                        running = false;
                        Console.WriteLine("....Goodbye");
                        Environment.Exit(0);
                        break;
                    case "1":
                        LogSession();
                        break;
                    case "2":
                        CodingSession.TimedCodingSession();
                        break;
                    case "3":
                        ListSessions();
                        break;
                    case "4":
                        ListGoals();
                        break;
                    case "5":
                        ListDailyTotals();
                        break;
                    case "6":
                        ListWeeklyTotals();
                        break;
                    case "7":
                        Settings.ProgramSettings(settings);
                        break;
                    case "8":
                        if (settings.TestMode)
                        {
                            GenerateRandomCodingSessions(); 
                        }
                        break;
                }
            }
        }

        internal static DataTable MenuTable(List<string> menu)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Options",typeof(string));
            foreach(string item in menu) 
            {
                dataTable.Rows.Add(item);
            }
            return dataTable;
        }

        internal static void ListSessions()
        {
            bool running = true;
            int sort = 0;
            bool reporting = false;
            while (running)
            {
                Console.Clear();
                String title = "CODING LOGS";
                List<CodingSession> sessions = Data.LoadSessions();
                switch(sort)
                {
                    case 0:
                        sessions = CodingSession.BubbleSortByStartTime(sessions);
                        break;
                    case 1:
                        sessions = CodingSession.ReverseBubbleSortByStartTime(sessions);
                        break;
                    case 2:
                        //no sort
                        break;
                  
                }
                DataTable dataTable = CodingSession.BuildDataTableNoID(sessions);
                if (dataTable == null) 
                {
                    running = false;
                    break;
                }
                Helpers.ShowTable(dataTable, title);
                if (reporting) 
                {
                    CodingSession.Reporting(sessions);

                }
                Helpers.CenterText("|X: EXIT  |S: SORT  |W: BY WEEK  |B: BY DAY  |R: REPORT  |M: MODIFY  |D: DELETE  |");
                Console.WriteLine();
                Helpers.CenterCursor();
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper().Trim();

                switch (userInput)
                {
                    case "X":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        running = false;
                        break;
                    case "S":
                        sort = SortMenu(sort);
                        break;
                    case "W":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        CodingSessionByWeek(sessions, sort);
                        break;
                    case "B":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        CodingSessionByDay(sessions, sort);
                        break;
                    case "R":
                        if(!reporting)
                        { reporting = true; }
                        else 
                        { reporting = false; }
                        break;
                    case "M":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        DataTable idDataTable = CodingSession.BuildDataTableWithId(sessions);
                        Helpers.ShowTable(idDataTable, title);
                        Console.WriteLine();
                        Console.WriteLine();
                        CodingSession session = GetSession("Please enter the Id of the session you wish to Modify");
                        ModifyCodingSessionMenu(session);
                        break;
                    case "D":
                        // Clears the screen and the scrollback buffer in xterm-compatible terminals.
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        idDataTable = CodingSession.BuildDataTableWithId(sessions);
                        Helpers.ShowTable(idDataTable, title);
                        Console.WriteLine();
                        Console.WriteLine();
                        session = GetSession("Please enter the Id of the session you wish to Delete");
                        DeleteSessionMenu(session);
                        break;
                    
                    default:
                        Helpers.Output("Invalid Option Press ENTER to Continue.");
                        Console.ReadLine();
                        break;
                }

            }

        }

        internal static int SortMenu(int sort)
             {
              string userInput;
              bool sortMenu = true;
                  while (sortMenu)
                  {
                        Helpers.Output("|X: BACK  |A: ASCENDING  |D: DESCENDING  |N: NO SORT");
                        Helpers.CenterCursor();
                        userInput = Console.ReadLine();
                        userInput = userInput.Trim().ToUpper();
                        switch (userInput)
                        {
                        case "X":
                            sortMenu = false;
                            break;
                        case "A":
                            sort = 0;
                            sortMenu = false;
                            break;
                        case "D":
                            sort = 1;
                            sortMenu = false;
                            break;
                        case "N":
                            sort = 2;
                            sortMenu = false;
                            break;
                        default:
                             Helpers.CenterText("Invalid Option Please Try Again.  Press Any Key to Continue");
                             Console.ReadKey();
                             break;
                        }
                  }
            return sort;
         }

        internal static void CodingSessionByDay(List<CodingSession> sessions, int sort)
        {
            bool reporting = false;
            bool running = true;
            DateOnly showDate = DateOnly.FromDateTime(DateTime.Now);

            while (running)
            {
                List<CodingSession> daySessions = CodingSession.LoadSessionsByDate(showDate);
                switch (sort)
                {
                    case 0:
                        daySessions = CodingSession.BubbleSortByStartTime(daySessions);
                        break;
                    case 1:
                        daySessions = CodingSession.ReverseBubbleSortByStartTime(daySessions);
                        break;
                    case 2:
                        //no sort
                        break;

                }
                DataTable dayDataTable = CodingSession.BuildDataTableWithId(daySessions);
                string titleDate = $@"Date: {showDate}";
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Helpers.ShowTable(dayDataTable, titleDate);
                if (daySessions.Count == 0)
                {
                    Helpers.CenterText($@"There are no coding sessions for {showDate}.");
                }
                if (reporting)
                {
                    CodingSession.Reporting(daySessions);

                }
                Console.WriteLine();
                Helpers.CenterText("|X: EXIT  |S: SORT  |R: REPORTING  |P: PRIOR DAY  |N: NEXT DAY  |M: MODIFY  |D: DELETE  |");
                Console.WriteLine();
                Helpers.CenterCursor();
                String userInput = Console.ReadLine();
                userInput = userInput.Trim().ToUpper();

                switch (userInput)
                {
                    case "X":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        running = false;
                        break;
                    case "S":
                        sort = SortMenu(sort);
                        break;
                    case "R":
                        if (!reporting)
                        { reporting = true; }
                        else
                        { reporting = false; }
                        break;
                    case "P":
                        showDate = showDate.AddDays(-1);
                        break;
                    case "N":
                        showDate = showDate.AddDays(1);
                        break;
                    case "M":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        DataTable idDataTable = CodingSession.BuildDataTableWithId(sessions);
                        Helpers.ShowTable(idDataTable, titleDate);
                        Console.WriteLine();
                        Console.WriteLine();
                        CodingSession session = GetSession("Please enter the Id of the session you wish to Modify");
                        ModifyCodingSessionMenu(session);
                        break;
                    case "D":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        idDataTable = CodingSession.BuildDataTableWithId(sessions);
                        Helpers.ShowTable(idDataTable, titleDate);
                        Console.WriteLine();
                        Console.WriteLine();
                        session = GetSession("Please enter the Id of the session you wish to Delete");
                        DeleteSessionMenu(session);
                        break;

                }

            }

        }
        internal static void CodingSessionByWeek(List<CodingSession> sessions, int sort)
        {
            bool reporting = false;
            bool running = true;
            int showYearWeek = WeeklyTotals.YearWeekCreator(DateTime.Now);
            
            while (running)
            {
                List<CodingSession> weekSessions = CodingSession.LoadSessionsByWeek(showYearWeek);
                switch (sort)
                {
                    case 0:
                        weekSessions = CodingSession.BubbleSortByStartTime(weekSessions);
                        break;
                    case 1:
                        weekSessions = CodingSession.ReverseBubbleSortByStartTime(weekSessions);
                        break;
                    case 2:
                        //no sort
                        break;

                }
                DataTable weekDataTable = CodingSession.BuildDataTableWithId(weekSessions);
                int year = showYearWeek / 100;
                int week = showYearWeek % 100;
                string titleWeek = $@"Year: {year}  Week: {week}";
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Helpers.ShowTable(weekDataTable, titleWeek);
                if (weekSessions.Count == 0)
                {
                    Helpers.CenterText($@"There are no coding sessions for the {week} week of {year}.");
                }
                if (reporting)
                {
                    CodingSession.Reporting(weekSessions);

                }
                Console.WriteLine();
                Helpers.CenterText("|X: EXIT  |S: SORT  |R: REPORTING  |P: PRIOR WEEK  |N: NEXT WEEK  |M: MODIFY  |D: DELETE  |");
                Console.WriteLine();
                Helpers.CenterCursor();
                String userInput = Console.ReadLine();
                userInput = userInput.Trim().ToUpper();

                switch (userInput)
                {
                    case "X":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        running = false;
                        break;
                    case "S":
                        sort = SortMenu(sort);
                        break;
                    case "R":
                        if (!reporting)
                        { reporting = true; }
                        else
                        { reporting = false; }
                        break;
                    case "P":
                        showYearWeek = Helpers.LastWeek(showYearWeek);
                        break;
                    case "N":
                        showYearWeek = Helpers.NextWeek(showYearWeek);
                        break;
                    case "M":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        DataTable idDataTable = CodingSession.BuildDataTableWithId(sessions);
                        Helpers.ShowTable(idDataTable, titleWeek);
                        Console.WriteLine();
                        Console.WriteLine();
                        CodingSession session = GetSession("Please enter the Id of the session you wish to Modify");
                        ModifyCodingSessionMenu(session);
                        break; 
                    case "D":
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                        Console.Clear();
                        idDataTable = CodingSession.BuildDataTableWithId(sessions);
                        Helpers.ShowTable(idDataTable, titleWeek);
                        Console.WriteLine();
                        Console.WriteLine();
                        session = GetSession("Please enter the Id of the session you wish to Delete");
                        DeleteSessionMenu(session);
                        break;
                     
                }

            }

        }

        internal static void GenerateRandomCodingSessions()
        {
            Helpers.CenterText("Please enter the number (between 1 and 50) of random sessions you would like to create.");
            Helpers.CenterCursor();
            string randomCreateString = Console.ReadLine();
            randomCreateString = randomCreateString.ToUpper().Trim();
            if (Int32.TryParse(randomCreateString, out int randomCreate))
            {
                if (randomCreate >= 1 && randomCreate <= 50)// between 1 and 50
                {
                    List<CodingSession> sessions = new List<CodingSession>();
                    Random random = new Random();
                    for (int i = 0; i < randomCreate; i++)
                    {
                        CodingSession session = CodingSession.GenerateRandomSession();
                        sessions.Add(session);
                    }
                    DataTable dataTable = CodingSession.BuildDataTableWithId(sessions);
                    Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                    Console.Clear();
                    Helpers.ShowTable(dataTable,"Added Sessions");
                    Helpers.CenterText("Coding sessions added.  Press any key to Continue.");
                    Helpers.CenterCursor();
                    Console.ReadKey();


                }
                else
                {
                    Helpers.CenterText("Please enter a number between 1 and 50.");
                }
            }
            else
            {
                Helpers.CenterText("Invalid input. Please enter a valid number.");
            }
        }
    
        internal static void ModifyCodingSessionMenu(CodingSession session)
        {
            bool running = true;
            while (running)
            {
                List<CodingSession> codingSessions = new List<CodingSession>();
                codingSessions.Add(session);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                DataTable codingSessionDataTable = CodingSession.BuildDataTableWithId(codingSessions);
                Helpers.ShowTable(codingSessionDataTable, "Modify");
                Helpers.CenterText("Please choose the field you wish to modify. Press ENTER to Continue");
                Console.WriteLine();
                Helpers.CenterCursor();
                Console.ReadLine();
                Helpers.Output("|X: EXIT |D: Update Date |S: Update StartTime |E: Update EndTime |A: Update All |");
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper().Trim();
               
                switch (userInput)
                {
                    case "X":
                        running = false;
                        break;
                    case "D":
                        DateOnly date = GetDate("Date Entry");
                        session = CodingSession.UpdateDate(session, date);
                        break;
                    case "S":
                            bool inputGood = false;
                            TimeOnly startTime = new TimeOnly();
                            while (!inputGood)
                            {
                                startTime = GetTime("Start Time Entry");
                                TimeOnly eTime = TimeOnly.FromDateTime(session.EndTime);
                                if (eTime >= startTime)
                                {
                                session = CodingSession.UpdateStartTime(session, startTime);
                                inputGood = true;
                                }
                                else
                                {
                                Console.WriteLine();
                                Helpers.Output("Invalid time: Start Time must be earlier than EndTime. Press ENTER to Continue");
                                 Console.ReadLine();
                                }
                            }
                            session = CodingSession.UpdateStartTime(session, startTime);
                        break;
                    case "E":
                        inputGood = false;
                        TimeOnly endTime = new TimeOnly();
                        while (!inputGood)
                        {
                            endTime = GetTime("End Time Entry");
                            TimeOnly sTime = TimeOnly.FromDateTime(session.StartTime);
                            if (endTime >= sTime)
                            {
                                session = CodingSession.UpdateEndTime(session, endTime);
                                inputGood = true;
                            }
                            else
                            {
                                Console.WriteLine();
                                Helpers.Output("Invalid time: End Time must be later than Start Time. Press ENTER to Continue");
                                Console.ReadLine();
                            }
                        }
                            
                        break;
                    case "A":
                        session = CodingSession.UpdateAll(session);
                        break;
                    default:
                        Helpers.Output("Invalid Option Press ENTER to Continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        internal static CodingSession GetSession(string instruction)
        {
            CodingSession session = new CodingSession();
            bool exists = false;
            while (!exists)
            {
                Helpers.Output(instruction);
                string userInput = Console.ReadLine();
                if (int.TryParse(userInput, out int intUserInput))
                {
                    if (exists = CodingSession.DoesExist(intUserInput))
                    {
                        session = CodingSession.GetSession(intUserInput);
                    }
                    else 
                    {
                        Helpers.Output("Please enter a valid ID. Press ENTER to continue.");
                        Console.ReadLine();
                    }

                }
                else
                { 
                    Helpers.Output("Please enter a valid character. Press ENTER to continue.");
                    Console.ReadLine();
                } 
            }
            return session;
        }

        internal static void DeleteSessionMenu(CodingSession session) 
        {
            bool running = true;
            while (running)
            {
                List<CodingSession> codingSessions = new List<CodingSession>();
                codingSessions.Add(session);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                DataTable codingSessionDataTable = CodingSession.BuildDataTableWithId(codingSessions);
                Helpers.ShowTable(codingSessionDataTable, "Delete");
                Helpers.CenterText("|X: EXIT |D: Delete |C: Cancel |");
                Helpers.CenterCursor();
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper().Trim();
                switch (userInput)
                {
                    case "X":
                        running = false;
                        break;
                    case "D":
                        Data.DeleteSession(session);
                        CodingSession.CheckGoals(session, 1);
                        Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11;
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.WriteLine();
                        Helpers.Output("The session has been deleted. Press ENTER to continue.");
                        Console.ReadLine();
                        running = false;
                        break;
                    case "C":
                        running = false;
                        break;
                    default:
                        Helpers.Output("Invalid Option Press ENTER to Continue.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        internal static void ListGoals()
        {
            bool listGoals = true;
            while (listGoals)
            {
                Console.Clear();
                String title = "Goals";
                DataTable dataTable = Goals.BuildGoalTable();
                Helpers.ShowTable(dataTable, title);
                Helpers.CenterText("|X: EXIT  |M: MODIFY");
                Console.WriteLine();
                Helpers.CenterCursor();
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper().Trim();

                switch (userInput) 
                {
                    case "M":
                        Console.SetCursorPosition(0, Console.CursorTop - 3);
                        Helpers.ClearLine();
                        Helpers.CenterText("Please Enter the ID of the Goal you wish to modify.");
                        Helpers.ClearLine();
                        Console.WriteLine();
                        Helpers.ClearLine();
                        Helpers.CenterCursor();
                        userInput = Console.ReadLine();
                        if (userInput == "1" || userInput == "2")
                        {
                            int intInput = int.Parse(userInput);
                            Goals.ModifyGoals(intInput);
                        }
                        else 
                        {
                            Console.SetCursorPosition(0, Console.CursorTop - 3);
                            Helpers.ClearLine(); 
                            Helpers.CenterText("Invalid option Please Try Again. Press ENTER to Continue");
                            Helpers.ClearLine();
                            Console.ReadLine();

                        }
                        break;
                    case "X":
                        listGoals = false;
                        break;
                    default: break;
                }
            }


        }

        internal static void ListDailyTotals()
        {
            int sort = 0;
            bool dailyTotalsMenu = true;

            while (dailyTotalsMenu)
            {
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                String title = "DAILY TOTALS";
                List<DailyTotals> dailyTotals = Data.LoadDailyTotals();
                switch (sort)
                {
                    case 0:
                        dailyTotals = DailyTotals.DailyBubbleSort(dailyTotals);
                        break;
                    case 1:
                        dailyTotals = DailyTotals.ReverseDailyBubbleSort(dailyTotals);
                        break;
                    case 2:
                        //no sort
                        break;

                }
                DataTable dataTable = DailyTotals.BuildDataTable(dailyTotals);
                Helpers.ShowTable(dataTable, title);
                Console.WriteLine();
                Helpers.CenterText("|X: EXIT  |S: SORT");
                Console.WriteLine();
                string userInput = Console.ReadLine();
                userInput = userInput.Trim().ToUpper();

                switch (userInput)
                {
                    case "X":
                        dailyTotalsMenu = false;
                        break;
                    case "S":
                        sort = SortMenu(sort);
                        break;
                    default:
                        Console.WriteLine("Invalid Entry: Please press Any Key to coninue.");
                        Console.ReadKey();
                        break;
                }
            }

        }

        internal static void ListWeeklyTotals()
        {
            int sort = 0;
            bool weeklyTotalsMenu = true;

            while (weeklyTotalsMenu)
            {
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                String title = "WEEKLY TOTALS";
                List<WeeklyTotals> weeklyTotals = Data.LoadWeeklyTotals();
                switch (sort)
                {
                    case 0:
                        weeklyTotals = WeeklyTotals.WeeklyBubbleSort(weeklyTotals);
                        break;
                    case 1:
                        weeklyTotals = WeeklyTotals.ReverseWeeklyBubbleSort(weeklyTotals);
                        break;
                    case 2:
                        //no sort
                        break;

                }
                DataTable dataTable = WeeklyTotals.BuildDataTable(weeklyTotals);
                Helpers.ShowTable(dataTable, title);
                Console.WriteLine();
                Helpers.CenterText("|X: EXIT  |S: SORT");
                Console.WriteLine();
                string userInput = Console.ReadLine();
                userInput = userInput.Trim().ToUpper();

                switch (userInput)
                {
                    case "X":
                        weeklyTotalsMenu = false;
                        break;
                    case "S":
                        sort = SortMenu(sort);
                        break;
                    default:
                        Console.WriteLine("Invalid Entry: Please press Any Key to coninue.");
                        Console.ReadKey();
                     break;
                }
            }
        }

        internal static void LogSession()
        {
            DateOnly date = GetDate("Date Entry");
            TimeOnly start = GetTime("Start Time");
            TimeOnly end = start;
            while (end <= start)
            {
                end = GetTime("End Time");

                if(end <= start)
                {
                    Console.WriteLine("Invalid time press ENTER to Continue");
                    Console.ReadLine();
                }
            }
            DateTime dateTimeStart = Helpers.DateTimeBuilder(date, start);
            DateTime dateTimeEnd = Helpers.DateTimeBuilder(date, end);
            CodingSession session = new CodingSession(dateTimeStart,dateTimeEnd);
            session = CodingSession.SessionTime(session);
            Data.AddSession(session);

            CodingSession.CheckGoals(session, 1);
        }

        internal static DateOnly GetDate(string userInstruction)
        {

            bool validInput = false;
            bool topMenu = false;
            DateOnly dateInput = new DateOnly();

            while (!validInput && !topMenu)
            {
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                List<string> dateMenu = new List<string>();
                dateMenu.Add("X: Back");
                dateMenu.Add("T: Choose Today");
                DataTable dateMenuTable = MenuTable(dateMenu);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Helpers.ShowTable(dateMenuTable, userInstruction);
                Helpers.CenterText("| Insert the date: (Format: MM-DD-YYYY) |");
                Helpers.ClearLine();
                Console.WriteLine();
                Helpers.ClearLine();
                Helpers.CenterCursor();
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper().Trim();
                switch (userInput)
                {
                    case "T":
                        dateInput = DateOnly.FromDateTime(DateTime.Now);
                        validInput = true;
                        break;
                    case "X":
                        topMenu = true;
                        break;
                    default:
                        if (UserInput.CanParseDate(userInput))
                        {
                            dateInput = UserInput.ParseDate(userInput);
                            validInput = true;
                        }
                        else
                        {
                            Helpers.Output("Invalid Input press ENTER to Try Again.");
                            Console.ReadLine();
                        }
                        break;
                }
            }
            if (topMenu)
            {
                Menu.TopMenu();
            }
            return dateInput;

        }

        internal static TimeOnly GetTime(string userInstruction)
        {

            bool validInput = false;
            bool topMenu = false; 
            TimeOnly timeInput = new TimeOnly();
            while (!validInput && !topMenu)
            {
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                List<string> timeMenu = new List<string>();
                timeMenu.Add("X: Back");
                timeMenu.Add("N: Choose Now");
                DataTable timeMenuTable = MenuTable(timeMenu);
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Helpers.ShowTable(timeMenuTable, userInstruction);
                Helpers.CenterText("Insert the start time: (Format: HH:MM TT) Example: 2:25 PM.");
                Helpers.CenterCursor();
                string userInput = Console.ReadLine();
                userInput = userInput.ToUpper().Trim();
                switch (userInput)
                {
                    case "N":
                        timeInput = TimeOnly.FromDateTime(DateTime.Now);
                        validInput = true;
                        break;
                    case "X":
                        topMenu = true;
                        break;
                    default:
                        if (UserInput.CanParseTime(userInput))
                        { 
                            timeInput = UserInput.ParseTime(userInput);
                            validInput = true;
                        }
                        else
                        {
                          Helpers.Output("Invalid Input press ENTER to Try Again.");
                          Helpers.CenterCursor();
                          Console.ReadLine() ;
                        }
                        break;
                }
            }
            if (topMenu)
            {
                Menu.TopMenu();
            }
            return timeInput;

        }      
    }
}
