using CodingTrackerJMS.Model;
using ConsoleTableExt;

namespace CodingTrackerJMS;

public class UILogic
{
    Validation validation = new Validation();
    DatabaseLogic databaseLogic = new DatabaseLogic();
    string SQLorder = "DESC";

    string startDate;
    string endDate;
    int totalTime;
    DateTime startDateT;
    DateTime endDateT;

    public void OnGoingSession(string goalSelected, int timeToGoal)
    {
        TimeSpan elapsedTime = TimeSpan.Zero;
        bool sessionActive = true;
        startDate = DateTime.Now.ToString("yyyy/MM/dd; HH:mm");

        // The while loop will keep running and counting time on the console until the user presses e.
        while (!sessionActive == false)
        {
            elapsedTime = Chronometer(startDateT);
            Console.Clear();
            Console.WriteLine(@"Session currently in progress...
                                Press e to end current coding session!");
            Console.WriteLine($"Start Time: {startDate}");
            Console.WriteLine("Chronometer: " + elapsedTime.ToString(@"hh\:mm\:ss"));

            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.KeyChar == 'e')
                {
                    sessionActive = false;
                }
            }
        }

        endDate = DateTime.Now.ToString("yyyy/MM/dd; HH:mm");
        totalTime = (int)elapsedTime.TotalMinutes;

        Console.WriteLine("Session ended. Total session time: " + elapsedTime.ToString(@"hh\:mm\:ss") +
                            $" Ending Date: {endDate}");
        Console.ReadLine();

