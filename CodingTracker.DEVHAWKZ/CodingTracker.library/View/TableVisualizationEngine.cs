using CodingTracker.library.Model;
using ConsoleTableExt;
using System.Data;

namespace CodingTracker.library.View;

internal static class TableVisualizationEngine
{
    private static DataTable GetSessionsHistory(List<CodingSessions> sessions)
    {
        DataTable sessionsTable = new DataTable();
        sessionsTable.Columns.Add("ID", typeof(int));
        sessionsTable.Columns.Add("Start Time", typeof(string));
        sessionsTable.Columns.Add("End Time", typeof(string));
        sessionsTable.Columns.Add("Duration (in minutes)", typeof(double));


        for (int i = 0; i < sessions.Count; i++)
        {
            sessionsTable.Rows.Add(sessions[i].SessionId, sessions[i].SessionStartTime, sessions[i].SessionEndTime, sessions[i].Duration);
        }

        return sessionsTable;
    }

    internal static void PrintSessions(List<CodingSessions> sessions)
    {
        Console.Clear();
        DataTable sessionsHistory = GetSessionsHistory(sessions);

        ConsoleTableBuilder.From(sessionsHistory)
            .WithTitle("Coding Sessions History", ConsoleColor.Black, ConsoleColor.DarkGreen, TextAligntment.Center)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
            {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center },
                {2, TextAligntment.Center },
                {3, TextAligntment.Center }
            })
            .WithMinLength(new Dictionary<int, int>
            {
                {1, 50 },
                {2, 30 }
            })
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                })
            .ExportAndWriteLine(TableAligntment.Center);

        Console.ReadKey();
    }

    private static DataTable GetMenuOptions(Dictionary<string, string> menu)
    {
        DataTable menuTable = new DataTable();

        menuTable.Columns.Add("Command", typeof(string));
        menuTable.Columns.Add("Option", typeof(string));


        foreach (KeyValuePair<string, string> pair in menu)
        {
            menuTable.Rows.Add(pair.Key, pair.Value);
        }



        return menuTable;
    }

    internal static void PrintMenues(Dictionary<string, string> menu, string title)
    {
        Console.Clear();

        DataTable menuTable = GetMenuOptions(menu);

        ConsoleTableBuilder.From(menuTable)
            .WithTitle(title, ConsoleColor.Black, ConsoleColor.DarkGreen, TextAligntment.Center)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
            {
                {0, TextAligntment.Center },
                {1, TextAligntment.Center }    
            })
            .WithMinLength(new Dictionary<int, int>
            {
                {1, 75 },
                {2, 30 }
            })
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                })
            .ExportAndWriteLine(TableAligntment.Center);
    }

    private static DataTable GetQueryValues(int id, string startTime, string endTime, double duration)
    {
        DataTable queryTable = new DataTable();

        queryTable.Columns.Add("ID", typeof(int));
        queryTable.Columns.Add("Start Time", typeof(string));
        queryTable.Columns.Add("End Time", typeof(string));
        queryTable.Columns.Add("Duration (in minutes)", typeof(double));

        queryTable.Rows.Add(id, startTime, endTime, duration);

        return queryTable;
    }

    internal static void PrintQuerieValues(int id, string startTime, string endTime, double duration, string title)
    {
        Console.Clear();

        DataTable queryTable = GetQueryValues(id, startTime, endTime, duration);

        ConsoleTableBuilder.From(queryTable)
            .WithTitle(title, ConsoleColor.Black, ConsoleColor.DarkGreen, TextAligntment.Center)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
            {
              {0, TextAligntment.Center },
              {1, TextAligntment.Center },
              {2, TextAligntment.Center },
              {3, TextAligntment.Center }
            })
            .WithMinLength(new Dictionary<int, int>
            {
                {1, 50 },
                {2, 30 }
            })
            .WithCharMapDefinition(
                CharMapDefinition.FramePipDefinition,
                new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
                })
            .ExportAndWriteLine(TableAligntment.Center);
    }

    private static DataTable GetSingleValue(double avg, string title)
    {
        DataTable queryTable = new DataTable();

        queryTable.Columns.Add(title, typeof(string));
        queryTable.Rows.Add(avg);

        return queryTable;
    }

    internal static void PrintSingleValue(double avg, string title)
    {
        Console.Clear();

        DataTable queryTable = GetSingleValue(avg, title);

        ConsoleTableBuilder.From(queryTable)
          .WithTextAlignment(new Dictionary<int, TextAligntment>
          {
              {0, TextAligntment.Center }
          })
           .WithMinLength(new Dictionary<int, int>
           {
                {1, 50 },
                {2, 30 }
           })
           .WithCharMapDefinition(
               CharMapDefinition.FramePipDefinition,
               new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
               })
           .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("\n\nPress any key to get back to main menu...");
        Console.ReadKey();
    }

    private static DataTable GetGoalStatus(double goalHours, double lastWeekHours, string message)
    {
        DataTable queryTable = new DataTable();

        queryTable.Columns.Add("Coding Goal", typeof(string));
        queryTable.Columns.Add("Total Coding Hours Last Week", typeof (string));
        queryTable.Columns.Add("Message", typeof(string)); 
        
        queryTable.Rows.Add(goalHours, lastWeekHours, message);

        return queryTable;
    }

    internal static void PrintGoalStatus(double goalHours, double lastWeekHours, string message)
    {
        Console.Clear();

        DataTable queryTable = GetGoalStatus(goalHours, lastWeekHours, message);

        ConsoleTableBuilder.From(queryTable)
          .WithTextAlignment(new Dictionary<int, TextAligntment>
          {
              {0, TextAligntment.Center },
              {1, TextAligntment.Center },
              {2, TextAligntment.Center },
              {3, TextAligntment.Center },
              {4, TextAligntment.Center },
              {5, TextAligntment.Center },
              {6, TextAligntment.Center }
          })
           .WithMinLength(new Dictionary<int, int>
           {
                {1, 50 },
                {2, 30 }
           })
           .WithCharMapDefinition(
               CharMapDefinition.FramePipDefinition,
               new Dictionary<HeaderCharMapPositions, char> {
                        {HeaderCharMapPositions.TopLeft, '╒' },
                        {HeaderCharMapPositions.TopCenter, '╤' },
                        {HeaderCharMapPositions.TopRight, '╕' },
                        {HeaderCharMapPositions.BottomLeft, '╞' },
                        {HeaderCharMapPositions.BottomCenter, '╪' },
                        {HeaderCharMapPositions.BottomRight, '╡' },
                        {HeaderCharMapPositions.BorderTop, '═' },
                        {HeaderCharMapPositions.BorderRight, '│' },
                        {HeaderCharMapPositions.BorderBottom, '═' },
                        {HeaderCharMapPositions.BorderLeft, '│' },
                        {HeaderCharMapPositions.Divider, '│' },
               })
           .ExportAndWriteLine(TableAligntment.Center);

        Console.WriteLine("\n\nPress any key to get back to main menu...");
        Console.ReadKey();
    }


}
