namespace CodingTracker.iGoodw1n;

public static class StringsFor
{
    public const string Menu =
@"
        Main Menu

What would you like to do?

Type 0 to Close Application.
Type 1 to View All Records.
Type 2 to Insert Record.
Type 3 to Delete Record.
Type 4 to Update Record.
Type 5 to View Annual Report.
-----------------------------
";
    private const string TimeQuery = "\nPlease enter the time you {0} your coding session " +
        "(Format: dd-MM-yy_HH:mm) or leave blank to store the current time. " +
        "Enter 0 to cancel operation and return to Main Menu: ";

    public static string EndTimeQuery => string.Format(TimeQuery, "ended");
    public static string StartTimeQuery => string.Format(TimeQuery, "started");
}
