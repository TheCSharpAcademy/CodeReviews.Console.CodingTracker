using System.Globalization;

namespace CodingTracker.jkjones98
{
    internal class GetUserInputs
    {
        MainMenu mainMenu = new();
        CodingController controller = new();
        internal void ViewRecords()
        {
            controller.GetRecords();

            Console.WriteLine("\nDo you want to filter the records? See options below");
            Console.WriteLine("Enter 0 to return to the main menu");
            Console.WriteLine("Enter 1 to filter by month");
            Console.WriteLine("Enter 2 to filter by year");

            string filterChoice = Console.ReadLine();

            switch(filterChoice)
            {
                case "1":
                    Console.Clear();
                    Console.WriteLine("Please enter the numeric value for the month you would like to filter using a 2 digit format");
                    Console.WriteLine("January would be entered as 01 or enter 0 to return to the main menu");
                    string month = Console.ReadLine();
                    
                    while(string.IsNullOrEmpty(month) || !Int32.TryParse(month, out _) || Int32.Parse(month) < 0 || Int32.Parse(month) > 12)
                    {
                        Console.WriteLine("\nInvalid entry please try again");
                        month = Console.ReadLine();
                    }
                    if(month == "0") mainMenu.DisplayMenu();

                    controller.GetFilterRecords("-" + month);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("Please enter the last 2 digits of the year you would like to filter e.g. 2022 = 22, 2023 = 23");
                    Console.WriteLine("Or enter 0 to return to the main menu");
                    string year = Console.ReadLine();
                    
                    while(string.IsNullOrEmpty(year) || !Int32.TryParse(year, out _) || Int32.Parse(year) < 0 || Int32.Parse(year) > 23)
                    {
                        Console.WriteLine("\nInvalid entry please try again");
                        month = Console.ReadLine();
                    }
                    if(year == "0") mainMenu.DisplayMenu();

                    controller.GetFilterRecords("-" + year);
                    break; 
                default:
                    break;
            }
        }
        internal void InsertRecord()
        {
            string startDateTime = GetDateInput("start") + " " + GetTime("start");
            DateTime startDateTimeParsed = DateTime.Parse(startDateTime);
            string endDateTime = GetDateInput("end") + " " + GetTime("end");
            DateTime endDateTimeParsed = DateTime.Parse(endDateTime);
            while(endDateTimeParsed < startDateTimeParsed)
            {
                Console.WriteLine("\nInvalid end date, end date cannot be before start date");
                endDateTime = GetDateInput("end") + " " + GetTime("end");
                endDateTimeParsed = DateTime.Parse(endDateTime);
            }
            var duration = CalculateDuration(startDateTimeParsed, endDateTimeParsed);

            CodingSession coding = new();

            coding.Date = startDateTimeParsed.ToString("dd-MM-yy");
            coding.StartTime = startDateTimeParsed.ToString("HH:mm");
            coding.EndTime = endDateTimeParsed.ToString("HH:mm");
            coding.Duration = duration;

            controller.InsertRecordDb(coding);
        }

        internal void DeleteRecord()
        {
            controller.GetRecords();
            var id = GetById("delete");

            var deleteRow = controller.CheckIdExists(id);

            while(deleteRow.Id == 0)
            {
                Console.WriteLine($"\nRecord with Id {id} does not exist");
                DeleteRecord();
            }

            controller.DeleteRecordDb(id);
        }

        internal void UpdateRecord()
        {
            controller.GetRecords();

            var id = GetById("update");

            var updateRow = controller.CheckIdExists(id);

            while(updateRow.Id == 0)
            {
                Console.WriteLine($"\nRecord with Id {id} does not exist");
                UpdateRecord();
            }

            UpdateWhichRecord(updateRow);
        }

        internal void StopWatchMenu()
        {
            Console.WriteLine("\nPlease enter s to start the timer or enter 0 to return to the main menu");
            var timerInput = Console.ReadLine();

            if(timerInput == "0") mainMenu.DisplayMenu();

            if(timerInput != "s")
            {
                Console.WriteLine("\nInvalid input please enter s to start the timer or enter 0 to return to the main menu");
                timerInput = Console.ReadLine();
                if(timerInput == "0") mainMenu.DisplayMenu();
            }

            DateTime currentDateTimeStart = DateTime.Now;

            Console.WriteLine("\nEnter f to stop the timer");
            string endTimer = Console.ReadLine();

            while(endTimer != "f")
            {
                Console.WriteLine("\nIncorrect input, timer still running, enter f to end the timer");
                endTimer = Console.ReadLine();
            }

            DateTime currentDateTimeEnd = DateTime.Now;

            string stpWatchDuration = CalculateDuration(currentDateTimeStart, currentDateTimeEnd);

            CodingSession coding = new();

            coding.Date = currentDateTimeStart.ToString("dd-mm-yy");
            coding.StartTime = currentDateTimeStart.ToString("HH:mm");
            coding.EndTime = currentDateTimeEnd.ToString("HH:mm");
            coding.Duration = stpWatchDuration;

            controller.InsertRecordDb(coding);
        }

