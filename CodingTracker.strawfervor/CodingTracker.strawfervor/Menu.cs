using ConsoleTableExt;
using System;
using System.Collections;

namespace CodingTracker
{
    partial class Tracker
    {
        public void MainMenu()
        {
            bool menuLoop = true;
            int userInput = -12;
            string appTitle = "== Coding Tracker ==";

            var menuTable = new List<List<object>>
            {
                new List<object> {"1)", "Show coding diary"},
                new List<object> {"2)", "Add new entry"},
                new List<object> {"3)", "Delete entry"},
                new List<object> {"4)", "Update entry"},
                new List<object> {"5)", "Coding stopper"},
                new List<object> {"0)", "Exit"},
            };

            while (menuLoop)
            {
                Console.WriteLine(appTitle + $"\n");

                //Drawing table using ConsoleTableBuilder
                ConsoleTableBuilder
                    .From(menuTable)
                    .WithTextAlignment(new Dictionary<int, TextAligntment> 
                    {
                        { 0, TextAligntment.Center },
                        { 1, TextAligntment.Left }
                    })
                    .WithMinLength(new Dictionary<int, int>
                    {
                        {0, 4 },
                        {1, 35 }
                    })
                    .WithTitle("  Please choose option  ")
                    .WithCharMapDefinition(CharMapDefinition.FramePipDefinition)
                    .ExportAndWriteLine();

                Console.Write(": ");

                try
                {
                    userInput = int.Parse(Console.ReadLine()!);
                    switch (userInput)
                    {
                        case 1:
                            Console.Clear();
                            ShowAllRecords();
                            break;
                        case 2:
                            Console.Clear();
                            NewEntry();
                            break;
                        case 3:
                            Console.Clear();
                            DeleteEntry();
                            break;
                        case 4:
                            Console.Clear();
                            UpdateEntry();
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Please choose correct option\n");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.WriteLine("Please enter numbers only\n");
                }
            }
        }
    }
}
