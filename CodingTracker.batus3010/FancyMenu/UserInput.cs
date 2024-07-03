

namespace Services
{
    public class UserInput
    {
        public static DateTime GetDateTimeFromUser(string prompt, string format= "yyyy-MM-dd HH:mm")
        {
            Console.WriteLine($"{prompt} (Format: {format})");
            string? input = Console.ReadLine();

            while (!Validation.IsValidDateTime(input, format))
            {
                Console.WriteLine($"Invalid format. Please use '{format}'. Try again:");
                input = Console.ReadLine();
            }

            // At this point, input is guaranteed to be in the correct format.
            return DateTime.ParseExact(input, format, null);
        }


    }
}
