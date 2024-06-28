using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker.ukpagrace
{
    internal class UserInput
    {
        public DateTime GetStartDate()
        {
            Console.WriteLine("Input your start time in the format of YYYY-MM-DD HH:MM");
            var dateInput1 = Console.ReadLine();

            DateTime startDate;


            while (!DateTime.TryParseExact(dateInput1, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                Console.WriteLine("Input your start time in the format of YYYY-MM-DD HH:MM");
                dateInput1 = Console.ReadLine();
            }

            return startDate;
        }


        public DateTime GetEndDate()
        {
            Console.WriteLine("Input your end time in the format of YYYY-MM-DD HH:MM");
            var dateInput2 = Console.ReadLine();

            DateTime endDate;
            while (!DateTime.TryParseExact(dateInput2, "yyyy-MM-dd hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                Console.WriteLine("Input your end time in the format of YYYY-MM-DD HH-MM");
                dateInput2 = Console.ReadLine();
            }
            return endDate;
        }


        public int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            var input = Console.ReadLine();

            while (!int.TryParse(input, out _) && Convert.ToInt32(input) < 0)
            {
                Console.WriteLine("Enter a valid integer");
                input = Console.ReadLine();
            }

            return Convert.ToInt32(input);
        }


        public string GetOrderInput()
        {
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"Would you like to order in ascending or descending order")
                    .PageSize(2)
                    .AddChoices("Ascending", "Descending")
            );
            if (option == null) option = "ascending";
            string order = (option == "ascending") ? "ASC" : "DESC";
            return order;
        }
    }
}
