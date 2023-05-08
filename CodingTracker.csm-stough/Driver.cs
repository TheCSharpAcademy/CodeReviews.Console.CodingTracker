using System;
using System.Collections;
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
        private static string dateFormat = ConfigurationManager.AppSettings.Get("dateFormat");

        public static void Main(string[] args)
        {
            Database.Init();

            MainMenu();
        }

        static void MainMenu()
        {
            Console.Clear();

            Calender calender = new Calender();
            
            Menu mainMenu = new Menu("Main Menu ~~~~~~~~~~~~~~~~~");

            mainMenu.AddOption("L", "Logs...", () => { RecordsMenu(); });
            mainMenu.AddOption("N", "New Log...", () => { NewRecordMenu(); });
            mainMenu.AddOption("G", "Goals...", () => { });
            mainMenu.AddOption("Q", "Quit Program", () => { Environment.Exit(0); });

            mainMenu.SelectOption(false);
        }

        static void RecordsMenu()
        {
            Menu recordsMenu = new Menu("Logs");

            recordsMenu.AddOption("A", "View All Logs", () => { AllRecordsMenu(); });
            recordsMenu.AddOption("F", "Filter Logs...", () => { FilterRecordsMenu(); });
            recordsMenu.AddOption("B", "Go Back To Main Menu...", () => { MainMenu(); });

            recordsMenu.SelectOption();
        }

        static void FilterRecordsMenu()
        {
            Menu filterMenu = new Menu("Filter Logs");

            filterMenu.AddOption("Y", "By Year...", () => { FilterByTimeInterval("%Y", "Year"); });
            filterMenu.AddOption("M", "By Month...", () => { FilterByTimeInterval("%Y-%m", "Month"); });
            filterMenu.AddOption("D", "By Day...", () => { FilterByTimeInterval("%Y-%m-%d", "Day"); });
            filterMenu.AddOption("R", "Between Dates/Times...", () => { FilterBetweenDates(); });
            filterMenu.AddOption("B", "Go Back To Main Menu...", () => { MainMenu(); });

            filterMenu.SelectOption();
        }

        static void FilterByTimeInterval(string timeFormat, string unit)
        {
            ReportsRenderer reportsRenderer = new ReportsRenderer(timeFormat, unit, null, null, resultsPerPage);
            reportsRenderer.DisplayTable();
            FilterRecordsMenu();
        }

        /**************OLD CODE *******************/


        static void FilterBetweenDates()
        {
            DateTime[] range = null;

            Form betweenDatesForm = new Form("Enter a datetime range", (values) =>
            {
                range = (DateTime[])values[0];
            });
            betweenDatesForm.AddDateTimeRangeQuery("Enter a DateTime", dateFormat);
            betweenDatesForm.Start();
            RecordsRenderer recordRenderer = new RecordsRenderer(
                (limit, offset) =>
                {
                    return new List<Object>(Database.GetAll(limit, offset, "Start BETWEEN '{range[0].ToString(dateFormat)}' AND '{range[1].ToString(dateFormat)}'"));
                },
                () =>
                {
                    return Database.GetCount($"Start BETWEEN '{range[0].ToString(dateFormat)}' AND '{range[1].ToString(dateFormat)}'");
                },
                resultsPerPage);
            recordRenderer.DisplayTable();
            FilterRecordsMenu();
        }

        static void AllRecordsMenu()
        {
            RecordsRenderer dataRenderer = new RecordsRenderer(
                (limit, offset) =>
                {
                    return new List<Object>(Database.GetAll(limit, offset));
                },
                () =>
                {
                    return Database.GetCount();
                },
                resultsPerPage);
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
                $"Please enter dates/times in the format ({dateFormat})\n" +
                "Example: 2015-05-29 05:50:00", 
                (values) =>
            {
                Database.Insert((DateTime)values[0], (DateTime)values[1]);
            });

            manualForm.AddDateTimeQuery("Please enter the starting date/time", dateFormat);
            manualForm.AddDateTimeQuery("Please enter the ending date/time", dateFormat);

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
