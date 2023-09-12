using ConsoleTableExt;
using System;
using System.Collections;
using System.Reflection.PortableExecutable;

namespace CodingTracker
{
    partial class Tracker
    {
        public void ShowRecordSimple()//show record separated by comas
        {
            foreach (var record in AllRecords())
            {
                Console.WriteLine($"{record.Id}, {record.Date}, {record.StartTime}, {record.EndTime}, {record.Duration}");
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        public void ShowTable()
        {
            var recordsList = new List<List<object>>();
            recordsList.Add(new List<object>() { "ID", "Date", "Start time", "End time", "Duration (minutes)" }); //header line add

            foreach (var record in AllRecords())
            {
                recordsList.Add(new List<object>() { record.Id, record.Date, record.StartTime, record.EndTime, record.Duration});
            }

            ConsoleTableBuilder
                .From(recordsList)
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                                    { 0, TextAligntment.Center },
                                    { 1, TextAligntment.Left }
                })
                .WithMinLength(new Dictionary<int, int>
                {
                                    {0, 4 }
                })
                .WithTitle("  Coding tracker  ")
                .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                .ExportAndWriteLine();
        }

        public void ShowAllRecords()
        {
            ShowTable();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
            Console.Clear();
        }
    }
}