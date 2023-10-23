
using System.Data;
using System.Diagnostics;


namespace CodingTracker.Paul_W_Saltzman
{
    internal static class StopWatch
    {
        internal static CodingSession Timer()
        {
            CodingSession session = new CodingSession();
            Stopwatch stopwatch = new Stopwatch();
            //session.StartTime = DateTime.Now;
            
            TimeSpan ts = stopwatch.Elapsed;
            Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
            Console.Clear();
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Timed Session", typeof(string));
            dataTable.Rows.Add("Elapsed time: " + elapsedTime);
            Helpers.ShowTable(dataTable, "Timed Session");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Helpers.Output("Press Enter to start the coding session.");
            Console.ReadLine();
            stopwatch.Start();
            Console.WriteLine(); session.StartTime = DateTime.Now;

            Task updateElapsedTask = UpdateElapsedTimeAsync(stopwatch);

            Console.ReadLine();

            stopwatch.Stop();
            session.EndTime = DateTime.Now;
            session = CodingSession.SessionTime(session);
               
            ts = stopwatch.Elapsed;
            Console.Clear();
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
            dataTable = new DataTable();
            dataTable.Columns.Add("Timed Session", typeof(string));
            dataTable.Rows.Add("Elapsed time: " + elapsedTime);
            Helpers.ShowTable(dataTable, "Timed Session");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Helpers.Output("Press any key to exit.");
            Console.ReadKey();
         
            return session;
        }

        private static async Task UpdateElapsedTimeAsync(Stopwatch stopwatch)
        {
            while (stopwatch.IsRunning)
            {
                TimeSpan ts = stopwatch.Elapsed;
                Console.WriteLine("\u001b[2J\u001b[3J"); //console clear does not work correctly in windows 11
                Console.Clear();
                Console.SetCursorPosition(0, Console.CursorTop);// Move cursor to the beginning of the line
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("Timed Session", typeof(string));
                dataTable.Rows.Add("Elapsed time: " + elapsedTime);
                Helpers.ShowTable(dataTable, "Timed Session");
                Helpers.CenterText("Stopwatch started. Press Enter to stop.");
                Helpers.CenterCursor();
                await Task.Delay(1000);  // Update every second
            }
        }
    }
}
