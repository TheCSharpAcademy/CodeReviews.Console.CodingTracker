namespace coding_tracker
{
    internal class UserInput
    {
        private Menu menu = new Menu();
        public CodingSession CodingSessionInput()
        {
            Console.WriteLine("\nWhen did you start coding? Please follow this format: 'MM/DD/YYYY HH:MM:SS' (Type 0 to return to menu)");
            string startTime = Console.ReadLine();
            if (startTime == "0") menu.GetUserOption();
            Console.WriteLine("\nWhen did you stop coding? Follow the same previous format: 'MM/DD/YYYY HH:MM:SS' (Type 0 to return to menu)");
            string endTime = Console.ReadLine();
            if (endTime == "0") menu.GetUserOption();

            return new CodingSession(startTime, endTime);
        }
    }
}