using CodingTracker.Golvi1124.Models;

namespace CodingTracker.Golvi1124.Data;

internal static class SeedData // Leaving this for myself
{
    internal static void SeedRecords(int count)
    {
        Random random = new Random();
        DateTime currentDate = DateTime.Now.Date; // Start with today's date

        List<CodingRecord> records = new List<CodingRecord>();

        for (int i = 1; i <= count; i++)
        {
            DateTime startDate = currentDate.AddHours(random.Next(13));
            DateTime endDate = startDate.AddHours(random.Next(13));

            records.Add(new CodingRecord
            {
                Id = i,
                DateStart = startDate,
                DateEnd = endDate,
            });

            // Increment the date for the next record
            currentDate = currentDate.AddDays(1);
        }

        var dataAccess = new DataAccess();
        dataAccess.BulkInsertRecords(records);
    }
}