        databaseLogic.InsertRecord(startDate, endDate, totalTime, goalSelected, timeToGoal);
    }

    public TimeSpan Chronometer(DateTime startTimeT)
    {
        TimeSpan elapsedTime = DateTime.Now - startTimeT;
        Thread.Sleep(1000);
        return elapsedTime;
    }

    public void GetRecordInput(string userInput, string goalSelected, int timeToGoal, int id = 0)
    {
        bool isValid = false;
        while (isValid == false)
        {
            Console.WriteLine("Type the START DATE of the session (yyyy/mm/dd; HH:mm):");
            startDate = Console.ReadLine();
            isValid = validation.GetValidDate(startDate, isValid, out startDateT);
        }

        isValid = false;
        while (isValid == false)
        {
            Console.WriteLine("Type the END DATE of the session ((yyyy/mm/dd; HH:mm):");
            endDate = Console.ReadLine();
            isValid = validation.GetValidDate(endDate, isValid, out endDateT);
        }

        startDate = startDateT.ToString("yyyy/MM/dd; HH:mm");
        endDate = endDateT.ToString("yyyy/MM/dd; HH:mm");

        TimeSpan totalTimeT = endDateT - startDateT;
        totalTime = (int)totalTimeT.TotalMinutes;

        if (userInput == "a")
        {
            databaseLogic.InsertRecord(startDate, endDate, totalTime, goalSelected, timeToGoal);
        }
        else if (userInput == "u")
        {
            databaseLogic.UpdateRecord(id, startDate, endDate, totalTime, goalSelected, timeToGoal);
        }
    }

    public List<int> ViewSessionRecords()
    {
        Console.Clear();
        bool backToMainMenu = false;
        string inputFilter = "d";
        int countRecords = 0;
        int durationOfAllSessions = 0;
        int averageTimePerSession = 0;
        List<int> iDs = new List<int>();


        while (backToMainMenu == false)
        {
            List<CodingSession> sessionRecords = databaseLogic.GetSessionRecords(ref SQLorder);

            if (inputFilter == "d")
            {
                durationOfAllSessions = 0;
                countRecords = 0;
                averageTimePerSession = 0;

                foreach (var record in sessionRecords)
                {
                    countRecords++;
                    iDs.Add(record.Id);

                    durationOfAllSessions = +(durationOfAllSessions + record.TotalTime);
                }
                if (countRecords == 0)
                {
                    Console.WriteLine("There are no Records");
                    Console.ReadLine();
                }
                if (countRecords > 0)
                {
                    Console.Clear();
                    averageTimePerSession = durationOfAllSessions / countRecords;
                    ConsoleTableBuilder.From(sessionRecords).ExportAndWriteLine();
                }

            }

            TableFilterOptions(sessionRecords, ref durationOfAllSessions, ref countRecords, ref averageTimePerSession, ref inputFilter, ref backToMainMenu, ref SQLorder);

        }
        return iDs;
    }

    void TableFilterOptions(List<CodingSession> sessionRecords, ref int durationOfAllSessions, ref int countRecords, ref int averageTimePerSession, ref string inputFilter, ref bool backToMainMenu, ref string SQLorder)
    {
        Console.WriteLine(@"Continue:
                            b - Continue 
                            f - Filter list
                            r - Report on total session time and average time per session
                            oa - Order table ascendingly by total time
                            od - Order table descendingly by total time");
        string input = Console.ReadLine();

        if (input == "f")
        {
            durationOfAllSessions = 0;
            countRecords = 0;
            averageTimePerSession = 0;

            List<CodingSession> filteredSessions = new List<CodingSession>();

            Console.WriteLine(@"Choose a filter option:
                                y - Filter by year
                                m - Filer by month
                                w - Filer by week
                                u - Filter by day
                                d - Default view - All records)");
            inputFilter = Console.ReadLine();

            switch (inputFilter)
            {
                case "y":
                    Console.WriteLine("Type the year (yyyy):");
                    string desiredDate = Console.ReadLine();
                    countRecords = 0;

                    foreach (var record in sessionRecords)
                    {
                        if (record.StartDate.ToString("yyyy") == desiredDate)
                        {
                            filteredSessions.Add(record);
                            countRecords++;
                            durationOfAllSessions = +(durationOfAllSessions + record.TotalTime);
                        }
                    }
                    if (countRecords == 0)
                    {
                        Console.WriteLine("No records of sessions under those specifications exist!");
                        Console.ReadLine();
                    }
                    if (countRecords > 0)
                    {
                        Console.Clear();
                        ConsoleTableBuilder.From(filteredSessions).ExportAndWriteLine();
                        averageTimePerSession = durationOfAllSessions / countRecords;

                    }
                    break;

                case "m":
                    Console.WriteLine("Type the year and month (yyyy/MM):");
                    desiredDate = Console.ReadLine();
                    countRecords = 0;

                    foreach (var record in sessionRecords)
                    {
                        if (record.StartDate.ToString("yyyy/MM") == desiredDate)
                        {
                            filteredSessions.Add(record);
                            durationOfAllSessions = +(durationOfAllSessions + record.TotalTime);
                            countRecords++;
                        }
                    }
                    if (countRecords == 0)
                    {
                        Console.WriteLine("No records of sessions under those specifications exist!");
                        Console.ReadLine();
                    }
                    if (countRecords > 0)
                    {
                        Console.Clear();
                        averageTimePerSession = durationOfAllSessions / countRecords;
                        ConsoleTableBuilder.From(filteredSessions).ExportAndWriteLine();
                    }
                    break;

                case "u":
                    Console.WriteLine("Type the year, month and day (yyyy/MM/dd):"); //going to need to write a method to validate the input as a valid year!
                    desiredDate = Console.ReadLine();
                    countRecords = 0;

                    foreach (var record in sessionRecords)
                    {
                        if (record.StartDate.ToString("yyyy/MM/dd") == desiredDate)
                        {
                            filteredSessions.Add(record);
                            durationOfAllSessions = +(durationOfAllSessions + record.TotalTime);
                            countRecords++;
                        }
                    }
                    if (countRecords == 0)
                    {
                        Console.WriteLine("No records of sessions under those specifications exist!");
                        Console.ReadLine();
                    }
                    else if (countRecords > 0)
                    {
                        Console.Clear();
                        averageTimePerSession = durationOfAllSessions / countRecords;
                        ConsoleTableBuilder.From(filteredSessions).ExportAndWriteLine();
                    }
                    break;
            }
        }

        if (input == "b")
        {
            backToMainMenu = true;
        }

        if (input == "r")
        {
            Console.WriteLine($@"

The total duration of all coding sessions is {durationOfAllSessions} minutes!
The average time per session is {averageTimePerSession} minutes! In a total of {countRecords} sessions!");
            
            Console.ReadLine();
        }

        if (input == "oa")
        {
            SQLorder = "ASC";
        }

        if (input == "od")
        {
            SQLorder = "DESC";
        }

        if (input != "f" && input != "b" && input != "r" && input != "oa" && input != "od")
        {
            Console.WriteLine("Invalid Input!");
            Console.ReadLine();
            Console.Clear();
        }
    }

    public void UIDeleteRecord(int id)
    {
        databaseLogic.DeleteRecord(id);
    }

    public void GetGoalsList()
    {
        List<Goals> goals = databaseLogic.GetGoalsRecords();
        ConsoleTableBuilder.From(goals).ExportAndWriteLine();
        Console.ReadLine();
    }

    public void CreateGoal(out string goalSelected, out int timeToGoal)
    {
        Console.WriteLine("Write the name of the new goal:");
        goalSelected = Console.ReadLine();
        Console.WriteLine("Write the number of minutes in order to achieve the goal:");
        timeToGoal = Int32.Parse(Console.ReadLine());

        databaseLogic.InsertRecord(startDate = "0001/01/01; 01:10", endDate = "0001/01/01; 01:10", totalTime = 0, goalSelected, timeToGoal);
    }

    public void SetGoalActive(out string goalSelected, out int timeToGoal)
    {
        List<Goals> goals = databaseLogic.GetGoalsRecords();
        Console.WriteLine("Select the goal that you wish to set as active from the table.");
        goalSelected = Console.ReadLine();

        timeToGoal = 1;
        bool goalInexistent = true;

        foreach(Goals record in goals) 
        {
            if ( record.Goal == goalSelected)
            {
                timeToGoal = record.TimeToGoal;
                goalInexistent = false;
            }
        }
        if(goalInexistent == true)
        {
            Console.WriteLine("No goals with that name exist in the table!");
            Console.ReadLine();
            goalSelected = "GeneralCoding";
            timeToGoal = 1;
        }

    }

    public int GetTotalTimeOfGoal(string goalSelected)
    {
        List<CodingSession> sessionRecords = databaseLogic.GetSessionRecords(ref SQLorder);

        int totalTime = 0;

        foreach (CodingSession record in sessionRecords)
        {   
            if( record.Goal == goalSelected)
            {
                totalTime = totalTime + record.TotalTime;
            }
        }
        return totalTime;
    }

    public void CalculatePercentageGoalCompleted(float totalGoalTime, float timeToGoal, string goalSelected)
    {
        float percentageCompleted =  totalGoalTime / timeToGoal * 100;
        float timeUntilCompleted = timeToGoal - totalGoalTime;

        if(timeUntilCompleted < 0 || percentageCompleted > 100)
        {
            timeUntilCompleted = 0;
            percentageCompleted = 100;
        }

        Console.WriteLine($@"The goal {goalSelected} has a total completion time of {timeToGoal} minutes.
You have currently coded {totalGoalTime} minutes towards the Goal completing {percentageCompleted}% of the goal!
To complete the goal you still need to dedicate a total of {timeUntilCompleted} minutes towards the goal!
                              ");
        Console.ReadLine();
    }
}




