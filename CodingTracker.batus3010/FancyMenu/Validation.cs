
namespace Services
{
    public class Validation
    {
        public static bool IsValidDateTime(string input, string expectedFormat)
        {
            if (DateTime.TryParseExact(input, expectedFormat, null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
            {
                return parsedDate <= DateTime.Now;
            }
            return false;
        }
    }
}
