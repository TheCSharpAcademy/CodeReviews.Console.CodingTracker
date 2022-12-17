using System.Globalization;

namespace TrackingProgram;

public class UserInput
{
    public static string format = "dd/MM/yyyy HH:mm tt";
    public static CultureInfo culture = CultureInfo.CreateSpecificCulture("en-AU");

    public static DateTime GetDateInput()
    {
        string enteredDateTime;
        DateTime outDateTime;
        enteredDateTime=Console.ReadLine();
        while (!DateTime.TryParse(enteredDateTime, culture, DateTimeStyles.None, out outDateTime))
        {
            Console.WriteLine("Invalid date/time format. Please try again.");
            enteredDateTime = Console.ReadLine();
        }
        return outDateTime;
    }



}
