using System;
using System.Collections.Generic;
using System.Configuration;
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

        }
    }
}
