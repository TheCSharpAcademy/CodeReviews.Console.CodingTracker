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

        public static void Main(string[] args)
        {
            Database.Init();

            MainMenu();
        }

        static void MainMenu()
        {
            Menu mainMenu = new Menu("Main Menu\nPlease select an option below");

            mainMenu.AddOption("R", "View Records...", () => { RecordsMenu(); });
            mainMenu.AddOption("N", "Create New Record...", () => { NewRecordMenu(); });
            mainMenu.AddOption("Q", "Quit Program", () => { Environment.Exit(0); });

            mainMenu.SelectOption();
        }

        static void RecordsMenu()
        {
            Menu recordsMenu = new Menu("Record Menu");

            recordsMenu.AddOption("A", "View All Records", () => { AllRecordsMenu(); });
            recordsMenu.AddOption("F", "Filter Records...", () => { FilterRecordsMenu(); });
            recordsMenu.AddOption("B", "Go Back To Main Menu...", () => { MainMenu(); });

            recordsMenu.SelectOption();
        }

        static void FilterRecordsMenu()
        {
            Menu filterMenu = new Menu("Filter Records");

            filterMenu.AddOption("Y", "By Year...", () => { FilterByTimeInterval("%Y"); });
            filterMenu.AddOption("M", "By Month...", () => { FilterByTimeInterval("%Y-%m"); });
            filterMenu.AddOption("D", "By Day...", () => { FilterByTimeInterval("%Y-%m-%d"); });
            filterMenu.AddOption("R", "Between Dates/Times...", () => { });
            filterMenu.AddOption("B", "Go Back To Main Menu...", () => { MainMenu(); });

            filterMenu.SelectOption();
        }

        static void FilterByTimeInterval(string timeFormat)
        {
            ReportsRenderer dataRenderer = new ReportsRenderer(timeFormat, limit: resultsPerPage);
            dataRenderer.DisplayTable();
            FilterRecordsMenu();
        }

        static void AllRecordsMenu()
        {
            //DataRenderer dataRenderer = new DataRenderer(limit: resultsPerPage, between:"Start", low:"'2020-01-01 00:00:00'", high:"'2023-05-06 07:00:00'");
            RecordsRenderer dataRenderer = new RecordsRenderer(limit: resultsPerPage);
            dataRenderer.DisplayTable();
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
                "Please enter dates/times in the format (yyyy-MM-dd hh:mm:ss)\n" +
                "Example: 2015-05-29 05:50:00", 
                (values) =>
            {
                Database.Insert((DateTime)values[0], (DateTime)values[1]);
            });

            manualForm.AddDateTimeQuery("Please enter the starting date/time", "yyyy-MM-dd hh:mm:ss");
            manualForm.AddDateTimeQuery("Please enter the ending date/time", "yyyy-MM-dd hh:mm:ss");

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
                Thread.Sleep(500);
            }
        }
    }
}
