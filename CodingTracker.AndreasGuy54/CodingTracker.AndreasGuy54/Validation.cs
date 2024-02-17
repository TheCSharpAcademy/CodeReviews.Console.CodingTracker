using System.Globalization;

namespace CodingTracker.AndreasGuy54
{
    internal static class Validation
    {
        internal static bool ValidInput(string userInput)
        {
            bool isValidated = false;
            int convertedInput;

            if (int.TryParse(userInput.ToLower().Trim(), out _))
            {
                isValidated = true;
                convertedInput = Convert.ToInt32(userInput);

                if (convertedInput < 0 || convertedInput > 4)
                {
                    isValidated = false;
                }
            }
            return isValidated;
        }

        internal static string ValidateDateInput(string dateInput)
        {
            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy HH:mm", new CultureInfo("en-UK"), DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine("\n\nInvalid data. Format = dd-MM-yy HH:mm.\n");
                dateInput = Console.ReadLine();
            }
            return dateInput;
        }

        internal static bool IsValidatedTimes(string startDate, string endDate)
        {
            bool validated = true;

            if (DateTime.Parse(endDate) < DateTime.Parse(startDate))
            {
                validated = false;
            }

            return validated;
        }
    }
}
