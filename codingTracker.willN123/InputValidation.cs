using System.Globalization;

namespace CodingTracker
{
    internal class InputValidation
    {
        public bool ValidDateInput(string input)
        {
            if(!DateTime.TryParseExact(input, "dd-MM-yy", new CultureInfo("en-AU"), DateTimeStyles.None, out _))
            {
                return false;
            }

            return true;
        }

        public bool ValidTimeInput(string input)
        {
            if(!TimeOnly.TryParseExact(input, "HHmm", out _))
            {
                return false;
            }

            return true;
        }

        public bool ValidIdInput(List<CodingSession> sessions, string input)
        {
            foreach(var session in sessions)
            {
                if(input == session.id.ToString())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
