namespace CodingTracker.Dejmenek.Helpers
{
    public static class RandomDate
    {
        private static readonly Random _random = new Random();

        public static (DateTime startDate, DateTime endDate) GetRandomDateTimes()
        {
            int year = _random.Next(2022, DateTime.Today.Year);
            int month = _random.Next(1, 13);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            int hour = _random.Next(0, 24);
            int minute = _random.Next(0, 60);

            DateTime startDateTime = new DateTime(year, month, day, hour, minute, 0);

            DateTime endDateTime;
            do
            {
                hour = _random.Next(0, 24);
                minute = _random.Next(0, 60);

                endDateTime = new DateTime(year, month, day, hour, minute, 0);
            } while (endDateTime <= startDateTime);

            return (startDateTime, endDateTime);
        }

        public static (DateTime startDate, DateTime endDate) GetRandomDates()
        {
            int year = _random.Next(2022, DateTime.Today.Year);
            int month = _random.Next(1, 13);
            int day = _random.Next(1, DateTime.DaysInMonth(year, month) + 1);

            DateTime startDateTime = new DateTime(year, month, day);

            DateTime endDateTime;
            do
            {
                month = _random.Next(1, 13);
                day = _random.Next(1, DateTime.DaysInMonth(year, month) + 1);

                endDateTime = new DateTime(year, month, day);
            } while (endDateTime <= startDateTime);

            return (startDateTime, endDateTime);
        }
    }
}
