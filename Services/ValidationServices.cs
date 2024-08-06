using Coding_Tracking_Application.DataModels;
using Spectre.Console;

namespace Coding_Tracking_Application.Services;

public class ValidationServices()
{
    public static void ParseUserMenuInput(string userMenuInput)
    {
        UserInput UserInput = new UserInput();
        DatabaseServices DatabaseServices = new();

        if (int.TryParse(userMenuInput, out int parsedUserInput))
        {
            switch (parsedUserInput)
            {
                case 0: Console.Clear(); break;
                case 1: DatabaseServices.GetSessionList(); break;
                case 2: UserInput.CreateEntryInput(); break;
                case 3: DatabaseServices.DeleteEntry(); break;
                case 4: UserInput.AddSessionInput(); break;
                default: Console.Clear(); Console.WriteLine("\n\nSorry, that input is not recognised. Please try again\n\n"); UserInput.MainMenuOptions(); break;
            }
        }
        else
        {
            UserInput.MainMenuOptions();
        }
    }

    public static void ParseUserDateInput(string startDateTimeAsString, string endDateTimeAsString)
    {
        if (DateTime.TryParse(startDateTimeAsString, out DateTime parsedStartDateTime) && DateTime.TryParse(endDateTimeAsString, out DateTime parsedEndDateTime))
        {
            CodingSession session = new CodingSession();
            session.StartTime = parsedStartDateTime;
            session.EndTime = parsedEndDateTime;
            session.CodingTime = ValidationServices.CodingSessionDuration(parsedStartDateTime, parsedEndDateTime);

            DatabaseServices.CreateEntry(session);
        }
        else
        {
            AnsiConsole.Markup("[red]\n\nPlease re-enter your Start and End date & time with the correct format.\n\n\n\n[/]");
            UserInput.CreateEntryInput();
        }
    }

    public static string CodingSessionDuration(DateTime parsedStartDateTime, DateTime parsedEndDateTime)
    {
        TimeSpan timeAndDateDifference = parsedEndDateTime - parsedStartDateTime;
        return timeAndDateDifference.ToString();
    }
}
