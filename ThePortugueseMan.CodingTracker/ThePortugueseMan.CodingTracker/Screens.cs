using ConsoleTableExt;

namespace ThePortugueseMan.CodingTracker;

internal class Screens
{
    AskInput askInput = new();
    Format format = new();
    DbCommands dbCmds = new();
    ListOperations listOp = new();
    GoalOperations goalOp = new();

    public void MainMenu()
    {
        bool exitMenu = false;
        List<object> optionsString = new List<object> { 
            "1 - View Logs", 
            "2 - Insert Log", 
            "3 - Update Log", 
            "4 - Delete Log",
            "5 - View Reports",
            "6 - Goal",
            "0 - Exit App"};

        while (!exitMenu) 
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Coding Tracker")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exitMenu = true; break;
                case 1: ViewLogsMenu(); break;
                case 2: InsertLogsMenu(); break;
                case 3: UpdateLogMenu(); break;
                case 4: DeleteLogScreen(); break;
                case 5: ReportsMenu(); break;
                case 6: GoalsMenu(); break;
                default: break;

            }
        }
        return;
    }
    
    private void GoalsMenu()
    {
        goalOp.UpdateGoals();
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            List<object> optionsString = new List<object> {
                "1 - View current goal",
                "2 - Set goal",
                "3 - View All goals",
                "4 - Delete goals",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Goals")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; continue;
                case 1: Console.Clear(); CurrentGoalDisplay(); break;

                case 2:
                    InsertGoal();
                    break;
                case 3:
                    Console.Clear();
                    ViewGoals();
                    break;
                case 4:
                    Console.Clear();
                    DeleteGoals();
                    break;
                default: continue;
            }
            askInput.AnyKeyToContinue();
        }
    }

    private void CurrentGoalDisplay()
    {
        Goal activeGoal = goalOp.GetActiveGoal();
        if (activeGoal != null)
        {
            TimeSpan daysLeftToEnd = activeGoal.EndDate.Date.Subtract(DateTime.Now.Date);
            if (daysLeftToEnd < TimeSpan.Zero) daysLeftToEnd = TimeSpan.Zero;
            TimeSpan timeLeftToGoal = activeGoal.TargetHours.Subtract(activeGoal.HoursSpent);
            if (timeLeftToGoal < TimeSpan.Zero) daysLeftToEnd = TimeSpan.Zero;
            var tableList = new List<List<object>>
            {
                new List<object>{"Start Date", format.DateToDisplayString(activeGoal.StartDate)},
                new List<object>{"End Date", format.DateToDisplayString(activeGoal.EndDate)},
                new List<object>{"Target time", format.TimeSpanToString(activeGoal.TargetHours)},
                new List<object>{"Time spent", format.TimeSpanToString(activeGoal.HoursSpent)},
                new List<object>{"Days Left", daysLeftToEnd.TotalDays},
                new List<object>{"Time needed", format.TimeSpanToString(timeLeftToGoal)},
                new List<object>{"Time/day needed", format.TimeSpanToString(timeLeftToGoal.Divide(daysLeftToEnd.TotalDays))},
            };

            ConsoleTableBuilder.From(tableList)
                .WithTitle("Goal")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
            Console.Write("\n");

        }

        else Console.WriteLine("\nThere's no active goal...\n");
    }

    private void ViewGoals()
    {
        List<Goal> listToDisplay = dbCmds.ReturnAllGoalsInTable();
        var tableDataDisplay = new List<List<object>>();
        if (listToDisplay is not null)
        {
            foreach (Goal goal in listToDisplay)
            {
                string timeLeftDisplay;
                TimeSpan timeLeft = goal.TargetHours.Subtract(goal.HoursSpent);
                if (timeLeft <= TimeSpan.Zero) timeLeftDisplay = "00:00";
                else timeLeftDisplay = timeLeft.ToString("hh\\:mm");

                tableDataDisplay.Add(
                    new List<object>
                    {
                    goal.Id,
                    goal.StartDate.ToString("dd-MM-yy"), goal.EndDate.ToString("dd-MM-yy"),
                    goal.TargetHours.ToString("hh\\:mm"), goal.HoursSpent.ToString("hh\\:mm"),
                    timeLeftDisplay, goal.Status
                    }) ;
            }
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle("Goals")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "End date", "Target time", "Time Spent", "Time left", "Status")
                .ExportAndWriteLine();
        }
        else
        {
            tableDataDisplay.Add(new List<object> { "", "", "", "", "", "" });
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle("EMPTY")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        return;
    }

    private void DeleteGoals()
    {
        int index;
        bool showError = false, exit = false;

        while (!exit)
        {
            Console.Clear();
            ViewGoals();
            do
            {
                Console.Write("\n");
                if (!showError) index = askInput.PositiveNumber("Select the index you want to delete. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExistsInTable(index, "Goals") && exit == false);

            if (!exit)
            {
                if (dbCmds.DeleteByIndex(index, "Goals")) Console.WriteLine("Log successfully deleted");
                else Console.WriteLine("Couldn't delete log...");
                askInput.AnyKeyToContinue();
            }
        }
        return;
    }

    private void InsertGoal()
    {
        if (goalOp.GetActiveGoal() != null)
        {
            Console.WriteLine("There's already an active goal");
            return; 
        }
        DateTime[] interval = askInput.DateInterval("Insert a start date.", "Insert an end date.");
        TimeSpan targetHours = 
            TimeSpan.FromHours(askInput.PositiveNumber("Insert the number of hours you want to code."));

        TimeSpan hoursSpent =
            listOp.TotalTimeInList(listOp.GetLogsBetweenDates(
                dbCmds.ReturnAllLogsInTable(), interval[0], interval[1]));

        Goal goalToInsert = new Goal
        {
            StartDate = interval[0],
            EndDate = interval[1],
            TargetHours = targetHours,
            HoursSpent = hoursSpent
        };
            

        if (dbCmds.Insert(goalToInsert)) Console.WriteLine("Goal inserted successfully!");
        else Console.WriteLine("Can't insert goal...");

    }

    private void ReportsMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - Overview",
                "2 - Date interval",
                "3 - Last X",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("View")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; continue;
                case 1:
                    Console.Clear();
                    ReportsView(dbCmds.ReturnAllLogsInTable());
                    break;
                case 2:
                    DateTime[] interval = askInput.DateInterval("Insert a start date.", "Insert an end date.");
                    Console.Clear();
                    ReportsView(listOp.GetLogsBetweenDates(dbCmds.ReturnAllLogsInTable(),
                        interval[0], interval[1]));
                    
                    break;
                case 3: break;
                default: continue;
            }
            askInput.AnyKeyToContinue("\n");
        }
    }

    private void ReportsView(List<CodingSession> listToReport)
    {
        if (listToReport == null)
        {
            Console.WriteLine("Nothing to report on");
            return;
        }
        else 
        {
            string totalTime = format.TimeSpanToString(listOp.TotalTimeInList(listToReport));
            string totalSessions = listOp.NumberOfSessionsInList(listToReport).ToString();
            string averageTime = format.TimeSpanToString(listOp.AverageTimeInList(listToReport));
            string firstDate = format.DateToMainDbString(listOp.FirstDateInList(listToReport));
            string lastDate = format.DateToMainDbString(listOp.LastDateInList(listToReport));
            string diffBetweenFirstAndLast = listOp.DifferenceBetweenFirsAndLastDates(listToReport).Days.ToString();

            var tableList = new List<List<object>>
            {
                new List<object>{"Total time spent", totalTime},
                new List<object>{"Number of sessions", totalSessions},
                new List<object>{"Number of days", diffBetweenFirstAndLast},
                new List<object>{"Average time per session", averageTime},
            };

            ConsoleTableBuilder.From(tableList)
                .WithTitle($"{firstDate} to {lastDate}")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .ExportAndWriteLine();
        }
        return;
    }
   
    private void ViewLogsMenu()
    {
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - View All Logs",
                "2 - View Ordered Logs",
                "3 - View Logs in date interval",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("View")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; break;
                case 1:
                    Console.Clear();
                    DisplaySessions(dbCmds.ReturnAllLogsInTable(), "VIEW ALL");
                    askInput.AnyKeyToContinue("\nPress any key to return");
                    break;
                case 2:
                    ViewOrderedLogsMenu();
                    break;
                case 3: ViewLogsInDateInterval(); break;
                default: break;
            }
        }
    }
    
    private void ViewLogsInDateInterval()
    {
        CodingSession codingSession = new();
        List<CodingSession> listToDisplay = new();

        DateTime[] interval = askInput.DateInterval("Insert the start date", "Insert the end date");
        listToDisplay = listOp.GetLogsBetweenDates(dbCmds.ReturnAllLogsInTable(), interval[0], interval[1]);

        Console.Clear();
        Console.Write('\n');
        DisplaySessions(listToDisplay, $"{interval[0].ToString("dd-MM-yy")} to " +
            $"{interval[1].ToString("dd-MM-yy")}");
        askInput.AnyKeyToContinue();
        return;
    }
    
    private void ViewOrderedLogsMenu()
    {
        bool exitMenu = false;
        List<string> optionsString = new List<string> {
            "1 - Ascending date",
            "2 - Descending date",
            "0 - Return"};


        while (!exitMenu)
        {
            Console.Clear();
            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("View")
                .ExportAndWriteLine();
            Console.Write("\n");
            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exitMenu = true; break;
                case 1:
                    ViewOrderedLogs("ASCENDING");
                    askInput.AnyKeyToContinue("\nPress any key to return");
                    break;
                case 2:
                    ViewOrderedLogs("DESCENDING");
                    askInput.AnyKeyToContinue("\nPress any key to return");
                    break;
                case 3: break;
                default: break;
            }
        }
    }
    
    private void ViewOrderedLogs (string? order)
    {
        List<CodingSession> listToDisplay;
        Console.Clear();
        if (order == "ASCENDING") listToDisplay = listOp.ReturnOrderedByAscendingDate(dbCmds.ReturnAllLogsInTable());

        else if (order == "DESCENDING") listToDisplay = listOp.ReturnOrderedByDescendingDate(dbCmds.ReturnAllLogsInTable());
        else throw new Exception($"ViewOrderedLogs order not valid: {order}");
        DisplaySessions(listToDisplay, order);
        return;
    }
    
    private void DisplaySessions(List<CodingSession> listToDisplay, string title)
    {
        var tableDataDisplay = new List<List<object>>();
        if (listToDisplay is not null)
        {
            foreach (CodingSession session in listToDisplay)
            {
                tableDataDisplay.Add(
                    new List<object>
                    {
                    session.Id,
                    format.DateToDisplayString(session.StartDateTime), format.DateToTimeString(session.StartDateTime),
                    format.DateToDisplayString(session.EndDateTime), format.DateToTimeString(session.EndDateTime),
                    format.TimeSpanToString(session.Duration)
                    });
            }
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle(title)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        else 
        {
            tableDataDisplay.Add(new List<object> { "", "", "", "", "", "" });
            ConsoleTableBuilder.From(tableDataDisplay)
                .WithTitle("EMPTY")
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Id", "Start date", "Start time", "End date", "End time", "Duration")
                .ExportAndWriteLine();
        }
        return;
    }
    
    private void InsertLogsMenu()
    {
        bool exit = false;
        while(!exit)
        {
            Console.Clear();
            List<string> optionsString = new List<string> {
                "1 - Manual Insert",
                "2 - StopWatch",
                "0 - Return"};

            ConsoleTableBuilder.From(optionsString)
                .WithFormat(ConsoleTableBuilderFormat.Alternative)
                .WithColumn("Insert")
                .ExportAndWriteLine();
            Console.Write("\n");

            switch (askInput.PositiveNumber("Please select an option or press 0 to exit"))
            {
                case 0: exit = true; continue;
                case 1: ManualLogInsert(); break;
                case 2: StopWatchInsert(); break;
                default: break;
            }
            askInput.AnyKeyToContinue();
        }
    }
    
    private void ManualLogInsert()
    {
        DateTime[] interval = askInput.DateIntervalWithHours("Insert the start date.", "Insert the end date.");

        if (interval is null) return;
        if (listOp.TotalTimeBetweenDates(dbCmds.ReturnAllLogsInTable(), interval[0], interval[1]) != TimeSpan.Zero)
        {
            Console.WriteLine("Time overlap. Couldn't insert log...");
            return;
        }

        CodingSession sessionToInsert = new(interval[0], interval[1], interval[1].Subtract(interval[0]));
        if (dbCmds.Insert(sessionToInsert)) Console.WriteLine("Entry was logged successfully");
        else Console.WriteLine("Couldn't insert log...");

        return;
    }
    
    private void StopWatchInsert()
    {
        bool exit = false;
        exit = askInput.ZeroOrOtherAnyKeyToContinue(
            "Press any key to start the stopwatch. Or press 0 to return");

        if(!exit)
        {
            askInput.ClearPreviousLines(1);
            Console.WriteLine("Press any key to stop.");
            CodingSession sessionToInsert = new();
            sessionToInsert.StartDateTime = DateTime.Now;

            while (!Console.KeyAvailable)
            {
                TimeSpan duration =
                    DateTime.Now.Subtract(sessionToInsert.StartDateTime);
                Console.Write(duration.ToString("hh\\:mm\\:ss"));
                Console.SetCursorPosition(0, Console.CursorTop);
            }

            Console.Write("\n");
            Console.ReadKey();

            sessionToInsert.EndDateTime = DateTime.Now;
            sessionToInsert.Duration =
                sessionToInsert.EndDateTime.Subtract(sessionToInsert.StartDateTime);
            
            if (sessionToInsert.Duration <= TimeSpan.FromMinutes(5))
            {
                Console.WriteLine("This session was too short to register.");
            }
            else
            {
                Console.WriteLine($"\nThis session was {format.TimeSpanToString(sessionToInsert.Duration)}");
                dbCmds.Insert(sessionToInsert);
            }
        }
        return;
    }
    
    private void UpdateLogMenu()
    {
        int index;
        bool showError = false, exit = false;

        while (!exit)
        {
            Console.Clear();
            DisplaySessions(dbCmds.ReturnAllLogsInTable(), "UPDATE");
            do
            {
                Console.Write("\n");
                if (!showError) index = askInput.PositiveNumber("Select the index you want to update. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExistsInTable(index, "Main") && exit == false);

            if (exit) continue;

            DateTime[] interval = askInput.DateIntervalWithHours("Insert the new start date.", "Insert the new end date.");

            if (interval is null) return;

            CodingSession sessionToInsert = new(interval[0], interval[1], interval[1].Subtract(interval[0]));
            if (dbCmds.Update(index, sessionToInsert)) Console.WriteLine("Entry was updated successfully");
            else Console.WriteLine("Couldn't update log...");
            askInput.AnyKeyToContinue();
            return;
        }
        return;
    }
    
    private void DeleteLogScreen()
    {
        int index;
        bool showError = false, exit = false;

        while(!exit)
        {
            Console.Clear();
            DisplaySessions(dbCmds.ReturnAllLogsInTable(), "DELETE");
            do
            {
                Console.Write("\n");
                if (!showError) index = askInput.PositiveNumber("Select the index you want to delete. Or press 0 to return");
                else index = askInput.PositiveNumber("Please select a valid index");
                if (index == 0) exit = true;
            } while (!dbCmds.CheckIfIndexExistsInTable(index, "Main") && exit == false);

            if (!exit) 
            {
                if (dbCmds.DeleteByIndex(index, "Main")) Console.WriteLine("Log successfully deleted");
                else Console.WriteLine("Couldn't delete log...");
                askInput.AnyKeyToContinue();
            }
        }
        return;
    }
}
