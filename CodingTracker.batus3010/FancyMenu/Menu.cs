namespace FancyMenu
{
    public class Menu
    {
        private static string[] options = { "New Coding session", "All coding sessions", "About", "Exit" };
        private static int SelectedIndex = 0;
        private static string prefix = "*";

        private static readonly string WelcomeMessage = @"
 ██████╗ ██████╗ ██████╗ ██╗███╗   ██╗ ██████╗     ████████╗██████╗  █████╗  ██████╗██╗  ██╗███████╗██████╗ 
██╔════╝██╔═══██╗██╔══██╗██║████╗  ██║██╔════╝     ╚══██╔══╝██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝██╔══██╗
██║     ██║   ██║██║  ██║██║██╔██╗ ██║██║  ███╗       ██║   ██████╔╝███████║██║     █████╔╝ █████╗  ██████╔╝
██║     ██║   ██║██║  ██║██║██║╚██╗██║██║   ██║       ██║   ██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██╔══██╗
╚██████╗╚██████╔╝██████╔╝██║██║ ╚████║╚██████╔╝       ██║   ██║  ██║██║  ██║╚██████╗██║  ██╗███████╗██║  ██║
 ╚═════╝ ╚═════╝ ╚═════╝ ╚═╝╚═╝  ╚═══╝ ╚═════╝        ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝                                                                                                
Welcome to the Coding Tracker
Use the Up and Down arrow key to cycle through options, press Enter to select";

        public static void RunMainMenu()
        {
            int selectedIndex = Run();
            switch (selectedIndex)
            {
                case 0:
                    Console.WriteLine("New Coding session()");
                    // Add new coding session
                    break;
                case 1:
                    Console.WriteLine("All coding sessions()");
                    // Display all coding sessions
                    break;
                case 2:
                    // Display about information
                    About();
                    break;
                case 3:
                    // Exit
                    ExitProgram();
                    break;

            }
        }

        private static void ExitProgram()
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
        private static void Display()
        {
            Console.WriteLine(WelcomeMessage);
            for (int i = 0; i < options.Length; i++)
            {
                string currentOption = options[i];

                if (i == SelectedIndex)
                {
                    prefix = "*";
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                else
                {
                    prefix = " ";
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                }

                Console.WriteLine($"{prefix} << {currentOption} >>");
            }
            Console.ResetColor();
        }

        private static int Run()
        {
            ConsoleKey keyPressed;
            do
            {
                Console.Clear();
                Display();
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                // update SelectedIndex base on key pressed
                if (keyPressed == ConsoleKey.DownArrow)
                {
                    SelectedIndex++;
                    if (SelectedIndex >= options.Length)
                    {
                        SelectedIndex = 0;
                    }
                }
                else if (keyPressed == ConsoleKey.UpArrow)
                {
                    SelectedIndex--;
                    if (SelectedIndex < 0)
                    {
                        SelectedIndex = options.Length - 1;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);
            return SelectedIndex;
        }
        private static void About()
        {
            Console.Clear();
            Console.WriteLine("About Coding Tracker");
            Console.WriteLine("====================");
            Console.WriteLine("Version: 1.0.0");
            Console.WriteLine("Author: batus");
            Console.WriteLine("Description: This application helps you track your coding sessions and manage your productivity efficiently.");
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey(true);
            RunMainMenu();
        }
    }
}
