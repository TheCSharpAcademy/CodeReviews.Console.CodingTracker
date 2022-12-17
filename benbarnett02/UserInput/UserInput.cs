using System.Globalization;

namespace TrackingProgram;

public class UserInput
{
    public static string format = "dd/MM/yyyy HH:mm tt";
    public CultureInfo culture = CultureInfo.CreateSpecificCulture("en-AU");

    public static DateTime GetDateInput()
    {
        string enteredDateTime;
        DateTime outDateTime;
        Console.ReadLine();
        while (!DateTime.TryParse(enteredStartTime, culture, DateTimeStyles.None, out outDateTime))
        {
            Console.WriteLine("Invalid date/time format. Please try again.");
            enteredStartTime = Console.ReadLine();
        }
        return outDateTime;
    }



}
