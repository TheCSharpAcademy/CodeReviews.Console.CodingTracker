using System.Globalization;
namespace CodingTracker.JaegerByte
{
    internal class Validation
    {
        public bool CheckDateInput(string startTime, string endTime)
        {
            DateTime start;
            DateTime end;

            if (DateTime.TryParseExact(startTime, "dd-MM-yyyy HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out start))
            {
                if (DateTime.TryParseExact(endTime, "dd-MM-yyyy HH:mm", new CultureInfo("de-DE"), DateTimeStyles.None, out end))
                {
                    if (end>start)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckIntInput(string input)
        {
            int result;
            if (Int32.TryParse(input, out result))
            {
                return true;
            }
            return false;
        }

        public bool CheckIndexExists(List<CodingSession> sessionList, string index)
        {
            int intindex = Convert.ToInt32(index);
            foreach (var item in sessionList)
            {
                if (item.ID == intindex)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetInvalidResponse()
        {
            return "invalid input. Press ANY key to return to the menu";
        }
    }
}
