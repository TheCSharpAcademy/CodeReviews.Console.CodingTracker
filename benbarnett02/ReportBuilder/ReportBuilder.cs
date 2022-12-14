using Spectre.Console;
namespace TrackingProgram;
public class ReportBuilder
{
    public static Table ThisWeek()
    {
        List<TrackingData.CodeEntry> allEntries = TrackingData.GetAllCodeRecords();

        Table table = new Table();
        table.AddColumn("Label");
        table.AddColumn("Statistic");

        DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
        sevenDaysAgo = new DateTime(sevenDaysAgo.Year, sevenDaysAgo.Month, sevenDaysAgo.Day, 0, 0, 0);
        List<TrackingData.CodeEntry> entriesInPast7 = new List<TrackingData.CodeEntry>();
        List<double> durationsInPast7 = new List<double>();
        foreach (TrackingData.CodeEntry entry in allEntries)
        {
            if (entry.StartDate >= sevenDaysAgo)
            {
                entriesInPast7.Add(entry);
                durationsInPast7.Add(TrackingData.getDurationOfCodeEntry(entry).TotalSeconds);
            }
        }
        table.AddRow("Total time coding past 7 days", TimeSpan.FromSeconds(durationsInPast7.Sum()).ToString());
        table.AddRow("Number of code entries", entriesInPast7.Count().ToString());
        table.AddRow("Average time spent coding", TimeSpan.FromSeconds(durationsInPast7.Average()).ToString());
        table.AddRow("Amount of time spent not coding :(", TimeSpan.FromSeconds(604800.0 - durationsInPast7.Sum()).ToString());


        return table;
    }

    public static Table AllEntryTable()
    {
        List<TrackingData.CodeEntry> codeEntries = TrackingData.GetAllCodeRecords();
        Table table = new Table();
        table.AddColumn("ID");
        table.AddColumn("Label");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");
        table.Title("[RED]Table of past entries[/]");
        table.Caption("[AQUA]Use advanced reports to select the desired views.[/]");
        table.Border(TableBorder.DoubleEdge);
        if (codeEntries != null)
        {
            foreach (TrackingData.CodeEntry entry in codeEntries)
            {
                table.AddRow(entry.Id.ToString(), entry.Label, entry.StartDate.ToString(), entry.EndDate.ToString(), TrackingData.getDurationOfCodeEntry(entry).ToString("hh\\:mm\\:ss"));
            }
            return table;
        }
        else
        {
            table.AddRow("0", "No Records Found");
            return table;
        }
    }

    public static Table SingleEntryTable(string id)
    {
        Table table = new();
        TrackingData.CodeEntry entry = TrackingData.GetCodeEntry(id);
        table.AddColumn("ID");
        table.AddColumn("Label");
        table.AddColumn("Start Time");
        table.AddColumn("End Time");
        table.AddColumn("Duration");
        table.Title("[RED]Selected Entry to be Modified[/]");
        table.Border(TableBorder.DoubleEdge);
        if (entry != null)
        {
            table.AddRow(entry.Id.ToString(), entry.Label, entry.StartDate.ToString(), entry.EndDate.ToString(), TrackingData.getDurationOfCodeEntry(entry).ToString("hh\\:mm\\:ss"));
            return table;
        }
        else { return null; }
    }
    public static void Advanced()
    {
        Console.Clear();
        Console.WriteLine("\n\n Welcome to advanced reporting. Please select from the list below what you would like to view.");
    }

    public static Table FunStats()
    {
        Table table = new Table();
        table.AddColumn("Label");
        table.AddColumn("Statistic");

        List<TrackingData.CodeEntry> allEntries = TrackingData.GetAllCodeRecords();
        List<double> allDurations = new List<double>();
        foreach (TrackingData.CodeEntry entry in allEntries)
        {
            allDurations.Add(TrackingData.getDurationOfCodeEntry(entry).TotalSeconds);
        }
        TimeSpan averageDuration = TimeSpan.FromSeconds(allDurations.Average());
        table.AddRow("Average Duration of all Coding Entries", averageDuration.ToString());

        DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
        sevenDaysAgo = new DateTime(sevenDaysAgo.Year, sevenDaysAgo.Month, sevenDaysAgo.Day, 0, 0, 0);
        List<TrackingData.CodeEntry> entriesInPast7 = new List<TrackingData.CodeEntry>();
        List<double> durationsInPast7 = new List<double>();
        foreach (TrackingData.CodeEntry entry in allEntries)
        {
            if (entry.StartDate >= sevenDaysAgo)
            {
                entriesInPast7.Add(entry);
                durationsInPast7.Add(TrackingData.getDurationOfCodeEntry(entry).TotalSeconds);
            }
        }
        table.AddRow("Total time coding past 7 days", TimeSpan.FromSeconds(durationsInPast7.Sum()).ToString());
        table.AddRow("Number of records (all time)", allEntries.Count.ToString());
        return table;
    }
}