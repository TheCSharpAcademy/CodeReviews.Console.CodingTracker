using CodingTracker.alvaromosconi.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace CodingTracker.alvaromosconi.ConsoleUI
{
    internal static class UserInput
    {
        private const string BACK = "0";

        internal static char SelectMenuOption(string OPTIONS)
        {
            Console.Write("Select: ");
            var selectedOption = Console.ReadKey().KeyChar;

            if (!OPTIONS.Contains(selectedOption))
            { 
                Console.WriteLine("\nInvalid option. Please select a valid option.\n");

                selectedOption = SelectMenuOption(OPTIONS);
            }

            return selectedOption;
        }

        internal static string GetDateRangeFromUser(string rangeType)
        {
            string dateFormat = "dd-MM-yy HH:mm";

            Console.WriteLine($"\nPlease enter the {rangeType} date in the following format: [{dateFormat}].");
            string date = Console.ReadLine()!.Trim().ValidateDate(dateFormat);

            return date;
        }

        private static string ValidateDate(this string input, string dateFormat)
        {
            DateTime output = DateTime.MinValue;

            while (!DateTime.TryParseExact(input,
                                            dateFormat,
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                                            out output))
            {
                if (String.IsNullOrEmpty(input))
                    return BACK;

                Console.Write("\nInvalid input. Please try again: ");
                input = Console.ReadLine()!.Trim();
            }

            return output.ToString(dateFormat);
        }

        internal static int GetIdFromUser(List<CodeSessionModel> sessions)
        {
            Console.WriteLine("Please enter the ID of the session that you want to be deleted.");
            Console.Write("\nID: ");
            string userInput = Console.ReadLine()!.Trim().ValidateId(sessions);

            return int.Parse(userInput);
        }

        private static string ValidateId(this string userInput, List<CodeSessionModel> sessions)
        {
            int output;
            bool sessionExists = false;

            while (!Int32.TryParse(userInput, out output) || !sessionExists)
            {
                sessionExists = sessions.Exists(CodeSessionModel => CodeSessionModel.Id == output); 
                if (!sessionExists)
                {
                    Console.Write("Id not found. Please try again: ");
                    userInput = Console.ReadLine()!.Trim().ValidateId(sessions);
                }
            }

            return userInput;
        }
    }
}
