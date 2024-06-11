using CodingTracker.Arashi256.Classes;
using CodingTracker.Arashi256.Controllers;
using CodingTracker.Arashi256.Models;
using Spectre.Console;

namespace CodingTracker.Arashi256.Views
{
    internal class CodingReportsView
    {
        private const int REPORTS_MAIN_MENU_OPTION_NUM = 6;
        private SessionController? _sessionController;
        private Table? _tblReportMenu;
        private string[] _reportOptions =
        {
            "Show coding sessions for the last week",
            "Show coding sessions for the last month",
            "Show coding sessions for the 6 months",
            "Show coding sessions for the last year",
            "Show coding sessions between 2 dates",
            "Return to Main Menu"
        };

        public CodingReportsView(SessionController? sc)
        {
            if (sc != null) 
                _sessionController = sc;
            else
                _sessionController = new SessionController();        
        }

        public void CodingSessionReports()
        {
            _tblReportMenu = new Table();
            _tblReportMenu.AddColumn(new TableColumn("[white]CHOICE[/]").Centered());
            _tblReportMenu.AddColumn(new TableColumn("[white]OPTION[/]").LeftAligned());
            for (int i = 0; i < _reportOptions.Length; i++)
            {
                _tblReportMenu.AddRow($"[lightcyan3]{i + 1}[/]", $"[darkslategray1]{_reportOptions[i]}[/]");
            }
            _tblReportMenu.Alignment(Justify.Center);
            int selectedValue = 0;
            do
            {
                Console.WriteLine("\n");
                AnsiConsole.Render(new Text("R E P O R T S   M E N U").Centered());
                AnsiConsole.Write(_tblReportMenu);
                selectedValue = CommonUI.MenuOption($"Enter a value between 1 and {_reportOptions.Length}: ", 1, _reportOptions.Length);
                ProcessReportMenu(selectedValue);
            } while (selectedValue != REPORTS_MAIN_MENU_OPTION_NUM);
        }

        private void ProcessReportMenu(int option)
        {
            Utility.SortOrder sortOrder = Utility.SortOrder.ASC;
            AnsiConsole.Markup($"[white]Report option selected: {option}[/]\n");
            switch (option)
            {
                case 1:
                    // Show coding sessions for the last week
                    sortOrder = CommonUI.GetSortOrderDialog();
                    DisplayCodingSessions(_sessionController.GetDaysCodingSessions(7, sortOrder));
                    CommonUI.Pause();
                    break;
                case 2:
                    // Show coding sessions for the last month
                    sortOrder = CommonUI.GetSortOrderDialog();
                    DisplayCodingSessions(_sessionController.GetDaysCodingSessions(30, sortOrder));
                    CommonUI.Pause();
                    break;
                case 3:
                    // Show coding sessions for the last 6 months
                    sortOrder = CommonUI.GetSortOrderDialog();
                    DisplayCodingSessions(_sessionController.GetDaysCodingSessions(183, sortOrder));
                    CommonUI.Pause();
                    break;
                case 4:
                    // Show coding sessions for the last year
                    sortOrder = CommonUI.GetSortOrderDialog();
                    DisplayCodingSessions(_sessionController.GetDaysCodingSessions(365, sortOrder));
                    CommonUI.Pause();
                    break;
                case 5:
                    // Show coding sessions between 2 dates
                    DisplayCodingSessionsBetweenDates();
                    break;
            }
        }

        private void DisplayCodingSessionsBetweenDates()
        {
            Utility.SortOrder sortOrder = CommonUI.GetSortOrderDialog();
            DateTime[]? sessionsDateTimes = CommonUI.GetUserCodingSessionDatesDialog();
            if (sessionsDateTimes != null && sessionsDateTimes[0] != null && sessionsDateTimes[1] != null)
            {
                DisplayCodingSessions(_sessionController.GetCodingSessionsBetweenDates(sessionsDateTimes[0], sessionsDateTimes[1], sortOrder));
                CommonUI.Pause();
            }
        }

        private void DisplayCodingSessions(List<CodingSession> codingSessions)
        {
            Table tblSessionList = new Table();
            tblSessionList.AddColumn(new TableColumn("[white]ID[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[white]Start Date/Time[/]").LeftAligned());
            tblSessionList.AddColumn(new TableColumn("[white]End Date/Time[/]").RightAligned());
            tblSessionList.AddColumn(new TableColumn("[white]Duration[/]").RightAligned());
            if (codingSessions.Count > 0)
            {
                for (int i = 0; i < codingSessions.Count; i++)
                {
                    tblSessionList.AddRow($"[white]{i + 1}[/]", $"[white]{codingSessions[i].StartDateTime:dd-MM-yy HH:mm:ss}[/]", $"[white]{codingSessions[i].EndDateTime:dd-MM-yy HH:mm:ss}[/]", $"[white]{codingSessions[i].Duration:hh:mm:ss}[/]");
                }
                tblSessionList.AddRow("", "", "", "");
                tblSessionList.AddRow($"", "", "[skyblue2]Total Time[/]", $"[skyblue1]{Utility.SumCodingSessions(codingSessions.ToArray())}[/]");
                tblSessionList.AddRow($"", "", "[skyblue2]Average Time[/]", $"[skyblue1]{Utility.AverageCodingSessions(codingSessions.ToArray())}[/]");
            }
            else
            {
                tblSessionList.AddRow("[red]No sessions found[/]");
            }
            tblSessionList.Alignment(Justify.Left);
            AnsiConsole.Write(tblSessionList);
        }
    }
}
