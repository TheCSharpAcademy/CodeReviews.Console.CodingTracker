namespace coding_tracker
{
    internal class UserInput
    {
        private Menu menu = new Menu();
        public CodingSession CodingSessionInput()
        {
            Console.WriteLine("\nWhen did you start coding? Please follow this format: 'd/M/yyyy H:m' (Type 0 to return to menu)");
            string startTime = Console.ReadLine();
            if (startTime == "0")
            {
                Console.Clear();
                menu.GetUserOption();
            }
            Console.WriteLine("\nWhen did you stop coding? Follow the same previous format: 'd/M/yyyy H:m' (Type 0 to return to menu)");
            string endTime = Console.ReadLine();
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
            if (input == "r")
            {
                Console.Clear();
                menu.GetUserOption();
            }
            int id = Convert.ToInt32(input);

            return id;
        }
    }
}