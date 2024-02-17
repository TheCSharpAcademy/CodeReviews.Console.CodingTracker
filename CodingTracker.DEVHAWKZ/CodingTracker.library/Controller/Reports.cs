using CodingTracker.library.View;

namespace CodingTracker.library.Controller;

internal static class Reports
{
    internal enum MonthsName
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }

    internal static void MaxDuration()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("longest", "to view longest coding session");

            if (getBack) Menu.ReportMenu();

            else
            {
                QueriesDuration.MaxDurationQuery();
                Console.WriteLine("\n\nPress any key to get back to report menu...");
                Console.ReadKey();
                Menu.ReportMenu();

            }
        }



    }

    internal static void MinDuration()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("shortest", "to view shortest coding session");

            if (getBack) Menu.ReportMenu();

            else
            {
                QueriesDuration.MinDurationQuery();
                Console.WriteLine("\n\nPress any key to get back to report menu...");
                Console.ReadKey();
                Menu.ReportMenu();
            }
        }
    }

    internal static void AvgDuration()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("average", "to view average coding session time");

            if (getBack) Menu.ReportMenu();

            else
            {
                QueriesDuration.AvgDurationQuery();
                Menu.ReportMenu();

            }
        }
    }

    internal static void CodingSessionPerYear()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("year", "to view total coding session designated year");

            if (getBack) Menu.ReportMenu();

            else
            {
                string year = Helpers.GetYear();
                QueriesPeriod.CodingSessionPerYearQuery(year);
                Menu.ReportMenu();

            }
        }
    }

    internal static void CodingSessionPerMonth()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("month", "to view total coding session designated month");

            if (getBack) Menu.ReportMenu();

            else
            {
                string month = Helpers.GetMonth();
                string monthName = Helpers.GetMonthName(month);
                string year = Helpers.GetYear();

                QueriesPeriod.CodingSessionPerMonthQuery(year, month, monthName);
                Menu.ReportMenu();

            }
        }
    }

    internal static void CodingSessionPerDay()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("day", "to view total coding session designated day");

            if (getBack) Menu.ReportMenu();

            else
            {

                string month = Helpers.GetMonth();
                string monthName = Helpers.GetMonthName(month);
                string day = Helpers.GetDay(monthName);
                string year = Helpers.GetYear();

                QueriesPeriod.CodingSessionPerDayQuery(year, month, day, monthName);
                Menu.ReportMenu();

            }
        }

    }

    internal static void CodingSessionPerWeek()
    {
        Console.Clear();

        int rowCount = QueriesCrud.ViewAllSessionsQuery("query");

        if (rowCount > 0)
        {
            bool getBack = Helpers.GetBackToReportMenu("week", "to view total coding session for designated week");

            if (getBack) Menu.ReportMenu();

            else
            {
                string month = Helpers.GetMonth();
                string monthName = Helpers.GetMonthName(month);
                string day = Helpers.GetDay(monthName);
                string year = Helpers.GetYear();

                QueriesPeriod.CodingSessionPerWeekQuery(year, month, day, monthName);
                Menu.ReportMenu();

            }
        }
    }

}
