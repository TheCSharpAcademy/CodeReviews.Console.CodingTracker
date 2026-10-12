using System.Diagnostics;
using System.Reflection.Metadata;
using CodingTracker;

namespace CodingSession;

internal class DebugTool
{
  internal static void SeedData()
  {
    string[] activities = ["Coding Tracker", "Habit Logger", "Website"];
    DateTime startDate = RandomDateTime();
    Random rand = new();

    for (int i = 0; i < 30; i++)
    {
      int randDay = rand.Next(0, 4);
      int randMinutes = rand.Next(0, 580);
      startDate = startDate.AddDays(randDay).AddMinutes(randMinutes);

      string activity = activities[rand.Next(0, activities.Length)];

      TimeSpan duration = TimeSpan.FromMinutes(rand.Next(5, 440));
      DateTime endDate = startDate.AddMinutes((int)duration.TotalMinutes);

      Database.ConvertAndAdd(activity, startDate, endDate, duration);
    }
  }

  private static DateTime RandomDateTime()
  {
    Random rand = new();

    DateTime startDay = new DateTime(2026, 8, 1);
    int maxRange = (DateTime.Today - startDay).Days + 1;
    DateTime date = startDay.AddDays(rand.Next(0, maxRange));

    return date;
  }
}