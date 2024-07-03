
namespace Services
{
    public class Validation
    {
        public static bool IsValidDateTime(string input, string expectedFormat)
        {
            DateTime parsedDate;
            if (DateTime.TryParseExact(input, expectedFormat, null, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                return parsedDate <= DateTime.Now;
            }
            return false;
        }
    }
}
