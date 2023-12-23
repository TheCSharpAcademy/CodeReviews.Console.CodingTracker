namespace CodingTracker.Chad1082.Data
{
    static internal class Validation
    {
        internal static DateTime ValidateStartDate(ref string startDateEntry)
        {
            DateTime tmpStartDate;
            while (!DateTime.TryParse(startDateEntry, out tmpStartDate))
            {
                Console.Write("Unrecognised input, Enter in dd/mm/yyyy hh:mm format: ");
                startDateEntry = Console.ReadLine();
            }
            return tmpStartDate;
        }

        internal static DateTime ValidateEndDate(DateTime startDate, ref string endDateEntry)
        {
            DateTime endDate;
            while (!DateTime.TryParse(endDateEntry, out endDate) || endDate < startDate)
            {
                string msg = "Unrecognised input, Enter in dd/mm/yyyy hh:mm format: ";

                if (endDate < startDate)
                {
                    msg = "The end date cannot be before the start date. Enter in dd/mm/yyyy hh:mm format: ";
                }
                Console.Write(msg);
                endDateEntry = Console.ReadLine();
            }
            return endDate;
        }

        internal static void ValidateInt(ref string selection, ref int sessionID)
        {
            while (selection.ToLower() != "q" && !int.TryParse(selection, out sessionID))
            {
                Console.Write("Unrecognised input, which entry would you like to Update?: ");
                selection = Console.ReadLine();
            }
        }
    }
}
