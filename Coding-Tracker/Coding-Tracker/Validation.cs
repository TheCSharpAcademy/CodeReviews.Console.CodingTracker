using System.Globalization;

class Validation
{
    public static DateTime ValidateTime(string getTime)
    {

        DateTime result;
        while (!DateTime.TryParseExact(getTime, "HH:mm", null, DateTimeStyles.None, out result))
        {
            Console.WriteLine("Invalid format. Try again (HH:mm): ");
            getTime = Console.ReadLine();
        }

        return result;
    }


    public static int ValidateId(string Input)
    {
        // Console.WriteLine($"Input received: '{Input}'");

        int result;

        while (!int.TryParse(Input, out result) || (result <= 0))
        {
            Console.WriteLine("Please enter a valid id");
            Input = Console.ReadLine();
        }

        return result;

    }



}