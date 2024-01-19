using Spectre.Console;
using System.Globalization;

namespace frockett.CodingTracker.Library
{
    public class UserInputValidationService
    {
        public CodingSession GetStartEndTimeInput()
        {
            DateTime startTime = GetDateTime("Enter start time (DD-MM-YYYY HH:mm) || Please use 24-hour clock: ");
            DateTime endTime = GetDateTime("Enter end time (DD-MM-YYYY HH:mm) || Please use 24-hour clock: ");

            return new CodingSession { StartTime = startTime, EndTime = endTime, Duration = endTime - startTime };
        }


        public int GetSessionId(string prompt)
        {
            int sessionId = AnsiConsole.Ask<int>(prompt);
            return sessionId;
        }


        private DateTime GetDateTime(string prompt)
        {
            DateTime dateTime;
            string validFormat = "dd-MM-yyyy HH:mm";

            var sDateTime = AnsiConsole.Ask<string>(prompt);
            
            while (!DateTime.TryParseExact(sDateTime, validFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                AnsiConsole.WriteLine("\nIncorrect date/time format.");
                sDateTime = AnsiConsole.Ask<string>(prompt);
            }

            return dateTime;
        }
    }
}
