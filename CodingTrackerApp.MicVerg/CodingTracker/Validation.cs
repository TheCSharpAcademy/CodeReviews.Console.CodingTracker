namespace CodingTracker
{
    internal class Validation
    {
        internal static DateTime ParseAndValidateDate(string message)
        {
            DateTime parsedDate = DateTime.MinValue;
            bool isValidDate = false;
            while (isValidDate == false)
            {
                Console.WriteLine();
                Console.WriteLine(message);
                string dateInput = Console.ReadLine();

                if (dateInput == "0")
                {
                    TableVisualisationEngine.MainMenu();
                }
                else if (string.IsNullOrEmpty(dateInput) || !DateTime.TryParseExact(dateInput, "d/M/yyyy H:mm", null, System.Globalization.DateTimeStyles.None, out parsedDate))
                {
                    Console.WriteLine("Invalid input. Please enter a date with format d/M/yyyy HH:mm, or 0 to cancel.");
                }
                else
                {
                    isValidDate = true;
                    DateTime output = parsedDate;
                    return output;
                }
            }
            return parsedDate;
        }
    }
}
