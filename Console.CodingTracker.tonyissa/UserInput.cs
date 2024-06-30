using CodingTracker.Validation;

namespace CodingTracker.UserInput
{
    public static class UserInput
    {
        public static string GetDateInput()
        {
            var input = Console.ReadLine() ?? "quit";
            input = input.Trim().ToLower();

            if (input == "quit" || InputValidator.ValidateDateInput(input))
            {
                return input;
            } 
            else 
            {
                throw new ArgumentException("Your date was not in the correct format. The correct format is MM/DD/YYYY. Please try again.");
            }
        }

        public static string GetTimeInput()
        {
            var input = Console.ReadLine() ?? "quit";
            input = input.Trim().ToLower();

            if (input == "quit" || InputValidator.ValidateTimeInput(input))
            {
                return input;
            }
            else
            {
                throw new ArgumentException("Your time was not in the correct format. The correct format is (24-hour clock) HH:MM. Please try again.");
            }
        }
    }
}