namespace CodingTracker
{
    public class Input
    {
        // Converts string to DateTime to calculate duration, returns an new instance of Model.Record.
        public static Record NewRecord(string dateStart, string dateEnd)
        {
            try
            {
                DateTime startDate = DateTime.ParseExact(dateStart, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                // (DateTime endDate - DateTime startDate) -> Total Minutes -> Parse TimeSpan.TotalMinutes -> Decimal
                decimal duration = Decimal.Parse(DateTime.ParseExact(dateEnd, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture).Subtract(startDate).TotalMinutes.ToString());

                var Record = new Record
                {
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    Duration = $"{duration:N0}",
                };
                return Record;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
