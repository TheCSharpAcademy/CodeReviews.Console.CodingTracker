namespace coding_tracker
{
    internal class Validator
    {
        public bool ValidateDateFormat(string start)
        {
            DateTime aux;
            return DateTime.TryParseExact(start, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.AssumeUniversal, out aux);
        }

        public bool ValidateEndDate(string start, string end)
        {
            if (!this.ValidateDateFormat(end))
            {
                Console.WriteLine("The input doesn't follow the specified format (d/M/yyyy HH:mm). Try again:");
                return false;
            } else
            {
                DateTime startDate = DateTime.ParseExact(start, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(end, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);

                bool validEndDate = startDate.CompareTo(endDate) <= 0;
                if (!validEndDate) Console.WriteLine("Error: The date you finished the session is before it started. Try again.");

                return validEndDate;
            }
        }

        public bool ValidateNumber(string input, out int id)
        {
            return Int32.TryParse(input, out id);
        }
    }
}