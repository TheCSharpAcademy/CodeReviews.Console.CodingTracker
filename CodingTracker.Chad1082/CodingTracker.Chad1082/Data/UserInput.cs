namespace CodingTracker.Chad1082.Data
{
    static internal class UserInput
    {
        internal static void SessionInput(out string startDateEntry, out string endDateEntry)
        {
            Console.Write("Enter the start date / time for this session. (Enter in dd/mm/yyyy hh:mm format):  ");
            startDateEntry = Console.ReadLine();
            DateTime startDate;
            startDate = Validation.ValidateStartDate(ref startDateEntry);

            Console.Write("When did you finish this session? (Enter in dd/mm/yyyy hh:mm format):  ");
            endDateEntry = Console.ReadLine();
            DateTime endDate;
            endDate = Validation.ValidateEndDate(startDate, ref endDateEntry);
        }
        internal static void SessionSelect(out string selection, out int sessionID, Operation operation)
        {
            Console.WriteLine($"Please enter the number of the session you would like to {operation}\nEnter Q to return to main menu.");

            selection = Console.ReadLine();
            sessionID = 0;
            Validation.ValidateInt(ref selection, ref sessionID);
        }

    }
}
