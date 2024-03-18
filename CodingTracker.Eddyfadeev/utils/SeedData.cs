#if DEBUG
    using CodingTracker.models;
    using CodingTracker.services;

    namespace CodingTracker.utils;

    internal static class SeedData
    {
        internal static void SeedSessions(int count)
        {
            Random random = new();
            DateTime currentDate = DateTime.Now.Date;
            List<Tuple<DateTime, DateTime>> dateRanges = new();
            
            List<CodingSession> sessions = new();

            for (int i = 1; i <= count; i++)
            {
                DateTime startDate = currentDate.AddDays(-random.Next(365)).AddHours(-random.Next(13));
                DateTime endDate = startDate.AddHours(random.Next(13));
                
                dateRanges.Add(new Tuple<DateTime, DateTime>(startDate, endDate));
            }
            dateRanges.Sort((x, y) => x.Item1.CompareTo(y.Item1));

            for (int i = 0; i < count; i++)
            {
                sessions.Add(new CodingSession
                {
                    Id = i,
                    StartTime = dateRanges[i].Item1,
                    EndTime = dateRanges[i].Item2,
                });
            }
            
            var databaseService = new DatabaseService();
            databaseService.BulkInsertSessions(sessions);
        }
    }
#endif