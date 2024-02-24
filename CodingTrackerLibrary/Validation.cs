namespace CodingTracker
{
    public class Validation
    {
        public static bool CheckInsert(string dateStart, string dateEnd)
        {
            bool startIsValid = DateTime.TryParseExact(dateStart,"dd/MM/yyyy HH:mm",System.Globalization.CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None, out DateTime startResult);
            bool endIsValid = DateTime.TryParseExact(dateEnd, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime endResult);

            if (startIsValid & endIsValid)
                return true;

            else
                return false;
        }
    }
}
