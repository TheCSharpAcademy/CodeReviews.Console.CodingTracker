namespace CodingTracker.jkjones98
{
    internal class ReportMenu
    {
        internal void DisplayReportMenu()
        {
            ReportInputs reports = new();
            MainMenu mainMenu = new();

            bool closeApp = false;
            while(!closeApp)
            {
                Console.WriteLine("\n\nREPORT MENU");
                Console.WriteLine("\nSelect the report you would like to be displayed\n");
                Console.WriteLine("Enter t for the total number of sessions");
                Console.WriteLine("Enter s for the combined sum of hours");
                Console.WriteLine("Enter a for the average number of hours each session");
                Console.WriteLine("Enter 0 to return to the main menu");
                string reportChoice = Console.ReadLine();
                switch(reportChoice)
                {
                    case "t":
                        string count = "SELECT COUNT(Duration) FROM coding";
                        string countText = "Total number of sessions";
                        reports.GetReport(count, countText);
                        break;
                    case "s":
                        string sum = "SELECT SUM (Duration) FROM coding";
                        string sumText = "Total hours spent coding";
                        reports.GetReport(sum, sumText);
                        break;
                    case "a":
                        string avg = "SELECT AVG (Duration) FROM coding";
                        string avgText = "Average number of hers per session";
                        reports.GetReport(avg, avgText);
                        break;
                    case "0":
                        mainMenu.DisplayMenu();
                        break;  
                    default:
                        Console.Clear();
                        Console.WriteLine("\nInvalid report selection, please try again.");
                        DisplayReportMenu();
                        break;
                }
            }
        }
    }
}