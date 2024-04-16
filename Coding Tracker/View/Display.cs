using Spectre.Console;

namespace CodingTracker;

internal class Display
{
    internal static void DisplayMenu()
    {
        Console.Clear();
        var table = new Table();
        AnsiConsole.Markup("[orange1]Hello! What would you like to do?[/]\n");
        table.AddColumn(new TableColumn("[orange1]Option[/]").Centered());
        table.AddColumn(new TableColumn("[orange1]Description[/]").Centered());
        table.AddRow("[orange1]0[/]", "[orange1]View Record[/]");
        table.AddRow("[orange1]1[/]", "[orange1]Insert Record[/]");
        table.AddRow("[orange1]2[/]", "[orange1]Delete Record[/]");
        table.AddRow("[orange1]3[/]", "[orange1]Update Record[/]");
        table.AddRow("[orange1]4[/]", "[orange1]Start Live Coding Session[/]");
        table.AddRow("[orange1]5[/]", "[orange1]Exit Application[/]");
        table.Border = TableBorder.Rounded;
        table.Centered();
        AnsiConsole.Render(table);
    }

    internal static void ExitApplicationMessage()
    {
        AnsiConsole.Markup("[aqua]Thanks for using Goodbye.Press any key to close.[/]");
        Console.ReadLine();
    }

    internal static void InvalidInputMessage()
    {
        AnsiConsole.Markup("[red]Invalid input. Press any key to continue[/]");
        Console.ReadLine();
    }

    internal static void DisplayEndTimeWarning()
    {
        AnsiConsole.Markup("[red]1.End-time can be same as Start-time only in Live sessions otherwise it should be greater than start time.\n2.In case your session spans 2 days eg from Start Time : 23:15 on Date: 2024-03-14 to EndTime : 02:30 on Date: 2024-03-15.Kindly make separate entries for different dates.\nPress any key to continue\n[/]");
        Console.ReadLine();
    }

    internal static void DisplayFutureDateWarning()
    {
        AnsiConsole.Markup("[red]Dates from future are not allowed. Press any key to continue[/]");
        Console.ReadLine();
    }

    internal static void DisplayMaximumSessionTimeExceeded()
    {
        AnsiConsole.Markup("[red]The app allows a maximum coding session of only 9 hours.\nPls enter the EndTime accordingly.\nIf session time exceeds 9 hrs kindly make 2 seperate entries.\nPress any key to continue[/]");
        Console.ReadLine();
    }

    internal static void DisplayRecordInsertedMessage()
    {
        AnsiConsole.Markup("[green4]Record inserted sucessfully.Press any key to continue.\n[/]");
        Console.ReadLine();
    }

    internal static void StartTimeRejected()
    {
        AnsiConsole.Markup("[red]The following start time is a part of another coding slot for this date.\n[/]");
    }

    internal static void EndTimeRejected()
    {
        AnsiConsole.Markup("[red]The following end time is already taken in another coding slot.[/]");
    }

    internal static void GetCodingSessionDateConsoleMessage()
    {
        AnsiConsole.Markup("[yellow3]Enter a date in the format yyyy-MM-dd (e.g., 2024-03-14)\n[/]");
    }

    internal static void GetCodingSessionStartTimeConsoleMessage()
    {
        AnsiConsole.Markup("[yellow3]Enter valid Start-time in the format HH:mm (e.g., 12:30)\n[/]");
    }

    internal static void GetCodingSessionEndTimeConsoleMessage()
    {
        AnsiConsole.Markup("[yellow3]Enter valid End-time in the format HH:mm (e.g., 12:45)\n[/]");
    }

    internal static void NoRecordsFoundConsoleMessage()
    {
        AnsiConsole.Markup("[yellow3]No Records inserted. Kindly Insert some records .Press any key to continue[/]\n");
    }

    internal static void DisplayIndividualRecord(CodingSessionModel record)
    {
        AnsiConsole.Markup("[aqua]Session ID: {0} , Start Time: {1}, End Time: {2}, SessionDuration: {3}, Date: {4}\n[/]", record.SessionId, record.SessionStartTime, record.SessionEndTime, record.SessionDuration, record.SessionCodingDate);
    }

    internal static void DisplayPressKeyToContinue()
    {
        AnsiConsole.Markup("[orange1]Press any key to continue\n[/]");
    }

    internal static void DisplayGetSessionId()
    {
        AnsiConsole.Markup("[yellow3]Enter the session id you want to delete: [/]");
    }

    internal static void DisplayInvalidInput()
    {
        AnsiConsole.Markup("[red]Invalid input pls enter integer value.[/]");
    }

    internal static void DisplayUpdateGetSessionId()
    {
        AnsiConsole.Markup("[yellow3]Enter the session id you want to update: \n[/]");
    }

    internal static void DisplayRecordAlreadyPresent() {
        AnsiConsole.Markup("[aqua]The following records are already present for the specified date:\n[/]");
    }

    internal static void DisplayLiveSessionWarning()
    {
        AnsiConsole.Markup("[yellow3]Are you sure you want to start a live coding sesssion[[y/n]] ?[/]");
    }

    internal static void DisplayLiveSessionStarted()
    {
        AnsiConsole.Markup("[yellow3]Stopwatch started.[/]");
        AnsiConsole.Markup("[yellow3]Your Live Session has started. Press 'q' to stop the session any time.[/]");
    }

    internal static void DisplayLiveSessionStopped()
    {
        AnsiConsole.Markup("[yellow3]Stopwatch stopped.[/]");
        AnsiConsole.Markup("[yellow3]Your Live Session ended and saved successfully.\n[/]");
    }

    internal static void UserInputSessionIdNotPresent(int userInputSessionId, string userInputSessionDate)
    {
        AnsiConsole.Markup("[yellow3]The session id : {0} you entered is not present in the database for the given input date: {1}.Cannot update. Press any key to continue[/]", userInputSessionId, userInputSessionDate);
    }

    internal static void NoRecordsWithDate(string codingDate)
    {
        AnsiConsole.Markup("[yellow3]There are no records with the date : {0} . Nothing to Update.[/]", codingDate);
        Console.ReadLine();
    }

    internal static void UserInputSessionIdUpdatedSuccessMessage(int userInputSessionId)
    {
        AnsiConsole.Markup("[green4]The record with session id : {0} was updated successfully. Press any key to continue.[/]", userInputSessionId);
    }

    internal static void UserInputSessionIdNotPresentToDelete(int userInputSessionId)
    {
        AnsiConsole.Markup("[yellow3]The session id : {0} you entered is not present in the database.Cannot delete. Press any key to continue[/]", userInputSessionId);
    }

    internal static void RecordDeletedSuccessMessage(int userInputSessionId)
    {
        AnsiConsole.Markup("[green4]The record with session id : {0} was deleted successfully. Press any key to continue.[/]", userInputSessionId);
    }
}