using System.Globalization;

namespace CodingTrackerJMS;

public class Validation
{
    int idInteger;
    string id;

    public int GetValidID()
    {
        bool isValid = false;

        while (isValid == false)
        {
            Console.WriteLine("Enter Id of the record: ");
            id = Console.ReadLine();

            if (string.IsNullOrEmpty(id))
            {
                Console.WriteLine("Input is null!");
            }
            else
            {
                if (int.TryParse(id, out idInteger))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Invalid input! The ID must be a valid integer.");
                }
            }
        }

        return idInteger;
    }

    public bool GetValidDate(string inputDate, bool isValid, out DateTime startDateT)
    {
        string format = "yyyy/MM/dd; HH:mm";

            if (DateTime.TryParseExact(inputDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateT))
            {
                isValid = true;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter a date in the correct format (yyyy/mm/dd.");
            }

        return isValid;
    }
}
