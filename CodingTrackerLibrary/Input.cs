namespace CodingTracker
{
    public class Input
    {
        public static Record ParseData(string dateStart, string dateEnd)
        {
            if (Validation.CheckInsert(dateStart, dateEnd))
            {
                DateTime startDate = DateTime.ParseExact(dateStart, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                DateTime endDate = DateTime.ParseExact(dateEnd, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                string duration = DateTime.ParseExact(dateEnd, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture).Subtract(startDate).TotalMinutes.ToString();

                var Record = new Record
                {
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    Duration = duration,
                };

                
                return Record;
            }
            else
            {
                return null;
            }
        }
    }
}
