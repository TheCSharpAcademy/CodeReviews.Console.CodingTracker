using CodingTracker.rthring.Models;
using ConsoleTableExt;
using System.Globalization;

namespace CodingTracker.rthring
{
    public class UserHandler
    {
        DatabaseController Database;
        public UserHandler(DatabaseController Database)
        {
            this.Database = Database;
        }

        public bool GetUserInput()
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update record.");
            Console.WriteLine("------------------------------------\n");

            string command = Console.ReadLine();

            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    return true;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
                case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
            return false;
        }
        private void GetAllRecords()
        {
            Console.Clear();
            var result = Database.GetRecords();

            if (result.Count == 0)
            {
                Console.WriteLine("No rows found");
                return;
            }

            List<List<object>> tableData = new List<List<object>>();

            foreach (var session in result)
            {
                tableData.Add(new List<object>{
                    session.Id,
                    session.StartTime.ToString("yyyy/MM/dd HH:mm", new CultureInfo("en-US")),
                    session.EndTime.ToString("yyyy/MM/dd HH:mm", new CultureInfo("en-US")),
                    session.Duration});
            }
            ConsoleTableBuilder.From(tableData)
                .WithTitle("Coding Sessions").WithColumn("Id", "Start Time", "End Time", "Duration (Minutes)")
                .ExportAndWriteLine();
            return;
        }
        private void Insert()
        {
            if (!TryGetSessionInfo(out CodingSession session)) return;

            Database.InsertRecord(session);
            return;
        }

        private void Update()
        {
            GetAllRecords();

            Console.WriteLine("\n\nPlease type the Id of the record you want to update or type 0 to return to main menu.");
            int recordId = GetNumberInput();
            if (recordId == 0) return;
            bool exists = Database.RecordExistsById(recordId);

            while (!exists)
            {
                Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist. \n\n" +
                    $"Please type the Id of the record you want to update or type 0 to return to main menu.");
                recordId = GetNumberInput();
                exists = Database.RecordExistsById(recordId);
                if (recordId == 0) return;
            }

            if (!TryGetSessionInfo(out CodingSession session)) return;
            session.Id = recordId;
            Database.UpdateRecord(session);
        }

        private void Delete()
        {
            GetAllRecords();

            bool deleted = false;

            while (!deleted)
            {
                Console.WriteLine("\n\nPlease type the Id of the record you want to delete or type 0 to return to main menu.");
                int recordId = GetNumberInput();

                if (recordId == 0) return;
                deleted = Database.DeleteRecord(recordId);
                if (!deleted) Console.WriteLine($"\n\nRecord with Id {recordId} doesn't exist.");
            }
        }

        private static int GetNumberInput()
        {
            string numberInput = Console.ReadLine();

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)
            {
                Console.WriteLine("\n\nInvalid number. Try again.\n\n");
                numberInput = Console.ReadLine();
            }

            return Convert.ToInt32(numberInput);
        }

        private DateTime? GetTimeInput()
        {
            Console.WriteLine("Please insert date and time: (Format: yyyy/MM/dd HH:mm). Type 0 to return to main menu.");

            string dateInput = Console.ReadLine();

            while (!DateTime.TryParseExact(dateInput, "yyyy/MM/dd HH:mm", new CultureInfo("en-US"), DateTimeStyles.None, out _) && dateInput != "0")
            {
                Console.WriteLine("\n\nInvalid date. (Format: yyyy/MM/dd HH:mm). Type 0 to return to main menu or try again:\n\n");
                dateInput = Console.ReadLine();
            }

            if (dateInput == "0") return null;
            return DateTime.ParseExact(dateInput, "yyyy/MM/dd HH:mm", new CultureInfo("en-US"));
        }

        private bool TryGetSessionInfo(out CodingSession session)
        {
            session = new CodingSession();

            bool valid = false;
            while (!valid)
            {
                Console.WriteLine("\nStart Time: ");
                DateTime? startTime = GetTimeInput();
                if (startTime == null) return false;

                Console.WriteLine("\nEnd Time: ");
                DateTime? endTime = GetTimeInput();
                if (endTime == null) return false;

                session.StartTime = startTime ?? default;
                session.EndTime = endTime ?? default;
                session.Duration = Convert.ToInt32((session.EndTime.Subtract(session.StartTime)).TotalMinutes);

                if (session.Duration > 0) valid = true;
                else Console.WriteLine("\nInvalid times. The end time must be later than the start time.");
            }
            return true;
        }
    }
}
