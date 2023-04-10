namespace CodingTracker.jwhitt3r
{
    internal class GetUserInput
    {
        CodingController codingController = new();
        internal void MainMenu()
        {
            bool closeApp = false;
            while(closeApp == false)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to Close Application.");
                Console.WriteLine("Type 1 to View records");
                Console.WriteLine("Type 2 to Add records");
                Console.WriteLine("Type 3 to Delete records");
                Console.WriteLine("Type 4 to Update records");
            }

            string commandInput = Console.ReadLine();

            while (string.IsNullOrEmpty(commandInput))
            {
                Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                commandInput = Console.ReadLine();
            }

            switch (commandInput)
            {
                case "0":
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    codingController.Get();
                    break;
                case "2":
                    ProcessAdd();
                    break;
                case "3":
                    ProcessDelete();
                    break;
                case "4":
                    ProcessUpdate();
                    break;
                case "5":
                    ProcessReport();
                    break;

                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }
        }

        private void ProcessReport()
        {
            throw new NotImplementedException();
        }

        private void ProcessUpdate()
        {
            throw new NotImplementedException();
        }

        private void ProcessDelete()
        {
            throw new NotImplementedException();
        }

        private void ProcessAdd()
        {
            throw new NotImplementedException();
        }
    }
}