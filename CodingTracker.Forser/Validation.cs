using System.Globalization;

namespace CodingTracker.Forser
{
    internal class Validation
    {
        public readonly string selectedFormat;
        internal DateOnly startTime;
        internal DateOnly endTime;
        public Validation(string dateTimeFormat)
        {
            selectedFormat = dateTimeFormat;
        }

        public bool ValidateFormat(string startDate, string endDate)
        {
            CultureInfo culture = new CultureInfo("en-US");
            bool isFormatValid = false;

            if (DateTime.TryParseExact(startDate, selectedFormat, culture, DateTimeStyles.None, out DateTime StartDate) && DateTime.TryParseExact(endDate, selectedFormat, culture, DateTimeStyles.None, out DateTime EndDate)) 
            {
                isFormatValid = true;
                if (AreDatesValid(StartDate, EndDate))
                {
                    return true;
                }
            }
            if (!isFormatValid)
            {
                Console.WriteLine($"\n\nDates are invalid, please try again. Input must be in format: {selectedFormat}");
            }
            return false;
        }

        public bool AreDatesValid(DateTime startDate, DateTime endDate)
        {
            if(startDate < endDate)
            {
                return true;
            }
            else
            {
                Console.WriteLine("\n\nEnd Date can't be before Start Date");
                return false;
            }
        }
    }
}