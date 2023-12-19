using LucianoNicolasArrieta.CodingTracker;

namespace coding_tracker
{
    internal class UserInput
    {
        private Menu menu = new Menu();
        private Validator validator = new Validator();
        public CodingSession CodingSessionInput()
        {
            Console.WriteLine("\nWhen did you start coding? Please follow this format: 'd/M/yyyy HH:mm' (Type 0 to return to menu)");
            string startTime = Console.ReadLine();
            while (!validator.ValidateDateFormat(startTime) && !startTime.Equals("0"))
            {
                Console.WriteLine("The input doesn't follow the specified format (d/M/yyyy HH:mm). Try again:");
                startTime = Console.ReadLine();
            }
            if (startTime == "0")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            Console.WriteLine("\nWhen did you stop coding? Follow the same previous format: 'd/M/yyyy HH:mm' (Type 0 to return to menu)");
            string endTime = Console.ReadLine();
            while (!validator.ValidateEndDate(startTime, endTime) && !endTime.Equals("0"))
            {
                endTime = Console.ReadLine();
            }
            if (endTime == "0")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            return new CodingSession(startTime, endTime);
        }

        public int IdInput()
        {
            Console.Write("Please enter the ID of the record you want to select (type r to return to menu): ");
            string input = Console.ReadLine();
            int id;
            while (!validator.ValidateNumber(input, out id) && !input.Equals("r"))
            {
                Console.WriteLine("The input isn't a number. Try again:");
                input = Console.ReadLine();
            }
            if (input == "r")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            return id;
        }

        internal string DateInput()
        {
            Console.WriteLine("Please enter a date that follows this format: 'd/M/yyyy HH:mm' (Type 0 to return to menu)");
            string date = Console.ReadLine();
            while (!validator.ValidateDateFormat(date) && !date.Equals("0"))
            {
                Console.WriteLine("The input doesn't follow the specified format (d/M/yyyy H:m). Try again:");
                date = Console.ReadLine();
            }
            
            if (date == "0")
            {
                Console.Clear();
                menu.GetUserOption();
            }

            return date;
        }

        internal void GoalInput()
        {
            Console.WriteLine("How many hours would you like to code this month?");
            string time = Console.ReadLine();
            int goal;
            while (!Int32.TryParse(time, out goal))
            {
                Console.WriteLine("Please enter a number. Try again");
                time = Console.ReadLine();
            }

            GoalInfo.Default.Goal = goal;
            GoalInfo.Default.Month = DateTime.Now.Month.ToString();
            GoalInfo.Default.Save();

            Console.Clear();
            Console.WriteLine("Goal set successfully.");
        }

        internal string OrderInput()
        {
            Console.WriteLine("How do you want to order the results: asc for ascendant or desc for descendant");
            string order = Console.ReadLine().Trim().ToUpper();
            while (order != "ASC" && order != "DESC")
            {
                Console.WriteLine("Error: Please type ASC or DESC.");
                order = Console.ReadLine().Trim().ToUpper();
            }

            return order;
        }
    }
}