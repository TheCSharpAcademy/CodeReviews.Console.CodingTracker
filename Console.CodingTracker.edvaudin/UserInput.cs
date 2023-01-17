using System.Globalization;

namespace CodingTracker
{
    internal static class UserInput
    {
        public static string GetStartTime()
        {
            string input = Console.ReadLine();

            while (!Validator.IsValidDateInput(input))
            {
                Console.WriteLine("\nInvalid date and time. Use the format: dd-MM-yy HH-mm-ss.");
                input = Console.ReadLine();
            }
            return input;
        }

        public static string GetEndTime(DateTime startTime)
        {
            string input = Console.ReadLine();

            while (!Validator.IsValidDateInput(input))
            {
                Console.WriteLine("\nInvalid date and time. Use the format: dd-MM-yy HH-mm-ss.");
                input = Console.ReadLine();
            }
            if (!Validator.IsDateAfterStartTime(input, startTime))
            {
                Console.WriteLine("\nYou cannot have finished coding before you started! Enter a different end time.");
                input = GetEndTime(startTime);
            }
            return input;
        }

        public static int GetIdForUpdate()
        {
            DataAccessor dal = new();
            List<int> validIds = dal.GetCodingSessions().Select(o => o.Id).ToList();
            bool validIdEntered = false;
            while (!validIdEntered)
            {
                if (int.TryParse(Console.ReadLine(), out int result))
                {
                    if (validIds.Contains(result) || result == -1)
                    {
                        validIdEntered = true;
                        return result;
                    }
                }
                Console.Write("\nThis is not a valid id, please enter a number or to return to main menu type '-1': ");
            }
            return -1;
        }

        public static string GetUserFilterChoice()
        {
            string input = Console.ReadLine();
            while (!Validator.IsValidFilterOption(input))
            {
                Console.Write("\nThis is not a valid input. Please enter one of the above options: ");
                input = Console.ReadLine();
            }
            return input;
        }

        public static string GetUserOption()
        {
            string input = Console.ReadLine();
            while (!Validator.IsValidOption(input))
            {
                Console.Write("\nThis is not a valid input. Please enter one of the above options: ");
                input = Console.ReadLine();
            }
            return input;
        }
    }
}
