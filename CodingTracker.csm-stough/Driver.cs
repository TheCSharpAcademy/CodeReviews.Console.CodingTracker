using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTableExt;
using ConsoleUtilities;

namespace CodeTracker.csm_stough
{
    internal class Driver
    {

        private static int resultsPerPage = int.Parse(ConfigurationManager.AppSettings.Get("resultsPerPage"));
        private static Stopwatch timer;

        public static void Main(string[] args)
        {
            Database.Init();
            DataRenderer.Init();

            MainMenu();
        }

        static void MainMenu()
        {
            Menu mainMenu = new Menu("Main Menu\nPlease select an option below");

            mainMenu.AddOption("A", "View All Records...", () => { AllRecordsMenu(); });
            mainMenu.AddOption("N", "Create New Record...", () => { NewRecordMenu(); });
            mainMenu.AddOption("Q", "Quit Program", () => { Environment.Exit(0); });

            mainMenu.SelectOption();
        }

        static void AllRecordsMenu()
        {
            DataRenderer.DisplayData();
            MainMenu();
        }

        static void NewRecordMenu()
        {
            Menu newRecordMenu = new Menu("Create A New Record");

            newRecordMenu.AddOption("M", "Manual Entry", () => { ManualRecordEntry(); });
            newRecordMenu.AddOption("S", "Stopwatch Entry", () => { StopwatchMenu(); });
            newRecordMenu.AddOption("B", "Go Back To Main Menu", () => { MainMenu(); });

            newRecordMenu.SelectOption();
        }

        static void ManualRecordEntry()
        {
            Form manualForm = new Form(
                "Manual Record Entry Form.\n" +
                "Please enter dates/times in the format (MM/dd/yyyy hh:mm tt)\n" +
                "Example: 05/29/2015 05:50 AM", 
                (values) =>
            {
                Database.Insert((DateTime)values[0], (DateTime)values[1]);
            });

            manualForm.AddDateTimeQuery("Please enter the starting date/time", "MM/dd/yyyy hh:mm tt");
            manualForm.AddDateTimeQuery("Please enter the ending date/time", "MM/dd/yyyy hh:mm tt");

            manualForm.Start();

            NewRecordMenu();
        }

        static async void StopwatchMenu()
        {
            DateTime start = DateTime.Now;
            StartClock(start);
            DateTime end = DateTime.Now;

            Console.WriteLine("Stopwatch ended...");

            Form stopwatchForm = new Form($"You spent {(end - start).ToString(@"hh\:mm")} coding!", (values) =>
            {
                if (values[0].ToString().ToLower() == "yes")
                {
                    Database.Insert(start, end);
                }
            });

            stopwatchForm.AddChoiceQuery("Would you like to log this time?", "Yes", "No");

            stopwatchForm.Start();

            NewRecordMenu();
        }

        static void StartClock(DateTime start)
        {
            string startTimeLabel = $"Starting Date/Time: {start.ToString("MM/dd/yyyy hh:mm:ss tt")}";
            bool completed = false;

            Task.Run(() =>
            {
                ConsoleKeyInfo cki = new ConsoleKeyInfo();

                do
                {
                    cki = Console.ReadKey(true);
                } while (cki.Key != ConsoleKey.Enter);

                completed = true;
            });

            while (!completed)
            {
                Console.Clear();
                Console.WriteLine(startTimeLabel);
                Console.WriteLine($"Current Date/Time: {DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")}");
                Console.WriteLine("\nPress 'Enter' to end the stopwatch...");
                Thread.Sleep(1000);
            }
        }
    }
}
