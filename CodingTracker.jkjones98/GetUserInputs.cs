using System.Globalization;

namespace CodingTracker.jkjones98
{
    internal class GetUserInputs
    {
        MainMenu mainMenu = new();
        CodingController controller = new();
        internal void InsertRecord()
        {
            var date = GetDateInput();
            var startTime = GetTime("start");
            var endTime = GetTime("end");
            var duration = CalculateDuration(startTime, endTime);

            CodingSession coding = new();

            coding.Date = date;
            coding.StartTime = startTime;
            coding.EndTime = endTime;
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
            // Refactor all code - keep this as just a main menu and don't handle any data 
            // GetUserInputs will handle all data
            // Call relevant View/Insert/Delete/Update classes from GetUserInputs
            // Will need to create a method in CodingController that gets current end or start time dependent on 
            // Which time has been updated and then recalculate the duration - so need to SELECT {endOrStart} FROM coding WHERE Id = id
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

        internal string GetDateInput()
        {
            Console.WriteLine("\nPlease enter the date in the following format: dd-mm-yy or enter 0 to return to main menu");
            string dateInput = Console.ReadLine();

            if(dateInput == "0") mainMenu.DisplayMenu();

            while(!DateTime.TryParseExact(dateInput, "dd-mm-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
            {
                Console.WriteLine("\nInvalid date format please enter with the following format: dd-mm-yy or enter 0 to return to the main menu");
                dateInput = Console.ReadLine();

                if(dateInput == "0") mainMenu.DisplayMenu();
            }

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

            string parsedDuration = duration.ToString(@"hh\:mm");

            return parsedDuration;
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
                    coding.Date = GetDateInput();
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