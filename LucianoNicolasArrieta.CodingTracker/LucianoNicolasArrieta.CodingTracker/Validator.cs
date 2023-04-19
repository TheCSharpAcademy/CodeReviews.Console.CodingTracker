namespace coding_tracker
{
    internal class Validator
    {
        public bool ValidateDate(string input)
        {
            DateTime aux;
            return DateTime.TryParseExact(input, "d/M/yyyy H:m", null, System.Globalization.DateTimeStyles.AssumeUniversal, out aux);
        }

        public bool ValidateNumber(string input, out int id)
        {
            return Int32.TryParse(input, out id);
        }
    }
}