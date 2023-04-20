namespace coding_tracker
{
    internal class Validator
    {
        public bool ValidateStartDate(string start)
        {
            DateTime aux;
            return DateTime.TryParseExact(start, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.AssumeUniversal, out aux);
        }

        public bool ValidateEndDate(string start, string end)
        {
            DateTime aux;
            if (!DateTime.TryParseExact(end, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.AssumeUniversal, out aux))
            {
                Console.WriteLine("The input doesn't follow the specified format (d/M/yyyy HH:mm). Try again:");
                return false;
            } else if (DateTime.ParseExact(start, "d/M/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture) <= aux)
            {
                Console.WriteLine("Error: The date you finish the session is before it starts. Try again.");
                return false;
            } else
            {
                return true;
            }
        }

        public bool ValidateNumber(string input, out int id)
        {
            return Int32.TryParse(input, out id);
        }
    }
}