        internal void AddGoals()
        {
            Console.WriteLine("\n\nPlease enter 1-2 words to summarise your goal i.e. Software Developer or Pro Baller");
            Console.WriteLine("Or enter 0 to return to the main menu");
            string goalName = Console.ReadLine();
            
            if(goalName == "0") mainMenu.DisplayMenu();

            while(String.IsNullOrEmpty(goalName) || Int32.TryParse(goalName, out _))
            {
                Console.WriteLine("\nMust be 1 - 2 words and cannot only be numbers, please enter again or enter 0 to return to main menu");
                goalName = Console.ReadLine();
                if(goalName == "0") mainMenu.DisplayMenu();
            }

            Console.WriteLine("\n\nPlease enter the total amount of hours it will take to complete your goal using the format: 10000.00");
            Console.WriteLine("\nEnter 0 if you wish to return to the main menu");
            string goalHours = Console.ReadLine();

            if(goalHours == "0") mainMenu.DisplayMenu();

            while(!float.TryParse(goalHours, out _) || String.IsNullOrEmpty(goalHours))
            {
                Console.WriteLine("\nIncorrect format, please try again or enter 0 to return to the main menu");
                goalHours = Console.ReadLine();
                if(goalHours == "0") mainMenu.DisplayMenu();
            }

            GoalTracking goals = new();

            goals.GoalName = goalName;
            goals.GoalHours = float.Parse(goalHours);
            goals.HoursDone = controller.GetDurationSum();
            goals.HoursLeft = goals.GoalHours - goals.HoursDone;

            controller.InsertRecordDb(goals);
        }

        internal string GetDateInput(string startOrEnd)
        {
            Console.WriteLine($"\nPlease enter the {startOrEnd} date in the following format: dd-mm-yy or enter 0 to return to main menu");
            string dateInput = Console.ReadLine();

            if(dateInput == "0") mainMenu.DisplayMenu();

            while(!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid date format please enter with the following format: dd-mm-yy or enter 0 to return to the main menu");
                dateInput = Console.ReadLine();

                if(dateInput == "0") mainMenu.DisplayMenu();
            }

            DateTime test = DateTime.ParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None);

            return dateInput;
        }

        internal string GetTime(string startOrEnd)
        {
            Console.WriteLine($"\nPlease enter the {startOrEnd} time using a 24 hour format e.g: hh:mm ");
            Console.WriteLine("\ni.e. 9:30pm would be written as 21:30 or enter 0 to return to the main menu");
            string timeInput = Console.ReadLine();

            if(timeInput == "0") mainMenu.DisplayMenu();

            while(!TimeSpan.TryParseExact(timeInput, "h\\:mm", CultureInfo.InvariantCulture, out _))
            {
                Console.WriteLine("\nInvalid time format please enter using a 24 hour format e.g.: hh:mm");
                Console.WriteLine("\ni.e. 3:30pm would be written as 15:30");
                timeInput = Console.ReadLine();
                if(timeInput == "0") mainMenu.DisplayMenu(); 
            }

            return timeInput;
        }
        
        internal string CalculateDuration(string start, string end)
        {
            TimeSpan duration = TimeSpan.Parse(start).Subtract(TimeSpan.Parse(end));
            return duration.ToString(@"hh\:mm");;
        }
        
        internal string CalculateDuration(DateTime startDateTime, DateTime endDateTime)
        {
            TimeSpan duration = startDateTime.Subtract(endDateTime);
            return duration.ToString(@"hh\:mm");
        }

        internal int GetById(string updOrDel)
        {
            Console.WriteLine($"\nPlease enter the Id of the record you would like to {updOrDel}.");
            Console.WriteLine("Alternatively, enter 0 to return to the main menu");

            var id = Console.ReadLine();
            if(id == "0") mainMenu.DisplayMenu();

            while(!Int32.TryParse(id, out _) || Int32.Parse(id) < 0 || string.IsNullOrEmpty(id))
            {
                Console.WriteLine("\nInvalid Id entered, try again or enter 0 to return to the main menu");
                id = Console.ReadLine();
                if(id == "0") mainMenu.DisplayMenu();
            }

            var parsedId = Int32.Parse(id);

            return parsedId;
        }

        internal void UpdateWhichRecord(CodingSession coding)
        {
            Console.WriteLine("\nSelect what you would like to update");
            Console.WriteLine("Enter d for date");
            Console.WriteLine("Enter s for Start Time");
            Console.WriteLine("Enter e for End Time");
            Console.WriteLine("Duration will automatically update for the selected Id");

            var updateInput = Console.ReadLine();

            switch(updateInput)
            {
                case "d":
                    coding.Date = GetDateInput("updated");
                    break;
                case "s":
                    coding.StartTime = GetTime("start");
                    coding.EndTime = controller.GetStartOrEndTime("EndTime", coding.Id);
                    coding.Duration = CalculateDuration(coding.StartTime, coding.EndTime);
                    break;
                case "e":
                    coding.EndTime = GetTime("end");
                    coding.StartTime = controller.GetStartOrEndTime("StartTime", coding.Id);
                    coding.Duration = CalculateDuration(coding.StartTime, coding.EndTime);
                    break;
                default:
                    Console.WriteLine("\nInvalid selection please try again");
                    UpdateWhichRecord(coding);
                    break;
            }

            controller.UpdateRecordDb(coding);
        }
    }
}