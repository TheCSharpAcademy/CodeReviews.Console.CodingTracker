using ConsoleTableExt;
using ConsoleUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTracker.csm_stough
{
    public class DataRenderer
    {
        private static int currentPage;
        private static int lastPage;
        private static int resultsPerPage;

        public static void Init()
        {
            resultsPerPage = int.Parse(ConfigurationManager.AppSettings.Get("resultsPerPage"));
            currentPage = 0;
            lastPage = (Database.GetCount() / resultsPerPage);
        }

        public static void DisplayData()
        {
            List<CodingSession> sessions = Database.GetAll(resultsPerPage, currentPage * resultsPerPage);

            Console.Clear();

            ConsoleTableBuilder.From(translateData(sessions))
                .WithTitle($"Coding Records ~ Page {currentPage + 1} of {lastPage + 1}")
                .WithColumn("Start Time", "End Time", "Duration")
                .ExportAndWriteLine(TableAligntment.Left);

            Menu recordsMenu = new Menu("Records Menu");

            if(currentPage != 0)
            {
                recordsMenu.AddOption("F", "First Page...", () => { currentPage = 0; DisplayData(); });
                recordsMenu.AddOption("P", "Previous Page...", () => { currentPage--; DisplayData(); });
            }

            if (currentPage != lastPage)
            {
                recordsMenu.AddOption("N", "Next Page...", () => { currentPage++; DisplayData(); });
                recordsMenu.AddOption("L", "Last Page...", () => { currentPage = lastPage; DisplayData(); });
            }

            recordsMenu.AddOption("B", "Go Back To Main Menu...", () => { });

            recordsMenu.SelectOption(false);
        }

        private static List<List<Object>> translateData(List<CodingSession> sessions)
        {
            List<List<Object>> data = new List<List<Object>>();

            for(int s = 0; s < sessions.Count; s++)
            {
                data.Add(new List<object>());
                data[s].Add(sessions[s].startTime);
                data[s].Add(sessions[s].endTime);
                data[s].Add(sessions[s].duration);
            }

            return data;
        }
    }
}
