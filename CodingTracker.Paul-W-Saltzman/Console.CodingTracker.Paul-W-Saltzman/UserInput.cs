
namespace CodingTracker.Paul_W_Saltzman
{
    internal class UserInput
    {
        public static bool CanParseDate(string userInput)
        {
            bool canParse = false;
            DateOnly dateInput = new DateOnly(1900, 01, 01);

            string dateFormat = "MM-dd-yyyy";


            if (DateOnly.TryParseExact(userInput, dateFormat, null, System.Globalization.DateTimeStyles.None, out dateInput))
            {
                canParse = true;
            }

            return canParse;
        }

        public static DateOnly ParseDate(string userInput)
        {

            DateOnly dateInput = new DateOnly(1900, 01, 01);
            string dateFormat = "MM-dd-yyyy";


            if (DateOnly.TryParseExact(userInput, dateFormat, null, System.Globalization.DateTimeStyles.None, out dateInput))
            {
            }

            return dateInput;
        }

        public static bool CanParseTime(string userInput)
        {
            bool canParse = false;
            TimeOnly timeInput = new TimeOnly();
            string timeFormat = "h:mm tt";
            if (TimeOnly.TryParseExact(userInput, timeFormat, null, System.Globalization.DateTimeStyles.None, out timeInput))
            {
                canParse = true;
            }
            return canParse;
        }

        public static TimeOnly ParseTime(string userInput)
        {
            TimeOnly timeInput = new TimeOnly();
            string timeFormat = "h:mm tt";
            if (TimeOnly.TryParseExact(userInput, timeFormat, null, System.Globalization.DateTimeStyles.None, out timeInput))
            { }
            return timeInput;
        }
        public static bool CanParseTimeSpan(string userInput)
        {
            bool canParse = false;
            TimeSpan timeInput = new TimeSpan();
            string timeFormat = "h:mm tt";
            if (TimeSpan.TryParseExact(userInput,"hh\\:mm\\:ss", null, out timeInput))
            {
                canParse = true;
            }
            return canParse;
        }
        public static TimeSpan ParseTimeSpan(string userInput)
        {
            TimeSpan timeInput = new TimeSpan();
            string timeFormat = "h:mm tt";
            if (TimeSpan.TryParseExact(userInput,"hh\\:mm\\:ss", null, out timeInput))
            { }
            return timeInput;
        }
    }
}